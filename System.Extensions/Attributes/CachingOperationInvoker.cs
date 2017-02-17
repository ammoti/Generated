// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Runtime.Remoting.Messaging;
	using System.Security.Cryptography;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;
	using System.Text;
	using System.Threading;

	sealed public class CachingOperationInvoker : IOperationInvoker
	{
		#region [ Members ]

		private readonly IOperationInvoker _invoker = null;
		private readonly OperationDescription _operation = null;

		private readonly long _cacheDuration = 0;
		private readonly Type _keyGeneratorType = null;
		private readonly bool _withUserContextDependency = false;

		#endregion

		#region [ Constructors ]

		static CachingOperationInvoker()
		{
		}

		public CachingOperationInvoker(IOperationInvoker invoker, OperationDescription operation, long cacheDuration, Type keyGeneratorType, bool withUserContextDependency)
		{
			_invoker = invoker;
			_operation = operation;

			_cacheDuration = cacheDuration;
			_keyGeneratorType = keyGeneratorType;
			_withUserContextDependency = withUserContextDependency;
		}

		#endregion

		#region [ IOperationInvoker Implementation ]

		delegate object InvokerDelegate(object[] inputs, out object[] outputs);

		public object[] AllocateInputs()
		{
			return _invoker.AllocateInputs();
		}

		public object Invoke(object instance, object[] inputs, out object[] outputs)
		{
			string cacheKey = this.CreateCacheKey(inputs);

			CachedOperationResult cachedItem = CacheServiceHelper.Current.Get<CachedOperationResult>(cacheKey);
			if (cachedItem != null)
			{
				LoggerServiceHelper.Current.WriteLine(this, LogStatusEnum.Debug, "{0}.{1} -> Gets method result from cache",
					_operation.DeclaringContract.ContractType,
					_operation.Name);

				outputs = cachedItem.Outputs;
				return cachedItem.Data;
			}

			object data = _invoker.Invoke(instance, inputs, out outputs);

			if (_cacheDuration > 0)
			{
				cachedItem = new CachedOperationResult { Data = data, Outputs = outputs };

				CacheServiceHelper.Current.Add(cacheKey, cachedItem, TimeSpan.FromSeconds(_cacheDuration));
			}

			return data;
		}

		public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
		{
			string cacheKey = this.CreateCacheKey(inputs);

			CachedOperationResult cachedItem = CacheServiceHelper.Current.Get<CachedOperationResult>(cacheKey);
			CachingUserState cachingUserState = new CachingUserState
			{
				CacheItem = cachedItem,
				CacheKey = cacheKey,
				Callback = callback,
				State = state
			};

			IAsyncResult originalAsyncResult;
			if (cachedItem != null)
			{
				InvokerDelegate invoker = cachedItem.GetValue;

				object[] notUsed;
				originalAsyncResult = invoker.BeginInvoke(inputs, out notUsed, this.InvokerCallback, cachingUserState);
			}
			else
			{
				originalAsyncResult = _invoker.InvokeBegin(instance, inputs, this.InvokerCallback, cachingUserState);
			}

			return new CachingAsyncResult(originalAsyncResult, cachingUserState);
		}

		public object InvokeEnd(object instance, out object[] outputs, IAsyncResult asyncResult)
		{
			CachingAsyncResult result = asyncResult as CachingAsyncResult;
			CachingUserState state = result.CachingUserState;

			if (state.CacheItem == null)
			{
				object data = _invoker.InvokeEnd(instance, out outputs, result.OriginalAsyncResult);
				state.CacheItem = new CachedOperationResult { Data = data, Outputs = outputs };

				CacheServiceHelper.Current.Add(state.CacheKey, state.CacheItem, TimeSpan.FromSeconds(_cacheDuration));

				return data;
			}
			else
			{
				InvokerDelegate invoker = ((AsyncResult)result.OriginalAsyncResult).AsyncDelegate as InvokerDelegate;
				invoker.EndInvoke(out outputs, result.OriginalAsyncResult);

				return state.CacheItem.Data;
			}
		}

		public bool IsSynchronous
		{
			get { return _invoker.IsSynchronous; }
		}

		private void InvokerCallback(IAsyncResult asyncResult)
		{
			CachingUserState state = asyncResult.AsyncState as CachingUserState;
			state.Callback(new CachingAsyncResult(asyncResult, state));
		}

		#endregion

		#region [ Private Methods ]

		private string CreateCacheKey(object[] inputs, bool withGuidFormat = true)
		{
			string key = string.Format("{0}.{1}",
				_operation.DeclaringContract.ContractType,
				_operation.Name);

			if (_keyGeneratorType != null)
			{
				key = KeyGeneratorHelper.GenerateKey<string>(_keyGeneratorType, inputs);
			}
			else
			{
				string inputsKey = string.Empty;

				// *** BEGIN SPECIFIC CODE ***
				// ***************************

				// Note: if you plan to use this class in another project, you will probably remove this code.

				if (_withUserContextDependency) // -> The operation result depends on the user context
				{
					// Design Constraint: input[0] is always IUserContext parameter (LayerCake Generator process checks this point).

					if (inputs.Length > 0)
					{
						inputsKey = SerializerHelper.ToXml(SerializerType.DataContract, inputs[0]);
					}

#if DEBUG
					if (!inputsKey.Contains("ClientContext") &&
						!inputsKey.Contains("MemberContext"))
					{
						ThrowException.Throw(
							"Something is going wrong: Business & Service methods must define at first the IUserContext parameter.");
					}
#endif
				}

				// *** END SPECIFIC CODE ***
				// ***************************

				StringBuilder sbXml = new StringBuilder(inputsKey);

				for (int i = 1; i < inputs.Length; i++)
				{
					sbXml.Append(SerializerHelper.ToXml(SerializerType.DataContract, inputs[i]));
				}

				inputsKey = sbXml.ToString();

				key = string.Format("{0}({1})", key, inputsKey);
			}

			if (withGuidFormat)
			{
				using (var provider = MD5.Create())
				{
					byte[] data = provider.ComputeHash(Encoding.Default.GetBytes(key));
					key = new Guid(data).ToString();
				}
			}

			return key;
		}

		#endregion
	}

	class CachingUserState
	{
		public string CacheKey
		{
			get;
			set;
		}

		public CachedOperationResult CacheItem
		{
			get;
			set;
		}

		public AsyncCallback Callback
		{
			get;
			set;
		}

		public object State
		{
			get;
			set;
		}
	}

	class CachedOperationResult
	{
		public object Data
		{
			get;
			set;
		}

		public object[] Outputs
		{
			get;
			set;
		}

		public object GetValue(object[] inputs, out object[] outputs)
		{
			outputs = this.Outputs;

			return this.Data;
		}
	}

	class CachingAsyncResult : IAsyncResult
	{
		private IAsyncResult _result;
		private CachingUserState _state;

		public CachingAsyncResult(IAsyncResult result, CachingUserState state)
		{
			_result = result;
			_state = state;
		}

		public object AsyncState
		{
			get { return _state.State; }
		}

		public WaitHandle AsyncWaitHandle
		{
			get { return _result.AsyncWaitHandle; }
		}

		public bool CompletedSynchronously
		{
			get { return _result.CompletedSynchronously; }
		}

		public bool IsCompleted
		{
			get { return _result.IsCompleted; }
		}

		internal CachingUserState CachingUserState
		{
			get { return _state; }
		}

		internal IAsyncResult OriginalAsyncResult
		{
			get { return _result; }
		}
	}
}
