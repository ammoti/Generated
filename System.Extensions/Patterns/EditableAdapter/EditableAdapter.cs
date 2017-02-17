// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

// Based on http://www.codeproject.com/Articles/35066/Generic-implementation-of-IEditableObject-via-Type?fid=1538597&select=3098265&fr=11#xx0xx

namespace System
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;

	public sealed class EditableAdapter<T> : IEditableObject, ICustomTypeDescriptor, INotifyPropertyChanged where T : class
	{
		#region [ Members ]

		private T _backup = null;

		#endregion

		#region [ Constructor ]

		public EditableAdapter(T target)
		{
			this.Target = target;
		}

		#endregion

		#region [ Properties ]

		public T Target { get; set; }

		/// <summary>
		/// Gets value indicating whether the object is in edition mode (BeginEdit()).
		/// </summary>
		public bool IsEditing { get; private set; }

		#endregion

		#region [ IEditableObject Implementation ]

		/// <summary>
		/// [BEGIN] Begins an edit on an object.
		/// </summary>
		public void BeginEdit()
		{
			this.IsEditing = true;

			_backup = this.Target.DeepCopy<T>();
		}

		/// <summary>
		/// [ROLLBACK] Discards changes since the last System.ComponentModel.IEditableObject.BeginEdit() call.
		/// </summary>
		public void CancelEdit()
		{
			if (_backup != null)
			{
				Type targetType = this.Target.GetType();

				var props = _backup.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
								   .Where(p => p.CanRead && p.CanWrite);

				foreach (var prop in props)
				{
					targetType.GetProperty(prop.Name).SetValue(this.Target, prop.GetValue(_backup));
				}
			}

			this.IsEditing = false;
		}

		/// <summary>
		/// [COMMIT] Pushes changes since the last System.ComponentModel.IEditableObject.BeginEdit() or System.ComponentModel.IBindingList.AddNew() call into the underlying object.
		/// </summary>
		public void EndEdit()
		{
			_backup = null;

			this.IsEditing = false;
		}

		#endregion

		#region [ ICustomTypeDescriptor Implementation ]

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			IList<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();

			var readonlyPropertyInfos =
				typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
						 .Where(p => p.CanRead && !p.CanWrite);

			foreach (var property in readonlyPropertyInfos)
			{
				var propertyCopy = property;

				var propertyDescriptor = PropertyDescriptorFactory.CreatePropertyDescriptor(
					property.Name,
					typeof(T),
					property.PropertyType,
					(component) => propertyCopy.GetValue(((EditableAdapter<T>)component).Target, null));

				propertyDescriptors.Add(propertyDescriptor);
			}

			var writablePropertyInfos =
				typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
						 .Where(p => p.CanRead && p.CanWrite);

			foreach (var property in writablePropertyInfos)
			{
				var propertyCopy = property;

				var propertyDescriptor = PropertyDescriptorFactory.CreatePropertyDescriptor(
					property.Name,
					typeof(T),
					property.PropertyType,
					(component) => propertyCopy.GetValue(((EditableAdapter<T>)component).Target, null),
					(component, value) => propertyCopy.SetValue(((EditableAdapter<T>)component).Target, value, null));

				propertyDescriptors.Add(propertyDescriptor);
			}

			return new PropertyDescriptorCollection(propertyDescriptors.ToArray());
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this.GetType());
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return TypeDescriptor.GetClassName(this.GetType());
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this.GetType());
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter(this.GetType());
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this.GetType());
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this.GetType());
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this.GetType(), editorBaseType);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this.GetType(), attributes);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this.GetType());
		}

		PropertyDescriptorCollection
		  ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(this.GetType(), attributes);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			//return this;
			throw new NotImplementedException();

		}

		#endregion

		#region [ INotifyPropertyChanged Implementation ]

		private event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if (this.Target is INotifyPropertyChanged)
				{
					this.PropertyChanged += value;
					((INotifyPropertyChanged)this.Target).PropertyChanged += this.NotifyPropertyChanged;
				}
			}

			remove
			{
				if (this.Target is INotifyPropertyChanged)
				{
					this.PropertyChanged -= value;
					((INotifyPropertyChanged)this.Target).PropertyChanged -= this.NotifyPropertyChanged;
				}
			}
		}

		#endregion
	}
}
