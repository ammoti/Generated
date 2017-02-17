// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// CacheService implementation.
	/// </summary>
	public class LocalMemoryCacheService : ICacheService, IDisposable
	{
		#region [ Members ]

		private readonly string _name = null;

		private readonly ConcurrentDictionary<string, CacheItem> _items = new ConcurrentDictionary<string, CacheItem>();
		private readonly object _locker = new object();

		private readonly ICacheStatistics _statistics = new CacheStatistics();

		private readonly CancellationTokenSource _purgeTaskTokenSource = new CancellationTokenSource();

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// 
		public LocalMemoryCacheService()
			: this(Guid.NewGuid().ToString())
		{
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="name"></param>
		public LocalMemoryCacheService(string name = null)
		{
			_name = !string.IsNullOrEmpty(name) ? name : Guid.NewGuid().ToString();

			this.PurgeTaskAsync(); // background task
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~LocalMemoryCacheService()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ ICacheService Implementation ]

		#region [ Synchronous Methods ]

		/// <summary>
		/// Adds a CacheItem object to the cache.
		/// If the key already exists, the cache is updated with the new data.
		/// </summary>
		/// 
		/// <param name="item">
		/// CacheItem object.
		/// </param>

		public bool Add(CacheItem item)
		{
			lock (_locker)
			{
				this.Remove(item.Key);

				return _items.TryAdd(item.Key, item);
			}
		}

		/// <summary>
		/// Adds a new object to the cache with expiration TimeSpan.
		/// If the key already exists, the cache is updated with the new data.
		/// </summary>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <param name="data">
		/// Object to add to the cache.
		/// </param>
		/// 
		/// <param name="timeSpan">
		/// Expiration TimeSpan.
		/// </param>
		public bool Add(string key, object data, TimeSpan timeSpan)
		{
			DateTime expiration = DateTime.MaxValue;

			try
			{
				expiration = DateTime.Now.AddMilliseconds(timeSpan.TotalMilliseconds);
			}
			catch (ArgumentOutOfRangeException)
			{
				if (timeSpan.TotalDays > 0) expiration = DateTime.MaxValue;
				if (timeSpan.TotalDays < 0) expiration = DateTime.Now;
			}

			return this.Add(new CacheItem { Key = key, Data = data, Expiration = expiration });
		}

		/// <summary>
		/// Gets an object from the cache given its unique ID.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the retrieved object.
		/// </typeparam>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <returns>
		/// The retrieved object. If not found returns null.
		/// </returns>
		public T Get<T>(string key) where T : class
		{
			T item = default(T);

			var statistics = _statistics as CacheStatistics;

			statistics.IncrementRequestCount();

			if (_items.ContainsKey(key))
			{
				lock (_locker)
				{
					if (_items.ContainsKey(key))
					{
						if (_items[key].IsExpired)
						{
							this.Remove(key);
						}
						else
						{
							item = TypeHelper.To<T>(_items[key].Data);
						}
					}
				}
			}

			if (item == null)
			{
				statistics.IncrementMissCount();
			}
			else
			{
				statistics.IncrementHitCount();
			}

			return item;
		}

		public T Get<T>(string key, Func<T> whenNull, DateTime expiration) where T : class
		{
			var item = this.Get<T>(key);
			if (item == null)
			{
				item = whenNull();
				if (item != null)
				{
					this.Add(new CacheItem { Key = key, Data = item, Expiration = expiration });
				}
			}

			return item;
		}

		public T Get<T>(string key, Func<T> whenNull, TimeSpan expiration) where T : class
		{
			return this.Get<T>(key, whenNull, DateTime.Now.AddMilliseconds(expiration.TotalMilliseconds));
		}

		/// <summary>
		/// Removes an object from the cache given its unique key.
		/// </summary>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <returns>
		/// True if the object has been removed from the cache; otherwise, false.
		/// </returns>
		public bool Remove(string key)
		{
			if (_items.ContainsKey(key))
			{
				lock (_locker)
				{
					if (_items.ContainsKey(key))
					{
						CacheItem item = null;

						bool bRemoved = _items.TryRemove(key, out item);
						if (!bRemoved)
						{
							var disposer = item as IDisposable;
							disposer.SafeDispose();
						}

						return bRemoved;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Removes all the objects having the given tag value from the cache.
		/// </summary>
		/// 
		/// <param name="tag">
		/// Tag value.
		/// </param>
		/// 
		/// <returns>
		/// The number of objects that have been set as expired.
		/// </returns>
		public int RemoveByTag(string tag)
		{
			int nbItemsRemoved = 0;

			lock (_locker)
			{
				var keys = _items.Keys.ToList();

				for (int i = 0; i < keys.Count; i++)
				{
					var item = _items[keys[i]];

					if (item.Tag == tag && !item.IsExpired)
					{
						this.Remove(item.Key);

						nbItemsRemoved++;
					}
				}
			}

			return nbItemsRemoved;
		}

		#endregion

		#region [ Asynchronous Methods ]

		public async Task<bool> AddAsync(CacheItem cacheItem)
		{
			return await Task.Run<bool>(() => this.Add(cacheItem));
		}

		public async Task<bool> AddAsync(string key, object data, TimeSpan timeSpan)
		{
			return await Task.Run<bool>(() => this.Add(key, data, timeSpan));
		}

		public async Task<T> GetAsync<T>(string key) where T : class
		{
			return await Task.Run<T>(() => this.Get<T>(key));
		}

		public async Task<T> GetAsync<T>(string key, Func<T> whenNull, DateTime expiration) where T : class
		{
			var item = await this.GetAsync<T>(key);
			if (item == null)
			{
				item = whenNull();
				if (item != null)
				{
					await this.AddAsync(new CacheItem { Key = key, Data = item, Expiration = expiration });
				}
			}

			return item;
		}

		public async Task<T> GetAsync<T>(string key, Func<T> whenNull, TimeSpan expiration) where T : class
		{
			return await this.GetAsync<T>(key, whenNull, DateTime.Now.AddMilliseconds(expiration.TotalMilliseconds));
		}

		public async Task<bool> RemoveAsync(string key)
		{
			return await Task.Run<bool>(() => this.Remove(key));
		}

		public async Task<int> RemoveByTagAsync(string key)
		{
			return await Task.Run<int>(() => this.RemoveByTag(key));
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the cache statistics.
		/// </summary>
		public ICacheStatistics Statistics
		{
			get
			{
				var statistics = _statistics as CacheStatistics;

				statistics.Info = new Dictionary<string, CacheItemDisplay>();
				statistics.ItemCount = (_items != null) ? _items.Count : 0;

				foreach (var item in _items)
				{
					statistics.Info.Add(item.Key, new CacheItemDisplay(item.Value));
				}

				return statistics;
			}
		}

		#endregion

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Executes the background task that removes expired items from the cache.
		/// </summary>
		private Task PurgeTaskAsync()
		{
			return Task.Factory.StartNew((parameter) =>
			{
				int counter = 0;
				const int delay = 500;

				while (true)
				{
					if (_purgeTaskTokenSource.Token.IsCancellationRequested)
					{
						break;
					}

					try
					{
						Thread.Sleep(TimeSpan.FromMilliseconds(delay));
					}
					catch (ThreadAbortException)
					{
						// Nothing.
					}

					if (_purgeTaskTokenSource.Token.IsCancellationRequested)
					{
						break;
					}

					if (++counter == 30 /* = 30 iterations -> 30 * 500ms -> 15secs */)
					{
						counter = 0;

						lock (_locker)
						{
							foreach (string key in _items.Values.Where(v => v.IsExpired).Select(v => v.Key))
							{
								CacheItem cacheItem = null;
								_items.TryRemove(key, out cacheItem);

								cacheItem.SafeDispose();
							}
						}
					}
				}
			}, _purgeTaskTokenSource);
		}

		#endregion

		#region [ IDisposable Implementation ]

		private bool _isDisposed = false;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// 
		/// <param name="disposing">
		/// For internal use.
		/// </param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_tokenSource", Justification = "SafeDispose()")]
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_purgeTaskTokenSource.Cancel();
				_purgeTaskTokenSource.SafeDispose();

				lock (_locker)
				{
					IEnumerator<CacheItem> iterator = _items.Values.GetEnumerator();

					while (iterator.MoveNext())
					{
						IDisposable disposer = iterator.Current as IDisposable;
						disposer.SafeDispose();
					}

					_items.Clear();
					_isDisposed = true;
				}
			}
		}

		#endregion
	}
}
