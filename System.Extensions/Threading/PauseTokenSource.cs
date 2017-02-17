// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Threading;

	/// <summary>
	/// Signals to a Token that it should be paused or resumed.
	/// </summary>
	public class PauseTokenSource : IDisposable
	{
		#region [ Members ]

		protected ManualResetEvent _mrEvent = null;

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Initializes the PauseTokenSource.
		/// </summary>
		public PauseTokenSource()
		{
			_mrEvent = new ManualResetEvent(true);
		}

		/// <summary>
		/// Destructs the PauseTokenSource.
		/// </summary>
		~PauseTokenSource()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the Token associated with this PauseTokenSource.
		/// </summary>
		public IPauseToken Token
		{
			get { return new PauseToken(this); }
		}

		/// <summary>
		/// Gets whether pause has been requested for this PauseTokenSource.
		/// </summary>
		public bool IsPauseRequested
		{
			get { return !_mrEvent.WaitOne(0); }
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Communicates a request for pause.
		/// </summary>
		public void Pause()
		{
			_mrEvent.Reset();
		}

		/// <summary>
		/// Communicates a request for resume.
		/// </summary>
		public void Resume()
		{
			_mrEvent.Set();
		}

		/// <summary>
		/// Blocks the current thread (if pause has been requested) until the current System.Threading.WaitHandle receives a resume signal.
		/// </summary>
		public void WaitOnPause()
		{
			_mrEvent.WaitOne();
		}

		#endregion

		#region [ IDisposable Implementation ]

		private bool _isDisposed = false;

		/// <summary>
		/// Releases all resources used by the current instance of the PauseTokenSource class
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the PauseTokenSource class and optionally releases the managed resources.
		/// </summary>
		/// 
		/// <param name="disposing">
		/// true to release both managed and unmanaged resources; false to release only unmanaged resources.
		/// </param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_mrEvent", Justification = "SafeDispose()")]
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_mrEvent.SafeDispose();
				}

				_isDisposed = true;
			}
		}

		#endregion
	}
}
