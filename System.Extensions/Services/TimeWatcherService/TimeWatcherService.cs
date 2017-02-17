// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Diagnostics;

	public class TimeWatcherService : ITimeWatcherService, IDisposable
	{
		#region [ Members ]

		private readonly Stopwatch _watcher = null;

		#endregion

		#region [ Constructors ]

		public TimeWatcherService()
		{
			_watcher = new Stopwatch();
			_watcher.Start();
		}

		~TimeWatcherService()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ ITimeWatcher Implementation ]

		public TimeSpan Time
		{
			get
			{
				return TimeSpan.FromMilliseconds(_watcher.ElapsedMilliseconds);
			}
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
		}

		#endregion
	}
}
