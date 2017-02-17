// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public abstract class PooledObject<I, T> : IPooledObject, IPoolableObject, IDisposable
		where I : class, IDisposable
		where T : class, I, new()
	{
		#region [ Members ]

		private Pool<I> _pool = null;
		protected T _innerObject = null;

		#endregion

		#region [ Constructor ]

		public PooledObject(Pool<I> pool)
		{
			if (pool == null)
			{
				throw new ArgumentNullException("pool");
			}

			_pool = pool;
			_innerObject = new T();
		}

		#endregion

		#region [ IPoolableObject Implementation ]

		public virtual void Initialize(object parameter)
		{
			// If _innerObject implements IPoolableObject interface you don't have to override this method.

			if (_innerObject is IPoolableObject)
			{
				((IPoolableObject)_innerObject).Initialize(parameter);
			}
		}

		public virtual void ResetState()
		{
			// If _innerObject implements IPoolableObject interface you don't have to override this method.

			if (_innerObject is IPoolableObject)
			{
				((IPoolableObject)_innerObject).ResetState();
			}
		}

		#endregion

		#region [ IDisposable Implementation ]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
		public void Dispose()
		{
			if (_pool.IsDisposed)
			{
				_innerObject.SafeDispose();
			}
			else
			{
				_pool.Release(this as I);
			}
		}

		#endregion
	}
}
