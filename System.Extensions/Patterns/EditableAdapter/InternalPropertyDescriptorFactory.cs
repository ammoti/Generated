// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

// From http://www.codeproject.com/Articles/35066/Generic-implementation-of-IEditableObject-via-Type?fid=1538597&select=3098265&fr=11#xx0xx

namespace System
{
	using System.ComponentModel;

	internal class InternalPropertyDescriptorFactory : TypeConverter
	{
		public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(
			string name, Func<TComponent, TProperty> getter, Action<TComponent, TProperty> setter)
		{
			return new GenericPropertyDescriptor<TComponent, TProperty>(name, getter, setter);
		}

		public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(
			string name, Func<TComponent, TProperty> getter)
		{
			return new GenericPropertyDescriptor<TComponent, TProperty>(name, getter);
		}

		public static PropertyDescriptor CreatePropertyDescriptor(
			string name, Type componentType, Type propertyType, Func<object, object> getter, Action<object, object> setter)
		{
			return new GenericPropertyDescriptor(name, componentType, propertyType, getter, setter);
		}

		public static PropertyDescriptor CreatePropertyDescriptor(
			string name, Type componentType, Type propertyType, Func<object, object> getter)
		{
			return new GenericPropertyDescriptor(name, componentType, propertyType, getter);
		}

		protected class GenericPropertyDescriptor<TComponent, TProperty> : TypeConverter.SimplePropertyDescriptor
		{
			private Func<TComponent, TProperty> _getter;
			private Action<TComponent, TProperty> _setter;

			public GenericPropertyDescriptor(string name, Func<TComponent, TProperty> getter, Action<TComponent, TProperty> setter)
				: base(typeof(TComponent), name, typeof(TProperty))
			{
				if (getter == null)
				{
					ThrowException.ThrowArgumentNullException("getter");
				}

				if (setter == null)
				{
					ThrowException.ThrowArgumentNullException("setter");
				}

				_getter = getter;
				_setter = setter;
			}

			public GenericPropertyDescriptor(string name, Func<TComponent, TProperty> getter)
				: base(typeof(TComponent), name, typeof(TProperty))
			{
				if (getter == null)
				{
					ThrowException.ThrowArgumentNullException("getter");
				}

				_getter = getter;
			}

			public override bool IsReadOnly
			{
				get
				{
					return _setter == null;
				}
			}

			public override object GetValue(object target)
			{
				TComponent component = (TComponent)target;
				TProperty value = _getter(component);

				return value;
			}

			public override void SetValue(object target, object value)
			{
				if (!this.IsReadOnly)
				{
					TComponent component = (TComponent)target;
					TProperty newValue = (TProperty)value;

					_setter(component, newValue);
				}
			}
		}

		protected class GenericPropertyDescriptor : TypeConverter.SimplePropertyDescriptor
		{
			private Func<object, object> _getter;
			private Action<object, object> _setter;

			public GenericPropertyDescriptor(string name, Type componentType, Type propertyType, Func<object, object> getter, Action<object, object> setter)
				: base(componentType, name, propertyType)
			{
				if (getter == null)
				{
					ThrowException.ThrowArgumentNullException("getter");
				}

				if (setter == null)
				{
					ThrowException.ThrowArgumentNullException("setter");
				}

				_getter = getter;
				_setter = setter;
			}

			public GenericPropertyDescriptor(string name, Type componentType, Type propertyType, Func<object, object> getter)
				: base(componentType, name, propertyType)
			{
				if (getter == null)
				{
					ThrowException.ThrowArgumentNullException("getter");
				}

				_getter = getter;
			}

			public override bool IsReadOnly
			{
				get
				{
					return _setter == null;
				}
			}

			public override object GetValue(object target)
			{
				object value = _getter(target);
				return value;
			}

			public override void SetValue(object target, object value)
			{
				if (!this.IsReadOnly)
				{
					object newValue = (object)value;
					_setter(target, newValue);
				}
			}
		}
	}
}
