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
	using System.Threading;

	public static class ActionExtensions
	{
		/// <summary>
		/// Executes an action with many retries.
		/// </summary>
		/// 
		/// <param name="action">
		/// Action to execute.
		/// </param>
		/// 
		/// <param name="options">
		/// Execution options.
		/// </param>
		/// 
		/// <returns>
		/// True if the action has been executed; otherwise, false.
		/// </returns>
		/// 
		/// <exception cref="AggregateException">
		/// AggregateException if the action is not complete.
		/// </exception>
		public static bool ExecuteWithRetry(this Action action, ExecuteWithRetryOptions options = null)
		{
			if (options == null)
				options = new ExecuteWithRetryOptions();

			int counter = 0;
			IList<Exception> exceptions = new List<Exception>();

			do
			{
				try
				{
					action();
					options.IsComplete = true;
				}
				catch (Exception x)
				{
					DebugTraceException(x, counter);
					exceptions.Add(x);
				}
				finally
				{
					counter++;
				}

				if (!options.IsComplete && counter < options.RetryCounter)
				{
					Thread.Sleep(options.RetryDelay);
				}
			}
			while (!options.IsComplete && counter < options.RetryCounter);

			if (!exceptions.IsNullOrEmpty())
			{
				ThrowException.ThrowAggregateException(exceptions);
			}

			return options.IsComplete;
		}

		/// <summary>
		/// Executes an action with many retries.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the argument.
		/// </typeparam>
		/// 
		/// <param name="action">
		/// Action to execute.
		/// </param>
		/// 
		/// <param name="arg">
		/// Action argument.
		/// </param>
		/// 
		/// <param name="options">
		/// Execution options.
		/// </param>
		/// 
		/// <returns>
		/// True if the action has been executed; otherwise, false.
		/// </returns>
		/// 
		/// <exception cref="AggregateException">
		/// AggregateException if the action is not complete.
		/// </exception>
		public static bool ExecuteWithRetry<T>(this Action<T> action, T arg, ExecuteWithRetryOptions options = null)
		{
			if (options == null)
				options = new ExecuteWithRetryOptions();

			int counter = 0;
			IList<Exception> exceptions = new List<Exception>();

			do
			{
				try
				{
					action(arg);
					options.IsComplete = true;
				}
				catch (Exception x)
				{
					DebugTraceException(x, counter);
					exceptions.Add(x);
				}
				finally
				{
					counter++;
				}

				if (!options.IsComplete && counter < options.RetryCounter)
				{
					Thread.Sleep(options.RetryDelay);
				}
			}
			while (!options.IsComplete && counter < options.RetryCounter);

			if (!exceptions.IsNullOrEmpty())
			{
				ThrowException.ThrowAggregateException(exceptions);
			}

			return options.IsComplete;
		}

		/// <summary>
		/// Executes an action with many retries.
		/// </summary>
		/// 
		/// <typeparam name="T1">
		/// Type of the first argument.
		/// </typeparam>
		/// 
		/// <typeparam name="T2">
		/// Type of the second argument.
		/// </typeparam>
		/// 
		/// <param name="action">
		/// Action to execute.
		/// </param>
		/// 
		/// <param name="arg1">
		/// First action argument.
		/// </param>
		/// 
		/// <param name="arg2">
		/// Second action argument.
		/// </param>
		/// 
		/// <param name="options">
		/// Execution options.
		/// </param>
		/// 
		/// <returns>
		/// True if the action has been executed; otherwise, false.
		/// </returns>
		/// 
		/// <exception cref="AggregateException">
		/// AggregateException if the action is not complete.
		/// </exception>
		public static bool ExecuteWithRetry<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2, ExecuteWithRetryOptions options = null)
		{
			if (options == null)
				options = new ExecuteWithRetryOptions();

			int counter = 0;
			IList<Exception> exceptions = new List<Exception>();

			do
			{
				try
				{
					action(arg1, arg2);
					options.IsComplete = true;
				}
				catch (Exception x)
				{
					DebugTraceException(x, counter);
					exceptions.Add(x);
				}
				finally
				{
					counter++;
				}

				if (!options.IsComplete && counter < options.RetryCounter)
				{
					Thread.Sleep(options.RetryDelay);
				}
			}
			while (!options.IsComplete && counter < options.RetryCounter);

			if (!exceptions.IsNullOrEmpty())
			{
				ThrowException.ThrowAggregateException(exceptions);
			}

			return options.IsComplete;
		}

		/// <summary>
		/// Executes an action with many retries.
		/// </summary>
		/// 
		/// <typeparam name="T1">
		/// Type of the first argument.
		/// </typeparam>
		/// 
		/// <typeparam name="T2">
		/// Type of the second argument.
		/// </typeparam>
		/// 
		/// <typeparam name="T3">
		/// Type of the third argument.
		/// </typeparam>
		/// 
		/// <param name="action">
		/// Action to execute.
		/// </param>
		/// 
		/// <param name="arg1">
		/// First action argument.
		/// </param>
		/// 
		/// <param name="arg2">
		/// Second action argument.
		/// </param>
		/// 
		/// <param name="arg3">
		/// Third action argument.
		/// </param>
		/// 
		/// <param name="options">
		/// Execution options.
		/// </param>
		/// 
		/// <returns>
		/// True if the action has been executed; otherwise, false.
		/// </returns>
		public static bool ExecuteWithRetry<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3, ExecuteWithRetryOptions options = null)
		{
			if (options == null)
				options = new ExecuteWithRetryOptions();

			int counter = 0;
			IList<Exception> exceptions = new List<Exception>();

			do
			{
				try
				{
					action(arg1, arg2, arg3);
					options.IsComplete = true;
				}
				catch (Exception x)
				{
					DebugTraceException(x, counter);
					exceptions.Add(x);
				}
				finally
				{
					counter++;
				}

				if (!options.IsComplete && counter < options.RetryCounter)
				{
					Thread.Sleep(options.RetryDelay);
				}
			}
			while (!options.IsComplete && counter < options.RetryCounter);

			if (!exceptions.IsNullOrEmpty())
			{
				ThrowException.ThrowAggregateException(exceptions);
			}

			return options.IsComplete;
		}

		/// <summary>
		/// Executes an action with many retries.
		/// </summary>
		/// 
		/// <typeparam name="T1">
		/// Type of the first argument.
		/// </typeparam>
		/// 
		/// <typeparam name="T2">
		/// Type of the second argument.
		/// </typeparam>
		/// 
		/// <typeparam name="T3">
		/// Type of the third argument.
		/// </typeparam>
		/// 
		/// <typeparam name="T4">
		/// Type of the fourth argument.
		/// </typeparam>
		/// 
		/// <param name="action">
		/// Action to execute.
		/// </param>
		/// 
		/// <param name="arg1">
		/// First action argument.
		/// </param>
		/// 
		/// <param name="arg2">
		/// Second action argument.
		/// </param>
		/// 
		/// <param name="arg3">
		/// Third action argument.
		/// </param>
		/// 
		/// <param name="arg4">
		/// Fourth action argument.
		/// </param>
		/// 
		/// <param name="options">
		/// Execution options.
		/// </param>
		/// 
		/// <returns>
		/// True if the action has been executed; otherwise, false.
		/// </returns>
		/// 
		/// <exception cref="AggregateException">
		/// AggregateException if the action is not complete.
		/// </exception>
		public static bool ExecuteWithRetry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, ExecuteWithRetryOptions options = null)
		{
			if (options == null)
				options = new ExecuteWithRetryOptions();

			int counter = 0;
			IList<Exception> exceptions = new List<Exception>();

			do
			{
				try
				{
					action(arg1, arg2, arg3, arg4);
					options.IsComplete = true;
				}
				catch (Exception x)
				{
					DebugTraceException(x, counter);
					exceptions.Add(x);
				}
				finally
				{
					counter++;
				}

				if (!options.IsComplete && counter < options.RetryCounter)
				{
					Thread.Sleep(options.RetryDelay);
				}
			}
			while (!options.IsComplete && counter < options.RetryCounter);

			if (!exceptions.IsNullOrEmpty())
			{
				ThrowException.ThrowAggregateException(exceptions);
			}

			return options.IsComplete;
		}

		private static void DebugTraceException(Exception exception, int counter)
		{
#if DEBUG
			if (exception != null)
			{
				Debug.WriteLine("ExecuteWithRetry, Exception #{0}: {1}", counter + 1, exception.Message);
			}
#endif //DEBUG
		}
	}

	public class ExecuteWithRetryOptions
	{
		#region [ Constructor ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExecuteWithRetryOptions()
		{
			this.RetryCounter = 3;
			this.RetryDelay = 250;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the value indicating how many times the action is executed when fails. Default value is 3.
		/// </summary>
		public int RetryCounter
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating the delay (ms) between each retry. Default value is 250ms.
		/// </summary>
		public int RetryDelay
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the value indicating whether the action is complete.
		/// </summary>
		public bool IsComplete
		{
			get;
			internal set;
		}

		#endregion
	}
}
