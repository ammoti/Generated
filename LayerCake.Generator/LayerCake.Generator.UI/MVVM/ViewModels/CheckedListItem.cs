// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public class CheckedListItem<T> : INotifyPropertyChanged where T : class, new()
	{
		#region [ Constructors ]

		public CheckedListItem()
		{
		}

		public CheckedListItem(T item, bool isChecked = false)
		{
			this.Item = item;
			this.IsChecked = isChecked;
		}

		#endregion

		#region [ Properties ]

		private T _item;
		public T Item
		{
			get { return _item; }
			set { this.SetProperty(ref _item, value); }
		}

		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set { this.SetProperty(ref _isChecked, value); }
		}

		#endregion

		#region [ Methods ]

		public override string ToString()
		{
			return string.Format("{0} (isChecked = {1}", this.Item, this.IsChecked);
		}

		#endregion

		#region [ INotifyPropertyChanged Implementation ]

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected bool SetProperty<TProperty>(ref TProperty member, TProperty value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<TProperty>.Default.Equals(member, value))
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
