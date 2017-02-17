// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using System.Runtime.Serialization;

	using VahapYigit.Test.Core;

	/// <summary>
	/// Model base class.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	public abstract class ModelBase : INotifyPropertyChanged
	{
		#region [ INotifyPropertyChanged Implementation ]

		/// <summary>
		/// Event raised on property changed.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Event raised on property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
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
