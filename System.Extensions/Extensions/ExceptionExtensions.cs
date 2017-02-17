// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;

	public static class ExceptionExtensions
	{
		public static IEnumerable<ExceptionInfo> GetMessages(this Exception exception)
		{
			IList<ExceptionInfo> messages = new List<ExceptionInfo>();

			if (exception != null)
			{
				if (exception is AggregateException)
				{
					var aggregateException = (AggregateException)exception;

					messages.Add(new ExceptionInfo
					{
						Exception = aggregateException,
						Message = string.Format("{0} ({1} error(s))", aggregateException.Message, aggregateException.InnerExceptions.Count)
					});

					foreach (var innerException in aggregateException.InnerExceptions)
					{
						messages.Add(ExceptionInfo.Create(innerException));
					}
				}
				else
				{
					messages.Add(ExceptionInfo.Create(exception));

					while (exception.InnerException != null)
					{
						messages.Add(ExceptionInfo.Create(exception.InnerException));

						exception = exception.InnerException;
					}
				}
			}

			return messages;
		}

		public static string GetFullMessage(this Exception exception, bool withExceptionType = true, string separator = " -> ")
		{
			string message = null;

			if (exception != null)
			{
				StringBuilder sbMessage = new StringBuilder(2048);

				foreach (var item in exception.GetMessages())
				{
					if (withExceptionType)
					{
						sbMessage.AppendFormat("{0} -> {1}", item.Type.Name, item.Message);
					}
					else
					{
						sbMessage.AppendFormat(item.Message);
					}

					sbMessage.Append(separator);
				}

				message = sbMessage.ToString();
				message = message.TrimEnd(separator);
			}

			return message;
		}
	}

	public class ExceptionInfo
	{
		#region [ Properties ]

		public string Message { get; set; }

		public Type Type
		{
			get { return this.Exception.GetType(); }
		}

		public Exception Exception { get; set; }

		#endregion

		#region [ Methods ]

		public static ExceptionInfo Create(Exception exception)
		{
			if (exception == null)
			{
				ThrowException.ThrowArgumentNullException("exception");
			}

			var info = new ExceptionInfo { Exception = exception };

			if (exception is FileNotFoundException)
			{
				info.Message = string.Format("{0} (file: {1})", exception.Message, ((FileNotFoundException)exception).FileName);
			}
			else
			{
				info.Message = exception.Message.Replace(Environment.NewLine, " ");
			}

			return info;
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1}", this.Type.Name, this.Message);
		}

		#endregion
	}
}
