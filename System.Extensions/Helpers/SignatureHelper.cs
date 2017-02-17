// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
//
// Based on https://gist.github.com/kellyelton/4477556
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	public static class MethodInfoExtensions
	{
		#region [ Public methods ]

		public static string GetSignature(this PropertyInfo property)
		{
			var getter = property.GetGetMethod();
			var setter = property.GetSetMethod();

			var sbSignature = new StringBuilder();
			var primaryDef = LeastRestrictiveVisibility(getter, setter);

			BuildReturnSignature(sbSignature, primaryDef);
			sbSignature.Append(" { ");

			if (getter != null)
			{
				if (primaryDef != getter)
				{
					sbSignature.Append(GetAccessModifier(getter) + " ");
				}

				sbSignature.Append("get; ");
			}
			if (setter != null)
			{
				if (primaryDef != setter)
				{
					sbSignature.Append(GetAccessModifier(setter) + " ");
				}

				sbSignature.Append("set; ");
			}

			sbSignature.Append("}");
			return sbSignature.ToString();
		}

		public static string GetSignature(this MethodInfo method, bool callable = false)
		{
			var sbSignature = new StringBuilder();

			BuildReturnSignature(sbSignature, method, callable);

			sbSignature.Append("(");

			var firstParam = true;
			var secondParam = false;
			var parameters = method.GetParameters();

			foreach (var param in parameters)
			{
				if (firstParam)
				{
					firstParam = false;
					if (method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
					{
						if (callable)
						{
							secondParam = true;
							continue;
						}

						sbSignature.Append("this ");
					}
				}
				else if (secondParam == true)
				{
					secondParam = false;
				}
				else
				{
					sbSignature.Append(", ");
				}
				if (param.IsOut)
				{
					sbSignature.Append("out ");
				}
				else if (param.ParameterType.IsByRef)
				{
					sbSignature.Append("ref ");
				}

				if (IsParamArray(param))
				{
					sbSignature.Append("params ");
				}

				if (!callable)
				{
					sbSignature.Append(TypeName(param.ParameterType));
					sbSignature.Append(' ');
				}

				sbSignature.Append(param.Name);

				if (param.IsOptional)
				{
					if (param.ParameterType.IsGenericParameter)
					{
						sbSignature.AppendFormat(" = {0}", (param.DefaultValue ?? "default(T)"));
					}
					else
					{
						if (param.DefaultValue is bool)
						{
							sbSignature.AppendFormat(" = {0}", param.DefaultValue.ToString().ToLowerInvariant());
						}
						else
						{
							sbSignature.AppendFormat(" = {0}", (param.DefaultValue ?? "null"));
						}
					}
				}
			}

			sbSignature.Append(")");

			// generic constraints

			foreach (var arg in method.GetGenericArguments())
			{
				List<string> constraints = new List<string>();
				foreach (var constraint in arg.GetGenericParameterConstraints())
				{
					constraints.Add(TypeName(constraint));
				}

				var attrs = arg.GenericParameterAttributes;

				if (attrs.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
				{
					constraints.Add("class");
				}

				if (attrs.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
				{
					constraints.Add("struct");
				}

				if (attrs.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
				{
					constraints.Add("new()");
				}

				if (constraints.Count > 0)
				{
					sbSignature.Append(" where " + TypeName(arg) + ": " + string.Join(", ", constraints));
				}
			}

			return sbSignature.ToString();
		}

		public static string GetSignatureAsync(this MethodInfo method, bool callable = false)
		{
			string[] c1 = method.GetSignature().Split(new char[] { ' ' }, 3);
			string[] c2 = c1[2].Split(new char[] { '(' }, 2);

			if (callable)
			{
				throw new NotImplementedException("MethodInfoExtensions::GetSignatureAsync(callable = true)");
			}

			if (IsVoidReturnType(method.ReturnType))
			{
				return string.Format("{0} async Task {1}Async({2}", c1[0], c2[0], c2[1]);
			}
			else
			{
				return string.Format("{0} async Task<{1}> {2}Async({3}", c1[0], c1[1], c2[0], c2[1]);
			}
		}

		public static string GetContractSignature(this MethodInfo method)
		{
			string[] signature = method.GetSignature().Split(new char[] { ' ' }, 2);
			return signature[1];
		}

		public static string GetContractSignatureAsync(this MethodInfo method)
		{
			string[] c1 = method.GetContractSignature().Split(new char[] { ' ' }, 2); // public | void MyMethod(...)
			string[] c2 = c1[1].Split(new char[] { '(' }, 2); // void MyMethod | ...)

			if (IsVoidReturnType(method.ReturnType))
			{
				return string.Format("Task {0}Async({1}", c2[0], c2[1]);
			}
			else
			{
				return string.Format("Task<{0}> {1}Async({2}", c1[0], c2[0], c2[1]);
			}
		}

		public static string TypeName(Type type)
		{
			var nullableType = Nullable.GetUnderlyingType(type);
			if (nullableType != null)
			{
				return TypeName(nullableType) + "?";
			}

			if (!type.IsGenericType && !type.Name.Contains("`"))
			{
				if (type.IsArray)
				{
					return TypeName(type.GetElementType()) + "[]";
				}

				switch (type.Name)
				{
					case "String":
						return "string";
					case "Int16":
						return "short";
					case "UInt16":
						return "ushort";
					case "Int32":
						return "int";
					case "UInt32":
						return "uint";
					case "Int64":
						return "long";
					case "UInt64":
						return "ulong";
					case "Decimal":
						return "decimal";
					case "Double":
						return "double";
					case "Boolean":
						return "bool";
					case "Object":
						return "object";
					case "Void":
						return "void";

					default:
						string typeName = string.IsNullOrWhiteSpace(type.FullName) ? type.Name : type.FullName;

						if (typeName.EndsWith("&"))
						{
							typeName = typeName.Substring(0, typeName.Length - 1);
						}

						typeName = typeName.Replace('+', '.');

						return typeName;
				}
			}

			var sb = new StringBuilder(type.FullName.Substring(0, type.FullName.IndexOf('`')));
			sb.Append('<');

			var first = true;

			foreach (var t in type.GetGenericArguments())
			{
				if (!first)
				{
					sb.Append(',');
				}

				sb.Append(TypeName(t));
				first = false;
			}

			sb.Append('>');
			return sb.ToString();
		}

		public static bool HasOutOrRefParameters(this MethodInfo method)
		{
			return method.GetParameters().Any(p => p.IsOut || p.ParameterType.IsByRef);
		}

		#endregion

		#region [ Private methods ]

		private static bool IsVoidReturnType(Type returnType)
		{
			return string.Compare(returnType.Name, "Void", true) == 0;
		}

		private static void BuildReturnSignature(StringBuilder sbSignature, MethodInfo method, bool callable = false)
		{
			var firstParam = true;
			if (callable == false)
			{
				sbSignature.Append(GetAccessModifier(method) + " ");

				if (method.IsStatic)
				{
					sbSignature.Append("static ");
				}

				sbSignature.Append(TypeName(method.ReturnType));
				sbSignature.Append(' ');
			}

			sbSignature.Append(method.Name);

			if (method.IsGenericMethod)
			{
				sbSignature.Append("<");
				foreach (var g in method.GetGenericArguments())
				{
					if (firstParam)
					{
						firstParam = false;
					}
					else
					{
						sbSignature.Append(", ");
					}

					sbSignature.Append(TypeName(g));
				}

				sbSignature.Append(">");
			}
		}

		private static string GetAccessModifier(MethodInfo method)
		{
			if (method.IsPublic)
				return "public";
			else if (method.IsPrivate)
				return "private";
			else if (method.IsAssembly)
				return "internal";
			else if (method.IsFamily)
				return "protected";

			ThrowException.ThrowFormatException("Unable to define the accessor modifier!");

			return string.Empty; // for compilation only
		}

		private static MethodInfo LeastRestrictiveVisibility(MethodInfo member1, MethodInfo member2)
		{
			if (member1 != null && member2 == null)
			{
				return member1;
			}
			else if (member2 != null && member1 == null)
			{
				return member2;
			}

			int vis1 = VisibilityValue(member1);
			int vis2 = VisibilityValue(member2);

			if (vis1 < vis2)
			{
				return member1;
			}
			else
			{
				return member2;
			}
		}

		private static int VisibilityValue(MethodInfo method)
		{
			if (method.IsPublic)
				return 1;
			else if (method.IsFamily)
				return 2;
			else if (method.IsAssembly)
				return 3;
			else if (method.IsPrivate)
				return 4;

			ThrowException.ThrowFormatException("Unable to define the visibility!");

			return -1; // for compilation only
		}

		private static bool IsParamArray(ParameterInfo info)
		{
			return info.GetCustomAttribute(typeof(ParamArrayAttribute), true) != null;
		}

		#endregion
	}
}
