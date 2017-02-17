// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	// Based on http://stackoverflow.com/questions/2510975/c-sharp-object-pooling-pattern-implementation

	public class Pool<T> : IDisposable
	{
		#region [ Members ]

		private Func<Pool<T>, T> _factory = null;
		private Func<object> _initializeFunc = null;
		private PoolLoadingModeEnum _poolLoadingMode = PoolLoadingModeEnum.Eager;
		private IItemStore _itemStore = null;
		private int _poolSize = 0;
		private int _count = 0;
		private Semaphore _sync = null;

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Creates a pool with Lazy loading mode and FIFO access mode.
		/// </summary>
		/// 
		/// <param name="poolSize">
		/// Size of the pool.
		/// </param>
		/// 
		/// <param name="factory">
		/// Func used to create PooledObjects.
		/// </param>
		/// 
		/// <param name="initializeFunc">
		/// Func used to initialize pool's intances (if the class implements IPoolableObject).
		/// </param>
		public Pool(int poolSize, Func<Pool<T>, T> factory, Func<object> initializeFunc = null)
			: this(poolSize, factory, PoolLoadingModeEnum.Lazy, PoolAccessModeEnum.FIFO, initializeFunc)
		{
		}

		/// <summary>
		/// Creates a pool.
		/// </summary>
		/// 
		/// <param name="poolSize">
		/// Size of the pool.
		/// </param>
		/// 
		/// <param name="factory">
		/// Func used to create PooledObjects.
		/// </param>
		/// 
		/// <param name="poolLoadingMode">
		/// Loading mode of the pool.
		/// </param>
		/// 
		/// <param name="poolAccessMode">
		/// Access mode of the pool.
		/// </param>
		/// 
		/// <param name="initializeFunc">
		/// Func used to initialize pool's intances (if the class implements IPoolableObject).
		/// </param>
		public Pool(int poolSize, Func<Pool<T>, T> factory, PoolLoadingModeEnum poolLoadingMode, PoolAccessModeEnum poolAccessMode, Func<object> initializeFunc = null)
		{
			if (poolSize <= 0)
			{
				throw new ArgumentOutOfRangeException("poolSize", poolSize, "The 'poolSize' value must be greater than zero.");
			}

			if (factory == null)
			{
				throw new ArgumentNullException("_factory");
			}

			_poolSize = poolSize;
			_poolLoadingMode = poolLoadingMode;

			_factory = factory;
			_initializeFunc = initializeFunc;

			_sync = new Semaphore(poolSize, poolSize);
			_itemStore = CreateItemStore(poolAccessMode, poolSize);

			if (poolLoadingMode == PoolLoadingModeEnum.Eager)
			{
				PreloadItems();
			}
		}

		~Pool()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ Methods ]

		public void Release(T item)
		{
			var poolableObject = item as IPoolableObject;
			if (poolableObject != null)
			{
				poolableObject.ResetState();
			}

			lock (_itemStore)
			{
				_itemStore.Store(item);
			}

			_sync.Release();
		}

		/// <summary>
		/// Returns a pooled instance. When the task is done this instance must be disposed in order to become available.
		/// You can use using() block: using (var connector = pool.Acquire()) { ... }.
		/// Or invoke the Release() method: pool.Release(connector).
		/// The first approach is recommended.
		/// </summary>
		/// 
		/// <returns>
		/// 
		/// </returns>
		public T Acquire()
		{
			_sync.WaitOne();

			if (_poolLoadingMode == PoolLoadingModeEnum.Eager)
			{
				return AcquireEager();
			}
			else if (_poolLoadingMode == PoolLoadingModeEnum.Lazy)
			{
				return AcquireLazy();
			}
			else if (_poolLoadingMode == PoolLoadingModeEnum.LazyExpanding)
			{
				return AcquireLazyExpanding();
			}

			throw new NotSupportedException(string.Format(
				"The '{0}' PoolLoadingModeEnum encountered in Acquire method is not supported",
				_poolLoadingMode));
		}

		private T AcquireEager()
		{
			lock (_itemStore)
			{
				return _itemStore.Fetch();
			}
		}

		private T AcquireLazy()
		{
			if (_itemStore.Count > 0)
			{
				lock (_itemStore)
				{
					if (_itemStore.Count > 0)
					{
						return _itemStore.Fetch();
					}
				}
			}

			Interlocked.Increment(ref _count);

			return this.CreateItem();
		}

		private T AcquireLazyExpanding()
		{
			bool shouldExpand = false;

			if (_count < _poolSize)
			{
				int newCount = Interlocked.Increment(ref _count);
				if (newCount <= _poolSize)
				{
					shouldExpand = true;
				}
				else // Another thread took the last spot - use the store instead
				{
					Interlocked.Decrement(ref _count);
				}
			}

			if (shouldExpand)
			{
				return this.CreateItem();
			}
			else
			{
				lock (_itemStore)
				{
					return _itemStore.Fetch();
				}
			}
		}

		private void PreloadItems(bool withParallelism = false)
		{
			for (int i = 0; i < _poolSize; i++)
			{
				T item = this.CreateItem();
				_itemStore.Store(item);
			}

			_count = _poolSize;
		}

		private T CreateItem()
		{
			T item = _factory(this);

			var poolableObject = item as IPoolableObject;
			if (poolableObject != null)
			{
				poolableObject.Initialize(_initializeFunc());
			}

			return item;
		}

		#endregion

		#region [ Collection Wrappers ]

		interface IItemStore
		{
			T Fetch();
			void Store(T item);
			int Count { get; }
		}

		private IItemStore CreateItemStore(PoolAccessModeEnum mode, int capacity)
		{
			if (mode == PoolAccessModeEnum.FIFO)
			{
				return new QueueStore(capacity);
			}
			else if (mode == PoolAccessModeEnum.LIFO)
			{
				return new StackStore(capacity);
			}
			else if (mode == PoolAccessModeEnum.Circular)
			{
				return new CircularStore(capacity);
			}

			throw new NotSupportedException(string.Format(
				"The '{0}' PoolAccessModeEnum encountered in CreateItemStore method is not supported",
				mode));
		}

		class QueueStore : Queue<T>, IItemStore
		{
			public QueueStore(int capacity)
				: base(capacity)
			{
			}

			public T Fetch()
			{
				return Dequeue();
			}

			public void Store(T item)
			{
				Enqueue(item);
			}
		}

		class StackStore : Stack<T>, IItemStore
		{
			public StackStore(int capacity)
				: base(capacity)
			{
			}

			public T Fetch()
			{
				return Pop();
			}

			public void Store(T item)
			{
				Push(item);
			}
		}

		class CircularStore : IItemStore
		{
			private List<Slot> slots;
			private int freeSlotCount;
			private int position = -1;

			public CircularStore(int capacity)
			{
				slots = new List<Slot>(capacity);
			}

			public T Fetch()
			{
				if (Count == 0)
					throw new InvalidOperationException("The buffer is empty.");

				int startPosition = position;
				do
				{
					Advance();

					Slot slot = slots[position];
					if (!slot.IsInUse)
					{
						slot.IsInUse = true;
						--freeSlotCount;

						return slot.Item;
					}
				}
				while (startPosition != position);

				throw new InvalidOperationException("No free slots.");
			}

			public void Store(T item)
			{
				Slot slot = slots.Find(s => object.Equals(s.Item, item));
				if (slot == null)
				{
					slot = new Slot(item);
					slots.Add(slot);
				}

				slot.IsInUse = false;
				++freeSlotCount;
			}

			public int Count
			{
				get { return freeSlotCount; }
			}

			private void Advance()
			{
				position = (position + 1) % slots.Count;
			}

			class Slot
			{
				public Slot(T item)
				{
					this.Item = item;
				}

				public T Item { get; private set; }
				public bool IsInUse { get; set; }
			}
		}

		#endregion

		#region [ IDisposable Implementation ]

		private bool _isDisposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!_isDisposed)
				{
					_isDisposed = true;

					if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
					{
						lock (_itemStore)
						{
							while (_itemStore.Count > 0)
							{
								var disposer = (IDisposable)_itemStore.Fetch();
								disposer.Dispose();
							}
						}
					}

					_sync.Close();
				}
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		#endregion
	}
}
