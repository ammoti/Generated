// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Threading.Tasks;

	/// <summary>
	/// Quick access to the ICacheService defined in the configuration file (ServiceLocator definition).
	/// </summary>
	public class CacheServiceHelper : Singleton<CacheServiceHelper>, ICacheService
	{
		#region [ Members ]

		private static readonly ICacheService _cacheService = ServiceLocator.Current.Resolve<ICacheService>();

		#endregion

		#region [ Constructors ]

		static CacheServiceHelper()
		{
		}

		~CacheServiceHelper()
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
		/// <param name="cacheItem">
		/// CacheItem object.
		/// </param>
		public bool Add(CacheItem cacheItem)
		{
			return _cacheService.Add(cacheItem);
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
			return _cacheService.Add(key, data, timeSpan);
		}

		/// <summary>
		/// Gets an object from the cache given its unique key.
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
			return _cacheService.Get<T>(key);
		}

		/// <summary>
		/// Gets an object from the cache given its unique key.
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
		/// <param name="whenNull">
		/// Function to execute when the key is not present in the cache.
		/// </param>
		/// 
		/// <param name="expiration">
		/// Expiration DateTime.
		/// </param>
		/// 
		/// <returns>
		/// The retrieved object. If not found returns null.
		/// </returns>
		public T Get<T>(string key, Func<T> whenNull, DateTime expiration) where T : class
		{
			return _cacheService.Get<T>(key, whenNull, expiration);
		}

		/// <summary>
		/// Gets an object from the cache given its unique key.
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
		/// <param name="whenNull">
		/// Function to execute when the key is not present in the cache.
		/// </param>
		/// 
		/// <param name="expiration">
		/// Expiration TimeSpan.
		/// </param>
		/// 
		/// <returns>
		/// The retrieved object. If not found returns null.
		/// </returns>
		public T Get<T>(string key, Func<T> whenNull, TimeSpan expiration) where T : class
		{
			return _cacheService.Get<T>(key, whenNull, expiration);
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
			return _cacheService.Remove(key);
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
		/// The number of objects removed.
		/// </returns>
		public int RemoveByTag(string tag)
		{
			return _cacheService.RemoveByTag(tag);
		}

		#endregion

		#region [ Asynchronous Methods ]

		public async Task<bool> AddAsync(CacheItem cacheItem)
		{
			return await _cacheService.AddAsync(cacheItem);
		}

		public async Task<bool> AddAsync(string key, object data, TimeSpan timeSpan)
		{
			return await _cacheService.AddAsync(key, data, timeSpan);
		}

		public async Task<T> GetAsync<T>(string key) where T : class
		{
			return await _cacheService.GetAsync<T>(key);
		}

		public async Task<T> GetAsync<T>(string key, Func<T> whenNull, DateTime expiration) where T : class
		{
			return await _cacheService.GetAsync<T>(key, whenNull, expiration);
		}

		public async Task<T> GetAsync<T>(string key, Func<T> whenNull, TimeSpan expiration) where T : class
		{
			return await _cacheService.GetAsync<T>(key, whenNull, expiration);
		}

		public async Task<bool> RemoveAsync(string key)
		{
			return await _cacheService.RemoveAsync(key);
		}

		public async Task<int> RemoveByTagAsync(string key)
		{
			return await _cacheService.RemoveByTagAsync(key);
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the cache statistics.
		/// </summary>
		public ICacheStatistics Statistics
		{
			get { return _cacheService.Statistics; }
		}

		#endregion

		#region [ IDisposable Implementation ]

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_cacheService.SafeDispose();
			}
		}

		#endregion

		#endregion
	}
}
