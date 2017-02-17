// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Collections;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using System.Runtime.Serialization;

	using VahapYigit.Test.Core;

	/// <summary>
	/// EntityBase is the base class for all entities.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	public abstract partial class EntityBase : IEntity, INotifyPropertyChanged, INotifyDataErrorInfo, IDataRowMapping, IDataReaderMapping
	{
		#region [ Members ]

		[NonSerialized]
		private static RequiredPropertyController _requiredPropertyController = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected EntityBase()
		{
			this.RegisterRequiredPropertyController();

			this.State = EntityState.None;
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Initialize the object when the instance is created.
		/// </summary>
		protected virtual void Initialize()
		{
		}

		private void RegisterRequiredPropertyController()
		{
			if (_requiredPropertyController != null)
			{
				// WCF does not allow to use [DataContract(IsReference = true)] (required for serialization with circular refs) and [DataMember(IsRequired = true)] (required for entity validation) together...
				// Thus we must use custom attribute to set required properties.

				_requiredPropertyController = new RequiredPropertyController(this, func: null /*TODO*/);
			}
		}

		protected void UpdateState()
		{
			this.State = (this.IsInDb) ? EntityState.ToUpdate : EntityState.ToInsert;
		}

		/// <summary>
		/// Returns the entity Xml representation (using XmlSerializerType.DataContract).
		/// </summary>
		/// 
		/// <returns>
		/// The entity Xml representation.
		/// </returns>
		public string ToXml()
		{
			return SerializerHelper.ToXml(SerializerType.DataContract, this);
		}

		/// <summary>
		/// Returns the Id and State of the entity.
		/// </summary>
		/// 
		/// <returns>
		/// The Id and State of the entity.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0} - {1}", this.State, this.Id);
		}

		#endregion

		#region [ Abstract Methods ]

		/// <summary>
		/// Indicates if the entity properties are correct.
		/// </summary>
		/// 
		/// <param name="errors">
		/// Translation errors list.
		/// </param>
		/// 
		/// <returns>
		/// True if the entity properties are correct; otherwise, false.
		/// </returns>
		public abstract bool IsValid(out IList<TranslationEnum> errors);

		/// <summary>
		/// Fill the entity properties using a source.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source.
		/// </param>
		public abstract void Map(EntityBase source);

		/// <summary>
		/// Fill the entity properties using a IDataReader object.
		/// </summary>
		/// 
		/// <param name="source">
		/// IDataReader object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		/// 
		/// <param name="columnPrefix">
		/// Column prefix (optional).
		/// </param>
		public abstract void Map(IDataReader source, IUserContext userContext = null, string columnPrefix = null);

		/// <summary>
		/// Fill the entity properties using a DataRow object.
		/// </summary>
		/// 
		/// <param name="source">
		/// DataRow object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		/// 
		/// <param name="columnPrefix">
		/// Column prefix (optional).
		/// </param>
		public abstract void Map(DataRow source, IUserContext userContext = null, string columnPrefix = null);

		/// <summary>
		/// Fills the entity properties and all its dependencies using a IDataReader object.
		/// </summary>
		/// 
		/// <param name="source">
		/// IDataReader object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		public abstract void DeepMap(IDataReader source, IUserContext userContext = null);

		/// <summary>
		/// Fill the entity properties and all its dependencies using a DataRow object.
		/// </summary>
		/// 
		/// <param name="source">
		/// DataRow object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (optional).
		/// </param>
		public abstract void DeepMap(DataRow source, IUserContext userContext = null);

		#endregion

		#region [ Serialization Methods ]

		protected bool _deserializing = false;

		/// <summary>
		/// Method called during the deserialization of an entity.
		/// </summary>
		/// 
		/// <param name="context">
		/// Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.
		/// </param>
		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			_deserializing = true;

			this.OnPostDeserializing();
		}

		/// <summary>
		/// Method called immediately after deserialization of an entity.
		/// </summary>
		/// 
		/// <param name="context">
		/// Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.
		/// </param>
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			// Reinitialize members after deserialization to avoid NullReferenceExceptions.

			this.RegisterRequiredPropertyController();

			if (_validationErrors == null)
			{
				_validationErrors = new ConcurrentDictionary<string, ICollection<TranslationEnum>>();
			}

			_deserializing = false;

			this.OnPostDeserialized();
		}

		protected virtual void OnPostDeserializing()
		{
		}

		protected virtual void OnPostDeserialized()
		{
		}

		#endregion

		#region [ IEntity Implementation ]

		#region [ Id ]

		/// <summary>
		/// Gets or sets the unique ID.
		/// </summary>
		[DebuggerHidden]
		[DataMember()]
		public long Id { get; set; }

		#endregion

		#region [ State ]

		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		[DebuggerHidden]
		[DataMember(Order = 9999)]
		public EntityState State { get; set; }

		#endregion

		#region [ IsInDb ]

		/// <summary>
		/// Gets the value indicating whether the entity has been loaded from the DB (if Id > 0).
		/// </summary>
		[DebuggerHidden]
		public bool IsInDb { get { return this.Id > 0; } }

		#endregion

		#endregion

		#region [ INotifyDataErrorInfo Implementation ]

		private ConcurrentDictionary<string, ICollection<TranslationEnum>> _validationErrors = new ConcurrentDictionary<string, ICollection<TranslationEnum>>();

		[field: NonSerialized]
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null)
		{
			this.OnErrorsChanged(propertyName);

			if (this.ErrorsChanged != null)
			{
				this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
			}
		}

		protected virtual void OnErrorsChanged(string propertyName)
		{
		}

		/// <summary>
		/// Gets the validation errors for a property. You should'nt use this method.
		/// Use IsValid() instead.
		/// </summary>
		/// 
		/// <param name="propertyName">
		/// Property name.
		/// </param>
		/// 
		/// <returns>
		/// Errors.
		/// </returns>
		IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return null;
			}

			if (!_validationErrors.ContainsKey(propertyName))
			{
				return null;
			}

			return _validationErrors[propertyName];
		}

		/// <summary>
		/// Determines whether the entity has validation errors. You should'nt use this property.
		/// Use IsValid() instead.
		/// </summary>
		bool INotifyDataErrorInfo.HasErrors
		{
			get
			{
				return _validationErrors.Count != 0 || _requiredPropertyController.HasErrors;
			}
		}

		protected void AddValidationError(string propertyName, TranslationEnum error)
		{
			ICollection<TranslationEnum> validationErrors =
				(_validationErrors.ContainsKey(propertyName)) ?
					validationErrors = _validationErrors[propertyName] :
					new Collection<TranslationEnum>();

			if (!validationErrors.Contains(error))
			{
				validationErrors.Add(error);

				_validationErrors[propertyName] = validationErrors;

				this.RaiseErrorsChanged(propertyName);
			}
		}

		protected void RemoveValidationErrors(string propertyName)
		{
			if (_validationErrors.ContainsKey(propertyName))
			{
				ICollection<TranslationEnum> validationErrors;
				_validationErrors.TryRemove(propertyName, out validationErrors);

				this.RaiseErrorsChanged(propertyName);
			}
		}

		protected ICollection<TranslationEnum> GetValidationError(string propertyName)
		{
			if (_validationErrors == null)
			{
				return null;
			}

			return _validationErrors[propertyName];
		}

		#endregion

		#region [ INotifyPropertyChanged Implementation ]

		/// <summary>
		/// Event raised on property changed.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Event raised on property changed.
		/// </summary>
		/// 
		/// <param name="propertyName">
		/// Property name.
		/// </param>
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.OnPropertyChanged(propertyName);

			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
		}

		/// <summary>
		/// Affects the value to the member.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the value to affect.
		/// </typeparam>
		/// 
		/// <param name="member">
		/// Private member of the class to affect.
		/// </param>
		/// 
		/// <param name="value">
		/// Value to affect to the member.
		/// </param>
		/// 
		/// <param name="propertyName">
		/// Property name.
		/// </param>
		/// 
		/// <returns>
		/// True if the affectation is done; otherwise, false (the value is equal to the member one).
		/// </returns>
		protected bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(member, value))
			{
				return false;
			}

			member = value;
			this.NotifyPropertyChanged(propertyName);

			return true;
		}

		#endregion
	}
}
