// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections;
	using System.Diagnostics;

	/// <summary>
	/// CacheItem object.
	/// </summary>
	[DebuggerDisplay("{Key} (isExpired = {IsExpired})")]
	public class CacheItem : IDisposable
	{
		#region [ Constructors ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CacheItem()
		{
			this.Expiration = DateTime.MaxValue;
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~CacheItem()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the unique Key.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the tag value. Tag is used to remove from cache severals items having the same tag value.
		/// </summary>
		public string Tag { get; set; }

		/// <summary>
		/// Gets or sets the Data (inner cached object).
		/// </summary>
		public object Data { get; set; }

		/// <summary>
		/// Gets or sets the expiration DateTime.
		/// </summary>
		public DateTime Expiration { get; set; }

		/// <summary>
		/// Indicates whether the object is expired.
		/// </summary>
		public bool IsExpired
		{
			get { return this.Expiration < DateTime.Now; }
		}

		/// <summary>
		/// Gets the description of the data.
		/// </summary>
		public string Description
		{
			get
			{
				string readable = null;

				if (this.Data is ICollection)
				{
					int count = ((ICollection)this.Data).Count;
					readable = string.Format("Collection ({0} item{1})", count, (count > 1) ? "s" : string.Empty);
				}
				else if (this.Data != null)
				{
					readable = this.Data.ToString();
				}

				return readable;
			}
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
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				var disposer = this.Data as IDisposable;
				disposer.SafeDispose();

				this.Data = null;

				_isDisposed = true;
			}

			this.Expiration = DateTime.MinValue;
		}

		#endregion
	}
}
