// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.ServiceModel;

	[Serializable]
	public class BusinessContext
	{
		#region [ Members ]

		private readonly Assembly _businessAssembly = null;

		#endregion

		#region [ Constructor ]

		private BusinessContext()
		{
			this.BusinessClasses = new List<BusinessClassInfo>();
		}

		public BusinessContext(Assembly businessAssembly)
			: this()
		{
			if (businessAssembly == null)
			{
				ThrowException.ThrowArgumentNullException("assembly");
			}

			_businessAssembly = businessAssembly;
		}

		#endregion

		#region [ Methods ]

		public void Load(out IList<string> errors)
		{
			errors = new List<string>();

			foreach (var type in _businessAssembly.GetTypes())
			{
				var bc = BusinessClassInfo.Create(type, ref errors);
				if (bc != null)
				{
					this.BusinessClasses.Add(bc);
				}
			}
		}

		public BusinessClassInfo GetBusinessClass(string businessClassName)
		{
			return this.BusinessClasses.FirstOrDefault(i => i.Name == businessClassName);
		}

		#endregion

		#region [ Properties ]

		public IList<BusinessClassInfo> BusinessClasses { get; private set; }

		#endregion
	}

	[Serializable]
	public class BusinessClassInfo
	{
		#region [ Constructor ]

		private BusinessClassInfo()
		{
			this.BusinessMethods = new List<BusinessMethodInfo>();
		}

		#endregion

		#region [ Methods ]

		public static BusinessClassInfo Create(Type businessClassType, ref IList<string> errors)
		{
			var attr = businessClassType.GetCustomAttributes().FirstOrDefault(i => i.TypeId.ToString().EndsWith("BusinessClassAttribute"));
			if (attr == null)
			{
				return null;
			}

			int errorCount = errors.Count;

			var bc = new BusinessClassInfo();
			bc.Name = businessClassType.Name.Substring(0, businessClassType.Name.LastIndexOf("Business"));

			if (!businessClassType.IsPublic)
			{
				errors.Add(string.Format("'{0}.{1}' -> Business classes must be 'public'",
					businessClassType.Namespace, businessClassType.Name));
			}

			if (businessClassType.IsAbstract)
			{
				errors.Add(string.Format("'{0}.{1}' -> Business classes cannot be 'abstract'",
					businessClassType.Namespace, businessClassType.Name));
			}

			if (!businessClassType.Name.EndsWith("Business"))
			{
				errors.Add(string.Format("'{0}.{1}' -> Business classes must end with 'Business' suffix ({1}Business)",
					businessClassType.Namespace, businessClassType.Name));
			}

			bc.ServiceContractAttribute = businessClassType.GetCustomAttributes<ServiceContractAttribute>().FirstOrDefault();
			bc.ServiceBehaviorAttribute = businessClassType.GetCustomAttributes<ServiceBehaviorAttribute>().FirstOrDefault();

			foreach (var method in businessClassType.GetMethods() /* Only 'public' methods are returned */)
			{
				var bm = BusinessMethodInfo.Create(method, ref errors);
				if (bm != null)
				{
					// Check that the business method name is unique (for WCF layer)...

					bool bOverridedName = false;
					string bmWsdlOperationName = bm.Method.Name;

					if (bm.OperationContractAttribute != null && !string.IsNullOrEmpty(bm.OperationContractAttribute.Name))
					{
						bOverridedName = true;
						bmWsdlOperationName = bm.OperationContractAttribute.Name;
					}

					var tmpBm = bc.BusinessMethods.FirstOrDefault(i => i.Method.Name == bmWsdlOperationName);
					if (tmpBm != null)
					{
						if (bOverridedName)
						{
							errors.Add(string.Format("[OperationContract(Name = \"{2}\")] '{0}::{1}()' -> Business method names must be unique (another method has the same name of this OperationContract Name)",
								businessClassType.Name, bm.Method.Name, bm.OperationContractAttribute.Name));
						}
						else
						{
							errors.Add(string.Format("'{0}::{1}()' -> Business method names must be unique (another method has the same name)",
								businessClassType.Name, bm.Method.Name));
						}
					}
					else
					{
						tmpBm = bc.BusinessMethods.Where(i1 => i1.OperationContractAttribute != null)
												  .Where(i2 => !string.IsNullOrEmpty(i2.OperationContractAttribute.Name))
												  .FirstOrDefault(i3 => i3.OperationContractAttribute.Name == bmWsdlOperationName);

						if (tmpBm != null)
						{
							if (bOverridedName)
							{
								errors.Add(string.Format("[OperationContract(Name = \"{2}\")] '{0}::{1}()' -> Business method names must be unique (another method has the same OperationContract Name)",
									businessClassType.Name, bm.Method.Name, bm.OperationContractAttribute.Name));
							}
							else
							{
								errors.Add(string.Format("'{0}::{1}()' -> Business method names must be unique (another method has the name of this OperationContract Name)",
									businessClassType.Name, bm.Method.Name));
							}
						}
					}

					bc.BusinessMethods.Add(bm);
				}
			}

			return (errorCount == errors.Count) ? bc : null;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1} ({2})", base.ToString(), this.Name, this.BusinessMethods.Count);
		}

		#endregion

		#region [ Properties ]

		public string Name { get; private set; }

		public ServiceContractAttribute ServiceContractAttribute { get; private set; }

		public ServiceBehaviorAttribute ServiceBehaviorAttribute { get; private set; }

		public IList<BusinessMethodInfo> BusinessMethods { get; private set; }

		#endregion
	}

	[Serializable]
	public class BusinessMethodInfo
	{
		#region [ Constructor ]

		public BusinessMethodInfo()
		{
		}

		#endregion

		#region [ Methods ]

		public static BusinessMethodInfo Create(MethodInfo methodInfo, ref IList<string> errors)
		{
			var attr = methodInfo.GetCustomAttributes().FirstOrDefault(i => i.TypeId.ToString().EndsWith("BusinessMethodAttribute"));
			if (attr == null)
			{
				return null;
			}

			int errorCount = errors.Count;

			var bmi = new BusinessMethodInfo();
			bmi.Method = methodInfo;

			var bmiParameters = methodInfo.GetParameters();

			if (bmiParameters == null || bmiParameters.Length == 0 || bmiParameters[0].ParameterType.Name != "IUserContext" || bmiParameters[0].Name != "userContext")
			{
				errors.Add(string.Format("{0}::{1}() -> The first paramater must be 'IUserContext userContext'",
					methodInfo.DeclaringType.Name, methodInfo.Name));
			}

			#region [ OperationSecurityAttribute (Custom) ]

			bmi.OperationSecurityAttribute = methodInfo.GetCustomAttribute<OperationSecurityAttribute>();

			#endregion

			#region [ CachingOperationAttribute (Custom) ]

			bmi.CachingOperationAttribute = methodInfo.GetCustomAttribute<CachingOperationAttribute>();

			#endregion

			#region [ OperationContractAttribute (.NET ) ]

			bmi.OperationContractAttribute = methodInfo.GetCustomAttributes<OperationContractAttribute>().FirstOrDefault();

			#endregion

			#region [ OperationBehaviorAttribute (.NET ) ]

			bmi.OperationBehaviorAttribute = methodInfo.GetCustomAttributes<OperationBehaviorAttribute>().FirstOrDefault();
			if (bmi.OperationBehaviorAttribute != null)
			{
				//  Checking IsOneWay value...

				if (bmi.OperationContractAttribute.IsOneWay && methodInfo.ReturnType != typeof(void))
				{
					errors.Add(string.Format("{0}::{1}() -> IsOneWay = true -> Return Type must be 'void'", methodInfo.DeclaringType.Name, methodInfo.Name));
				}
			}

			#endregion

			#region [ TransactionFlow (.NET ) ]

			var tfAttr = methodInfo.GetCustomAttributes<TransactionFlowAttribute>().FirstOrDefault();
			bmi.TransactionFlowAttribute = tfAttr /* Can be null because cannot set default value on TransactionFlow.Transactions property */;

			#endregion

			return (errorCount == errors.Count) ? bmi : null;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1})", base.ToString(), this.Method.Name);
		}

		#endregion

		#region [ Properties ]

		public MethodInfo Method { get; set; }

		public OperationSecurityAttribute OperationSecurityAttribute { get; private set; }

		public CachingOperationAttribute CachingOperationAttribute { get; private set; }

		public OperationContractAttribute OperationContractAttribute { get; private set; }

		public OperationBehaviorAttribute OperationBehaviorAttribute { get; private set; }

		public TransactionFlowAttribute TransactionFlowAttribute { get; private set; }

		#endregion
	}
}
