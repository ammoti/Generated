// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	[AttributeUsage(AttributeTargets.Method)]
	sealed public class CachingOperationAttribute : Attribute, IOperationBehavior
	{
		#region [ Properties ]

		/// <summary>
		/// Seconds
		/// </summary>
		public int Seconds
		{
			get;
			set;
		}

		/// <summary>
		/// Minutes
		/// </summary>
		public int Minutes
		{
			get;
			set;
		}

		/// <summary>
		/// Hours
		/// </summary>
		public int Hours
		{
			get;
			set;
		}

		/// <summary>
		/// Days
		/// </summary>
		public int Days
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the timeout value in second.
		/// </summary>
		public long TotalInSeconds
		{
			get
			{
				return this.Seconds + 60 * this.Minutes + 3600 * this.Hours + 86400 * this.Days;
			}
		}

		public Type KeyGeneratorType
		{
			get;
			private set;
		}

		/// <summary>
		/// Value indicationg the operation result depends on the user context (default value = false).
		/// If true then it means that the operation result depends on the user context.
		/// If false (default value) then it means that the operation result DOES NOT depend on the user context.
		/// </summary>
		public bool WithUserContextDependency
		{
			get;
			set;
		}

		#endregion

		#region [ Constructor ]

		public CachingOperationAttribute()
		{
		}

		public CachingOperationAttribute(Type keyGeneratorType)
			: this()
		{
			// If this constructor is not defined the exception is thrown
			// Method not found: Void CachingOperationAttribute(System.Type)

			// When using custom attributes that have a constructor parameter or a property of datatype "System.Type"
			// it is possible to get exceptions thrown instead of your attributes returned when you use reflection and
			// call MemberInfo.GetCustomAttributes(...)

			this.KeyGeneratorType = keyGeneratorType;
		}

		#endregion

		#region [ IOperationBehavior Implementation ]

		public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
		{
		}

		public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
		{
			dispatchOperation.Invoker = new CachingOperationInvoker(
				dispatchOperation.Invoker,
				operationDescription,
				this.TotalInSeconds,
				this.KeyGeneratorType,
				this.WithUserContextDependency);
		}

		public void Validate(OperationDescription operationDescription)
		{
		}

		#endregion
	}
}
