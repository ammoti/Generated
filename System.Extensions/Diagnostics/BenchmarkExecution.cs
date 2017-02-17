// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Class used to execute tasks and measure theirs time executions.
	/// </summary>
	public class BenchmarkExecution
	{
		#region [ Usage ]

		/*
        BenchmarkExecution benchmark = new BenchmarkExecution();

        benchmark.AddTask("Method_v1.0", () => Method_v1());
        benchmark.AddTask("Method_v2.0", () => Method_v2());

        benchmark.AddTask("Method_v2.1", () => PreMethod(),
                                         () => Method_v2(),
                                         () => PostMethod());

        benchmark.Execute();

        Console.WriteLine("-> Method_v1.0: {0}ms", benchmark.GetDuration("Method_v1.0"));
        Console.WriteLine("-> Method_v2.0: {0}ms", benchmark.GetDuration("Method_v2.0"));
        Console.WriteLine("-> Method_v2.1: {0}ms", benchmark.GetDuration("Method_v2.1"));

        Console.WriteLine(benchmarkToString());
       
        ...

        private static void Method_v1() { Thread.Sleep(200); }
        private static void Method_v2() { Thread.Sleep(100); }

        private static void PreMethod() { Thread.Sleep(50); }
        private static void PostMethod() { Thread.Sleep(50); }
        */

		#endregion

		#region [ Members ]

		private readonly IDictionary<string, BenchmarkExecutionAction> _actions = null;

		private readonly IDictionary<string, long> _durations = null;

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Constructor.
		/// </summary>
		public BenchmarkExecution()
		{
			_actions = new Dictionary<string, BenchmarkExecutionAction>();
			_durations = new Dictionary<string, long>();
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Adds a new task to the BenchmarkExecution.
		/// </summary>
		///
		/// <param name="idTask">
		/// Identifier of the task.
		/// </param>
		///
		/// <param name="action">
		/// Action to execute and measure.
		/// </param>
		public void AddTask(string idTask, Action action)
		{
			_actions.Add(idTask, new BenchmarkExecutionAction { Action = action });
		}

		/// <summary>
		/// Adds a new task to the BenchmarkExecution.
		/// </summary>
		///
		/// <param name="idTask">
		/// Identifier of the task.
		/// </param>
		///
		/// <param name="preAction">
		/// The action (Initialize) to execute before the action to measure.
		/// </param>
		///
		/// <param name="action">
		/// Action to execute and measure.
		/// </param>
		///
		/// <param name="postAction">
		/// The action (Clean) to execute after the action to measure (optional).
		/// </param>
		public void AddTask(string idTask, Action preAction, Action action, Action postAction = null)
		{
			_actions.Add(idTask, new BenchmarkExecutionAction
			{
				PreAction = preAction,
				Action = action,
				PostAction = postAction
			});
		}

		/// <summary>
		/// Executes the tasks.
		/// </summary>
		///
		/// <param name="repeatCounter">
		/// Value indication how many times is executed each action. Default value is 1.
		/// </param>
		public void Execute(int repeatCounter = 1)
		{
			IDictionary<string, long> data = new Dictionary<string, long>();
			Stopwatch watcher = new Stopwatch();

			_durations.Clear();

			for (int i = 0; i < repeatCounter; i++)
			{
				foreach (string key in _actions.Keys)
				{
					if (_actions[key].PreAction != null)
						_actions[key].PreAction();

					watcher.Start();
					_actions[key].Action();
					watcher.Stop();

					if (_actions[key].PostAction != null)
						_actions[key].PostAction();

					if (data.ContainsKey(key))
					{
						data[key] += watcher.ElapsedMilliseconds;
					}
					else
					{
						data.Add(key, watcher.ElapsedMilliseconds);
					}

					watcher.Reset();
				}
			}

			foreach (string key in data.Keys)
			{
				_durations[key] = data[key] / repeatCounter;
			}
		}

		/// <summary>
		/// Gets the task duration in millisecond.
		/// </summary>
		///
		/// <param name="idTask">
		/// Identifier of the task.
		/// </param>
		///
		/// <returns>
		/// The task duration in millisecond.
		/// </returns>
		public long GetDuration(string idTask)
		{
			return _durations[idTask];
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		///
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			int p = 0;

			StringBuilder sbResult = new StringBuilder();
			KeyValuePair<string, long> best = _durations.OrderBy(i => i.Value).FirstOrDefault();

			foreach (var item in _durations.OrderBy(i => i.Value))
			{
				if (p > 0 && best.Value > 0)
				{
					decimal delta = Math.Round((decimal)item.Value / best.Value * 100 - 100, 0);
					sbResult.AppendFormat("{0} ({1}ms/+{2}%)", item.Key, item.Value, delta);
				}
				else
				{
					sbResult.AppendFormat("{0} ({1}ms)", item.Key, item.Value);
				}

				if (p < _durations.Count - 1)
				{
					sbResult.Append(" << ");
				}

				p++;
			}

			return sbResult.ToString();
		}

		#endregion
	}

	internal class BenchmarkExecutionAction
	{
		/// <summary>
		/// Gets or sets the action executed before the action to measure.
		/// </summary>
		public Action PreAction { get; set; }

		/// <summary>
		/// Gets or sets the action to execute and measure.
		/// </summary>
		public Action Action { get; set; }

		/// <summary>
		/// Gets or sets the action executed after the action to measure.
		/// </summary>
		public Action PostAction { get; set; }
	}
}
