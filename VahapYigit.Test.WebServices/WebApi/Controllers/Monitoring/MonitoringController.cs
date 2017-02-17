// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;

	using VahapYigit.Test.Core;
	using VahapYigit.Test.Models;
	using VahapYigit.Test.Services;

	// Note :
	// ------
	// These WebApi operations should be invoked by an Admin Account.
	// See the appSettings section in the Web.config file and the AdminAuthenticationFilterAttribute attribute.

	[AdminAuthenticationFilter]
	[DebugExecutionTracer]
	public class MonitoringController : ApiControllerBase
	{
		// /api/Monitoring/GetLogContent
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetLogContent()
		{
			string data = FileHelper.TryReadAllText(LoggerServiceHelper.Current.LogSource);

			if (string.IsNullOrEmpty(data))
			{
				data = "Empty.";
			}

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		// /api/Monitoring/GetCacheStatistics
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetCacheStatistics()
		{
			var data = CacheServiceHelper.Current.Statistics;

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		// /api/Monitoring/GetCacheContent
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetCacheContent()
		{
			var data = new List<JsonCacheItem>();

			if (CacheServiceHelper.Current.Statistics != null &&
				CacheServiceHelper.Current.Statistics.Info != null)
			{
				CacheServiceHelper.Current.Statistics.Info.OrderBy(i => i.Key).ForEach(item =>
				{
					data.Add(new JsonCacheItem(item.Value));
				});
			}

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		// /api/Monitoring/GetExecutionTraces
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetExecutionTraces()
		{
			var options = new SearchOptions();

			options.Orders.Add(ExecutionTrace.ColumnNames.Module);
			options.Orders.Add(ExecutionTrace.ColumnNames.ClassName);
			options.Orders.Add(ExecutionTrace.ColumnNames.MethodName);
			options.Orders.Add(ExecutionTrace.ColumnNames.Tag);

			var data = new List<JsonExecutionTrace>();

			foreach (var item in ServiceContext.ExecutionTraceService
											   .Search(base.UserContext, ref options)
											   .OrderByDescending(i => i.AvgDuration)
											   .ThenByDescending(i => i.LastCall))
			{
				data.Add(new JsonExecutionTrace(item));
			}

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		// /api/Monitoring/GetTableInformation
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetTableInformation()
		{
			var data = ServiceContext.CommonService.GetTableSizes(base.UserContext).OrderByDescending(i => i.TotalSizeInMB);

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		// /api/Monitoring/GetProcessLogs
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetProcessLogs()
		{
			var options = new SearchOptions();

			options.Orders.Add(ProcessLog.ColumnNames.Date, OrderOperator.Desc);

			var data = new List<JsonProcessLog>();

			ServiceContext.ProcessLogService.Search(base.UserContext, ref options).ForEach(item =>
			{
				data.Add(new JsonProcessLog(item));
			});

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		// /api/Monitoring/GetProcessErrorLogs
		[HttpGet]
		[AcceptVerbs("GET")]
		public HttpResponseMessage GetProcessErrorLogs()
		{
			var options = new SearchOptions();

			options.Orders.Add(ProcessErrorLog.ColumnNames.Date, OrderOperator.Desc);

			var data = new List<JsonProcessErrorLog>();

			ServiceContext.ProcessErrorLogService.Search(base.UserContext, ref options).ForEach(item =>
			{
				data.Add(new JsonProcessErrorLog(item));
			});

			return this.Request.CreateResponse(HttpStatusCode.OK, data);
		}

		#region [ Json Models ]

		class JsonCacheItem
		{
			#region [ JsonCacheItem ]

			public string Key { get; set; }
			public string Description { get; set; }
			public string IsExpired { get; set; }
			public string Expiration { get; set; }

			public JsonCacheItem(CacheItemDisplay item)
			{
				this.Key = item.Key;
				this.Description = Format(item.Description);
				this.IsExpired = (item.IsExpired) ? "Yes" : "";

				var expirationTimeSpan = (item.Expiration - DateTime.Now);
				this.Expiration = expirationTimeSpan.TotalDays < 60 ? expirationTimeSpan.ToReadable() : "never";
			}

			private static string Format(string value) { return value != null ? value.Replace("\"", "'") : null; }

			#endregion
		}

		class JsonExecutionTrace
		{
			#region [ JsonExecutionTrace ]

			public string Name { get; set; }
			public int MinDuration { get; set; }
			public int MaxDuration { get; set; }
			public long AvgDuration { get; set; }
			public string TotalDuration { get; set; }
			public long Counter { get; set; }
			public string LastCall { get; set; }

			public JsonExecutionTrace(ExecutionTrace item)
			{
				this.Name = string.Format("{0}.{1}({2})", item.ClassName, item.MethodName, item.Tag);
				this.MinDuration = item.MinDuration;
				this.MaxDuration = item.MaxDuration;
				this.AvgDuration = item.AvgDuration;
				this.TotalDuration = Format(Math.Round((double)item.TotalDuration / 1000 /*secs*/ / 60 /*mins*/ / 60 /*hrs*/, 2));
				this.Counter = item.Counter;
				this.LastCall = item.LastCall.ToString("yyy-MM-dd HH:mm");
			}

			private static string Format(double value) { return value.ToString().Replace(",", "."); }

			#endregion
		}

		class JsonProcessLog
		{
			#region [ JsonProcessLog ]

			public string Date { get; set; }
			public string Procedure { get; set; }
			public string Data { get; set; }

			public JsonProcessLog() { }

			public JsonProcessLog(ProcessLog item)
			{
				this.Date = item.Date.ToString("yyyy-MM-dd, HH:mm:ss");
				this.Procedure = item.ProcedureName;
				this.Data = item.Data;
			}

			#endregion
		}

		class JsonProcessErrorLog : JsonProcessLog
		{
			#region [ JsonProcessErrorLog ]

			public string Message { get; set; }
			public int? Severity { get; set; }
			public int? State { get; set; }

			public JsonProcessErrorLog(ProcessErrorLog item)
			{
				this.Date = item.Date.ToString("yyyy-MM-dd, HH:mm:ss");
				this.Procedure = item.ProcedureName;
				this.Message = item.ErrorMessage;
				this.Data = item.Data;
				this.Severity = item.ErrorSeverity;
				this.State = item.ErrorState;
			}

			#endregion
		}

		#endregion
	}
}
