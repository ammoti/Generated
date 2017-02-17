// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;
	using System.Windows.Input;

	public class DelegateCommand : ICommand, INotifyActiveChanged
	{
		#region [ Members ]

		private Func<object, bool> _canExecute = null;
		private Action<object> _action = null;

		#endregion

		public DelegateCommand(Action<object> action)
			: this(action, null)
		{
		}

		public DelegateCommand(Action<object> action, Func<object, bool> canExecute)
		{
			if (action == null)
			{
				ThrowException.ThrowArgumentNullException("action");
			}

			_action = action;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			bool result = true;

			if (_canExecute != null)
			{
				result = _canExecute(parameter);
			}

			return result;
		}

		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			if (this.CanExecuteChanged != null)
			{
				this.CanExecuteChanged(this, new EventArgs());
			}
		}

		public void Execute(object parameter)
		{
			this.IsActive = true;

			_action(parameter);

			this.IsActive = false;
		}

		#region [ INotifyActiveChanged Implementation ]

		public event EventHandler IsActiveChanged;

		protected virtual void RaiseIsActiveChanged()
		{
			if (this.IsActiveChanged != null)
			{
				this.IsActiveChanged(this, EventArgs.Empty);
			}
		}

		private bool _isActive;

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (value != _isActive)
				{
					_isActive = value;
					this.RaiseIsActiveChanged();
				}
			}
		}

		#endregion
	}
}
