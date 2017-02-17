// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Runtime.Serialization;

	using VahapYigit.Test.Core;

	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	public class ExecutionTraceModel : ModelBase
	{
		#region [ Constructors ]

		public ExecutionTraceModel()
		{
			this.LastCall = DateTime.Now;
		}

		public ExecutionTraceModel(string module, string className, string methodName, string tag, long duration)
			: this()
		{
			this.Module = module;
			this.ClassName = className;
			this.MethodName = methodName;
			this.Tag = tag;
			this.Duration = duration;
		}

		#endregion

		#region [ Events ]

		protected override void OnPropertyChanged(string propertyName)
		{
			if (propertyName == "ClassName")
			{
				// Remove the `1[T] part at the end of the templated classes...
				// For example, VahapYigit.Test.Crud.CrudBase`1[T] -> VahapYigit.Test.Crud.CrudBase

				const string token = "`1[T]";

				if (this.ClassName != null && this.ClassName.EndsWith(token))
				{
					// Use the private member to avoid the NotifyPropertyChanged() loop...

					_className = this.ClassName.Substring(0, this.ClassName.Length - token.Length);
				}
			}
		}

		#endregion

		#region [ Properties ]

		private string _module;
		[DataMember]
		public string Module
		{
			get { return _module; }
			set { base.SetProperty(ref _module, value); }
		}

		private string _className;
		[DataMember]
		public string ClassName
		{
			get { return _className; }
			set { base.SetProperty(ref _className, value); }
		}

		private string _methodName;
		[DataMember]
		public string MethodName
		{
			get { return _methodName; }
			set { base.SetProperty(ref _methodName, value); }
		}

		private string _tag;
		[DataMember]
		public string Tag
		{
			get { return _tag; }
			set { base.SetProperty(ref _tag, value); }
		}

		private long _duration;
		[DataMember]
		public long Duration
		{
			get { return _duration; }
			set { base.SetProperty(ref _duration, value); }
		}

		private DateTime _lastCall;
		[DataMember]
		public DateTime LastCall
		{
			get { return _lastCall; }
			set { base.SetProperty(ref _lastCall, value); }
		}

		#endregion
	}
}