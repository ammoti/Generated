﻿//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was generated by LayerCake Generator v3.7.1.
// http://www.layercake-generator.net
// 
// Changes to this file may cause incorrect behavior AND WILL BE LOST IF 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.Data;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;

	using VahapYigit.Test.Core;

	/// <summary>
	/// Entity mapped to ExecutionTrace DB table.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	[System.CodeDom.Compiler.GeneratedCode("LayerCake Generator", "3.7.1")]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage()]
	public partial class ExecutionTrace : EntityBase, IExecutionTrace
	{
		/// <summary>
		/// Gets the name of the entity.
		/// </summary>
		public static readonly string EntityName = "ExecutionTrace";

		#region [ Constructor ]

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExecutionTrace()
			: base()
		{
			this.Initialize();
		}

		#endregion

		#region [ Events ]

		#endregion

		#region [ Column Names ]

		/// <summary>
		/// Contains the entity column names.
		/// </summary>
		public static partial class ColumnNames
		{
			/// <summary>
			/// Name of the Id column ("ExecutionTrace_Id").
			/// </summary>
			public static readonly string Id = "ExecutionTrace_Id";

			/// <summary>
			/// Name of the Module column ("ExecutionTrace_Module").
			/// </summary>
			public static readonly string Module = "ExecutionTrace_Module";

			/// <summary>
			/// Name of the ClassName column ("ExecutionTrace_ClassName").
			/// </summary>
			public static readonly string ClassName = "ExecutionTrace_ClassName";

			/// <summary>
			/// Name of the MethodName column ("ExecutionTrace_MethodName").
			/// </summary>
			public static readonly string MethodName = "ExecutionTrace_MethodName";

			/// <summary>
			/// Name of the Tag column ("ExecutionTrace_Tag").
			/// </summary>
			public static readonly string Tag = "ExecutionTrace_Tag";

			/// <summary>
			/// Name of the MinDuration column ("ExecutionTrace_MinDuration").
			/// </summary>
			public static readonly string MinDuration = "ExecutionTrace_MinDuration";

			/// <summary>
			/// Name of the MaxDuration column ("ExecutionTrace_MaxDuration").
			/// </summary>
			public static readonly string MaxDuration = "ExecutionTrace_MaxDuration";

			/// <summary>
			/// Name of the TotalDuration column ("ExecutionTrace_TotalDuration").
			/// </summary>
			public static readonly string TotalDuration = "ExecutionTrace_TotalDuration";

			/// <summary>
			/// Name of the Counter column ("ExecutionTrace_Counter").
			/// </summary>
			public static readonly string Counter = "ExecutionTrace_Counter";

			/// <summary>
			/// Name of the LastCall column ("ExecutionTrace_LastCall").
			/// </summary>
			public static readonly string LastCall = "ExecutionTrace_LastCall";

		}

		#endregion

		#region [ Property Names ]

		/// <summary>
		/// Contains the entity property names.
		/// </summary>
		public static partial class PropertyNames
		{
			/// <summary>
			/// Name of the Id property ("Id").
			/// </summary>
			public static readonly string Id = "Id";

			/// <summary>
			/// Name of the Module property ("Module").
			/// </summary>
			public static readonly string Module = "Module";

			/// <summary>
			/// Name of the ClassName property ("ClassName").
			/// </summary>
			public static readonly string ClassName = "ClassName";

			/// <summary>
			/// Name of the MethodName property ("MethodName").
			/// </summary>
			public static readonly string MethodName = "MethodName";

			/// <summary>
			/// Name of the Tag property ("Tag").
			/// </summary>
			public static readonly string Tag = "Tag";

			/// <summary>
			/// Name of the MinDuration property ("MinDuration").
			/// </summary>
			public static readonly string MinDuration = "MinDuration";

			/// <summary>
			/// Name of the MaxDuration property ("MaxDuration").
			/// </summary>
			public static readonly string MaxDuration = "MaxDuration";

			/// <summary>
			/// Name of the TotalDuration property ("TotalDuration").
			/// </summary>
			public static readonly string TotalDuration = "TotalDuration";

			/// <summary>
			/// Name of the Counter property ("Counter").
			/// </summary>
			public static readonly string Counter = "Counter";

			/// <summary>
			/// Name of the LastCall property ("LastCall").
			/// </summary>
			public static readonly string LastCall = "LastCall";

		}

		#endregion

		#region [ Properties ]

		#region [ Module ]

		private string _Module;

		/// <summary>
		/// Gets or sets the Module value (MANDATORY). 
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual string Module
		{
			get { return _Module; }
			set
			{
				if (value != _Module)
				{
					_Module = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateModule();
					}
				}
			}
		}

		/// <summary>
		/// Method called on Module value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the Module value is valid (format, value, etc).
		/// </param>
		partial void OnModuleValidation(ref TranslationEnum? error);

		private bool ValidateModule()
		{
			bool isValid = false;

			do
			{
				if (this.Module == null)
				{
					base.AddValidationError("Module", TranslationEnum.ModelExecutionTraceModuleIsRequired);
					break;
				}

				TranslationEnum? error = null;

				this.OnModuleValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("Module", error.Value);
					break;
				}

				base.RemoveValidationErrors("Module");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ ClassName ]

		private string _ClassName;

		/// <summary>
		/// Gets or sets the ClassName value (MANDATORY). 
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual string ClassName
		{
			get { return _ClassName; }
			set
			{
				if (value != _ClassName)
				{
					_ClassName = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateClassName();
					}
				}
			}
		}

		/// <summary>
		/// Method called on ClassName value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the ClassName value is valid (format, value, etc).
		/// </param>
		partial void OnClassNameValidation(ref TranslationEnum? error);

		private bool ValidateClassName()
		{
			bool isValid = false;

			do
			{
				if (this.ClassName == null)
				{
					base.AddValidationError("ClassName", TranslationEnum.ModelExecutionTraceClassNameIsRequired);
					break;
				}

				TranslationEnum? error = null;

				this.OnClassNameValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("ClassName", error.Value);
					break;
				}

				base.RemoveValidationErrors("ClassName");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ MethodName ]

		private string _MethodName;

		/// <summary>
		/// Gets or sets the MethodName value (MANDATORY). 
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual string MethodName
		{
			get { return _MethodName; }
			set
			{
				if (value != _MethodName)
				{
					_MethodName = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateMethodName();
					}
				}
			}
		}

		/// <summary>
		/// Method called on MethodName value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the MethodName value is valid (format, value, etc).
		/// </param>
		partial void OnMethodNameValidation(ref TranslationEnum? error);

		private bool ValidateMethodName()
		{
			bool isValid = false;

			do
			{
				if (this.MethodName == null)
				{
					base.AddValidationError("MethodName", TranslationEnum.ModelExecutionTraceMethodNameIsRequired);
					break;
				}

				TranslationEnum? error = null;

				this.OnMethodNameValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("MethodName", error.Value);
					break;
				}

				base.RemoveValidationErrors("MethodName");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ Tag ]

		private string _Tag;

		/// <summary>
		/// Gets or sets the Tag value (OPTIONAL). 
		/// </summary>
		[DebuggerHidden]
		[DataMember()]
		public virtual string Tag
		{
			get { return _Tag; }
			set
			{
				if (value != _Tag)
				{
					_Tag = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateTag();
					}
				}
			}
		}

		/// <summary>
		/// Method called on Tag value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the Tag value is valid (format, value, etc).
		/// </param>
		partial void OnTagValidation(ref TranslationEnum? error);

		private bool ValidateTag()
		{
			bool isValid = false;

			do
			{
				TranslationEnum? error = null;

				this.OnTagValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("Tag", error.Value);
					break;
				}

				base.RemoveValidationErrors("Tag");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ MinDuration ]

		private int _MinDuration;

		/// <summary>
		/// Gets or sets the MinDuration value (MANDATORY). Minimal duration (in millisecond)
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual int MinDuration
		{
			get { return _MinDuration; }
			set
			{
				if (value != _MinDuration)
				{
					_MinDuration = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateMinDuration();
					}
				}
			}
		}

		/// <summary>
		/// Method called on MinDuration value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the MinDuration value is valid (format, value, etc).
		/// </param>
		partial void OnMinDurationValidation(ref TranslationEnum? error);

		private bool ValidateMinDuration()
		{
			bool isValid = false;

			do
			{
				TranslationEnum? error = null;

				this.OnMinDurationValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("MinDuration", error.Value);
					break;
				}

				base.RemoveValidationErrors("MinDuration");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ MaxDuration ]

		private int _MaxDuration;

		/// <summary>
		/// Gets or sets the MaxDuration value (MANDATORY). Maximal duration (in millisecond)
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual int MaxDuration
		{
			get { return _MaxDuration; }
			set
			{
				if (value != _MaxDuration)
				{
					_MaxDuration = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateMaxDuration();
					}
				}
			}
		}

		/// <summary>
		/// Method called on MaxDuration value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the MaxDuration value is valid (format, value, etc).
		/// </param>
		partial void OnMaxDurationValidation(ref TranslationEnum? error);

		private bool ValidateMaxDuration()
		{
			bool isValid = false;

			do
			{
				TranslationEnum? error = null;

				this.OnMaxDurationValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("MaxDuration", error.Value);
					break;
				}

				base.RemoveValidationErrors("MaxDuration");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ TotalDuration ]

		private long _TotalDuration;

		/// <summary>
		/// Gets or sets the TotalDuration value (MANDATORY). Total duration (in millisecond)
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual long TotalDuration
		{
			get { return _TotalDuration; }
			set
			{
				if (value != _TotalDuration)
				{
					_TotalDuration = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateTotalDuration();
					}
				}
			}
		}

		/// <summary>
		/// Method called on TotalDuration value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the TotalDuration value is valid (format, value, etc).
		/// </param>
		partial void OnTotalDurationValidation(ref TranslationEnum? error);

		private bool ValidateTotalDuration()
		{
			bool isValid = false;

			do
			{
				TranslationEnum? error = null;

				this.OnTotalDurationValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("TotalDuration", error.Value);
					break;
				}

				base.RemoveValidationErrors("TotalDuration");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ Counter ]

		private long _Counter;

		/// <summary>
		/// Gets or sets the Counter value (MANDATORY). 
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual long Counter
		{
			get { return _Counter; }
			set
			{
				if (value != _Counter)
				{
					_Counter = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateCounter();
					}
				}
			}
		}

		/// <summary>
		/// Method called on Counter value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the Counter value is valid (format, value, etc).
		/// </param>
		partial void OnCounterValidation(ref TranslationEnum? error);

		private bool ValidateCounter()
		{
			bool isValid = false;

			do
			{
				TranslationEnum? error = null;

				this.OnCounterValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("Counter", error.Value);
					break;
				}

				base.RemoveValidationErrors("Counter");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#region [ LastCall ]

		private DateTime _LastCall;

		/// <summary>
		/// Gets or sets the LastCall value (MANDATORY). 
		/// </summary>
		[DebuggerHidden]
		[Required]
		[DataMember()]
		public virtual DateTime LastCall
		{
			get { return _LastCall; }
			set
			{
				if (value != _LastCall)
				{
					_LastCall = value;

					if (!_deserializing)
					{
						base.UpdateState();
					}

					this.NotifyPropertyChanged();

					if (!_deserializing)
					{
						this.ValidateLastCall();
					}
				}
			}
		}

		/// <summary>
		/// Method called on LastCall value validation.
		/// </summary>
		/// 
		/// <param name="error">
		/// Indicates whether the LastCall value is valid (format, value, etc).
		/// </param>
		partial void OnLastCallValidation(ref TranslationEnum? error);

		private bool ValidateLastCall()
		{
			bool isValid = false;

			do
			{
				if (this.LastCall == DateTime.MinValue)
				{
					base.AddValidationError("LastCall", TranslationEnum.ModelExecutionTraceLastCallIsRequired);
					break;
				}

				TranslationEnum? error = null;

				this.OnLastCallValidation(ref error);
				if (error != null)
				{
					base.AddValidationError("LastCall", error.Value);
					break;
				}

				base.RemoveValidationErrors("LastCall");
				isValid = true;
			}
			while (false);

			return isValid;
		}

		#endregion

		#endregion

		#region [ References ]

		#endregion

		#region [ EntityBase Implementation ]

		/// <summary>
		/// Gets the entity properties validity.
		/// </summary>
		/// 
		/// <param name="errors">
		/// Translation errors list (each item represents a translation key of an error).
		/// </param>
		/// 
		/// <returns>
		/// True if all the entity properties are correct; otherwise, false.
		/// </returns>
		public override bool IsValid(out IList<TranslationEnum> errors)
		{
			IList<TranslationEnum> errs = new List<TranslationEnum>();

			if (!this.ValidateModule())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("Module");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateClassName())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("ClassName");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateMethodName())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("MethodName");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateTag())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("Tag");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateMinDuration())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("MinDuration");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateMaxDuration())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("MaxDuration");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateTotalDuration())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("TotalDuration");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateCounter())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("Counter");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			if (!this.ValidateLastCall())
			{
				var iterator = ((INotifyDataErrorInfo)this).GetErrors("LastCall");
				iterator.ForEach(error => errs.Add((TranslationEnum)error));
			}

			errors = errs;

			return errors.Count == 0;
		}
		
		/// <summary>
		/// Fill the entity properties using a source.
		/// </summary>
		/// 
		/// <param name="source">
		/// Source.
		public override void Map(EntityBase source)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (!(source is ExecutionTrace))
			{
				ThrowException.ThrowArgumentException("The 'source' argument is not a 'ExecutionTrace' instance");
			}

			this.Module = ((ExecutionTrace)source).Module;
			this.ClassName = ((ExecutionTrace)source).ClassName;
			this.MethodName = ((ExecutionTrace)source).MethodName;
			this.Tag = ((ExecutionTrace)source).Tag;
			this.MinDuration = ((ExecutionTrace)source).MinDuration;
			this.MaxDuration = ((ExecutionTrace)source).MaxDuration;
			this.TotalDuration = ((ExecutionTrace)source).TotalDuration;
			this.Counter = ((ExecutionTrace)source).Counter;
			this.LastCall = ((ExecutionTrace)source).LastCall;

			this.Id = source.Id;
			this.State = source.State;
		}

		/// <summary>
		/// Fills the entity properties using a IDataReader object.
		/// </summary>
		/// 
		/// <param name="source">
		/// IDataReader object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (use base.UserContext).
		/// </param>
		/// 
		/// <param name="columnPrefix">
		/// Column prefix (optional).
		/// </param>
		public override void Map(IDataReader source, IUserContext userContext, string columnPrefix = null)
		{
			if (columnPrefix == null)
			{
				columnPrefix = string.Concat(EntityName, "_");
			}

			this.Id = TypeHelper.To<long>(source[string.Format("{0}Id", columnPrefix)]);
			this.Module = TypeHelper.To<string>(source[string.Format("{0}Module", columnPrefix)]);
			this.ClassName = TypeHelper.To<string>(source[string.Format("{0}ClassName", columnPrefix)]);
			this.MethodName = TypeHelper.To<string>(source[string.Format("{0}MethodName", columnPrefix)]);
			this.Tag = TypeHelper.To<string>(source[string.Format("{0}Tag", columnPrefix)]);
			this.MinDuration = TypeHelper.To<int>(source[string.Format("{0}MinDuration", columnPrefix)]);
			this.MaxDuration = TypeHelper.To<int>(source[string.Format("{0}MaxDuration", columnPrefix)]);
			this.TotalDuration = TypeHelper.To<long>(source[string.Format("{0}TotalDuration", columnPrefix)]);
			this.Counter = TypeHelper.To<long>(source[string.Format("{0}Counter", columnPrefix)]);
			this.LastCall = TypeHelper.To<DateTime>(source[string.Format("{0}LastCall", columnPrefix)]);

			this.State = EntityState.None;
		}

		/// <summary>
		/// Fill the entity properties using a DataRow object.
		/// </summary>
		/// 
		/// <param name="source">
		/// DataRow object.
		/// </param>
		/// 
		/// <param name="userContext">
		/// User context (use base.UserContext).
		/// </param>
		/// 
		/// <param name="columnPrefix">
		/// Column prefix (optional).
		/// </param>
		public override void Map(DataRow source, IUserContext userContext, string columnPrefix = null)
		{
			if (columnPrefix == null)
			{
				columnPrefix = string.Concat(EntityName, "_");
			}

			this.Id = TypeHelper.To<long>(source[string.Format("{0}Id", columnPrefix)]);
			this.Module = TypeHelper.To<string>(source[string.Format("{0}Module", columnPrefix)]);
			this.ClassName = TypeHelper.To<string>(source[string.Format("{0}ClassName", columnPrefix)]);
			this.MethodName = TypeHelper.To<string>(source[string.Format("{0}MethodName", columnPrefix)]);
			this.Tag = TypeHelper.To<string>(source[string.Format("{0}Tag", columnPrefix)]);
			this.MinDuration = TypeHelper.To<int>(source[string.Format("{0}MinDuration", columnPrefix)]);
			this.MaxDuration = TypeHelper.To<int>(source[string.Format("{0}MaxDuration", columnPrefix)]);
			this.TotalDuration = TypeHelper.To<long>(source[string.Format("{0}TotalDuration", columnPrefix)]);
			this.Counter = TypeHelper.To<long>(source[string.Format("{0}Counter", columnPrefix)]);
			this.LastCall = TypeHelper.To<DateTime>(source[string.Format("{0}LastCall", columnPrefix)]);

			this.State = EntityState.None;
		}
		
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
		public override void DeepMap(IDataReader source, IUserContext userContext = null)
		{
			this.Map(source, userContext);

			this.State = EntityState.None;
		}
		
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
		public override void DeepMap(DataRow source, IUserContext userContext = null)
		{
			this.Map(source, userContext);

			this.State = EntityState.None;
		}

		#endregion
	}
}