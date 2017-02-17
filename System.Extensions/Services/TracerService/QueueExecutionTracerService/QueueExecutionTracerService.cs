// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;
	using System.Timers;

	public class QueueExecutionTracerService<T> : ITracerService where T : class, new()
	{
		#region [ Members ]

		protected readonly string _tag = null;
		protected readonly Stopwatch _watcher = null;

		protected readonly static ConcurrentQueue<T> _queue = null;
		protected readonly static QueueExecutionTracerServiceSection _config = null;

		private readonly static Timer _timer = null;

		protected static IQueueExecutionTracerBehavior<T> _behavior = null;

		#endregion

		#region [ Constructors ]

		static QueueExecutionTracerService()
		{
			_config = QueueExecutionTracerServiceSectionHandler.GetSection();
			_queue = new ConcurrentQueue<T>();

			if (_config == null)
			{
				_config = new QueueExecutionTracerServiceSection
				{
					IsEnabled = true,
					WithDebugTrace = true,
					MaxItems = 1024,
					RollTimer = 1
				};
			}

			_timer = new System.Timers.Timer(TimeSpan.FromSeconds(_config.RollTimer).TotalMilliseconds);

			_timer.Elapsed += OnTimerElapsed;
			_timer.Enabled = true;
		}

		public QueueExecutionTracerService(string tag = null)
		{
			_tag = tag;

			_watcher = new Stopwatch();
			_watcher.Start();
		}

		~QueueExecutionTracerService()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ ITracerService Implementation ]

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			_watcher.Stop();

			MethodBase mb = new StackFrame(2).GetMethod();
			if (mb != null)
			{
				if (_behavior != null)
				{
					if (_queue.Count < _config.MaxItems)
					{
						T item = _behavior.CreateItem(mb.Module.Name, mb.ReflectedType.ToString(), mb.Name, _tag, _watcher.ElapsedMilliseconds);
						if (item != default(T))
						{
							_queue.Enqueue(item);
						}
					}
				}
			}
		}

		#endregion

		#region [ Events ]

		private static void OnTimerElapsed(object sender, EventArgs e)
		{
			_timer.Enabled = false;

			do
			{
				if (!_config.IsEnabled)
				{
					break;
				}

				if (_behavior != null && !_queue.IsEmpty)
				{
					T trace;
					IList<T> traces = new List<T>(_queue.Count);

					while (_queue.TryDequeue(out trace))
					{
						traces.Add(trace);
					}

					try
					{
						_behavior.TraceItems(traces);
					}
					catch (Exception x)
					{
						LoggerServiceHelper.Current.TraceException(typeof(QueueExecutionTracerService<T>), x);
					}
				}
			}
			while (false);

			_timer.Enabled = true;
		}

		#endregion
	}
}
