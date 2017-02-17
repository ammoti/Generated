// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;

	public static class SecureObjectHelper
	{
		public static void Secure(object obj, IEnumerable<SecurePropertyInfo> infos)
		{
			if (obj == null || infos.IsNullOrEmpty())
			{
				return;
			}

			var action = new Action<IEnumerable<PropertyInfo>, SecurePropertyInfo>((props, spi) =>
			{
				var prop = props.FirstOrDefault(p => p.Name == spi.PropertyName);
				if (prop != null)
				{
					var propValue = prop.GetValue(obj);
					prop.SetValue(obj, spi.Provider.Secure(propValue));
				}
			});

			var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.CanWrite);

			Parallel.ForEach(infos, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, info => action(properties, info));
		}

		public static void Unsecure(object obj, IEnumerable<SecurePropertyInfo> infos)
		{
			if (obj == null || infos.IsNullOrEmpty())
			{
				return;
			}

			var action = new Action<IEnumerable<PropertyInfo>, SecurePropertyInfo>((props, spi) =>
			{
				var prop = props.FirstOrDefault(p => p.Name == spi.PropertyName);
				if (prop != null)
				{
					var propValue = prop.GetValue(obj);
					prop.SetValue(obj, spi.Provider.Unsecure(propValue));
				}
			});

			var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.CanWrite);

			Parallel.ForEach(infos, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, info => action(properties, info));
		}
	}
}
