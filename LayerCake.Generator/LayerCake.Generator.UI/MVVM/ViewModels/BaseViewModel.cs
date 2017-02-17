// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;
	using System.Collections;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Windows;
	using System.Windows.Threading;

	public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
	{
		#region [ Members ]

		private TrackedPropertyController _trackedPropertyController = null;
		private RequiredPropertyController _requiredPropertyController = null;

		#endregion

		#region [ Constructor & Initialize ]

		public BaseViewModel()
		{
			this.InitializeControllers();

			this.Commands = new List<DelegateCommand>();

			this.PropertyChanged += this.OnPropertyChanged;

			this.IsEnabled = true;
		}

		public virtual void Initialize()
		{
		}

		private void InitializeControllers()
		{
			_trackedPropertyController = new TrackedPropertyController(this);
			_requiredPropertyController = new RequiredPropertyController(this);
		}

		#endregion

		#region [ Technicals ]

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { this.SetProperty(ref _isLoading, value); }
		}

		private bool _isEnabled;
		[DoNotPrune]
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { this.SetProperty(ref _isEnabled, value); }
		}

		#endregion

		#region [ Methods ]

		public bool IsTrackedProperty(string propertyName)
		{
			return _trackedPropertyController.IsTrackedProperty(propertyName);
		}

		#endregion

		#region [ Events ]

		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsLoading")
			{
				WaitMouseCursor.SetMouseCursor(this.IsLoading);
			}

			this.RaiseCanExecuteChanged();
		}

		#endregion

		#region [ Commands ]

		protected IList<DelegateCommand> Commands { get; set; }

		protected void RegisterCommand(DelegateCommand command)
		{
			if (command != null)
			{
				if (!this.Commands.Contains(command))
				{
					this.Commands.Add(command);
				}
			}
		}

		protected virtual void RaiseCanExecuteChanged()
		{
			foreach (var command in this.Commands)
			{
				Application.Current.Dispatcher.BeginInvokeAction(() =>
				{
					command.RaiseCanExecuteChanged();
				},
				DispatcherPriority.Normal);
			}
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

		/// <summary>
		/// Returns the name of the property given a lambda expression (base.GetPropertyName(() => TheProperty)).
		/// DO NOT USE THIS METHOD WITH OBFUSCATOR!
		/// </summary>
		/// 
		/// <param name="expression">
		/// The lambda expression.
		/// </param>
		/// 
		/// <returns>
		/// The name of the property given a lambda expression.
		/// </returns>
		protected string GetPropertyName(Expression<Func<object>> expression)
		{
			var lambda = expression as LambdaExpression;
			MemberExpression memberExpression;

			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = lambda.Body as UnaryExpression;
				memberExpression = unaryExpression.Operand as MemberExpression;
			}
			else
			{
				memberExpression = lambda.Body as MemberExpression;
			}

			System.Diagnostics.Debug.Assert(memberExpression != null, "Error: provide a lambda expression like '() => PropertyName'");

			if (memberExpression != null)
			{
				var propertyInfo = memberExpression.Member as PropertyInfo;
				return propertyInfo.Name;
			}

			return null;
		}

		#endregion

		#region [ INotifyDataErrorInfo Implementation ]

		private readonly ConcurrentDictionary<string, ICollection<string>> _validationErrors = new ConcurrentDictionary<string, ICollection<string>>();

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null)
		{
			if (this.ErrorsChanged != null)
			{
				this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
			}
		}

		public IEnumerable GetErrors(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
			{
				return null;
			}

			return _validationErrors[propertyName];
		}

		protected void AddValidationError(string propertyName, string error)
		{
			if (error != null)
			{
				ICollection<string> validationErrors = null;

				validationErrors = (_validationErrors.ContainsKey(propertyName)) ?
					validationErrors = _validationErrors[propertyName] :
					new List<string>();

				validationErrors.Add(error);

				_validationErrors[propertyName] = validationErrors;

				this.RaiseErrorsChanged(propertyName);
			}
		}

		protected void RemoveValidationErrors(string propertyName)
		{
			if (_validationErrors.ContainsKey(propertyName))
			{
				ICollection<string> validationErrors;
				_validationErrors.TryRemove(propertyName, out validationErrors);

				this.RaiseErrorsChanged(propertyName);
			}
		}

		public bool HasErrors
		{
			get
			{
				return _validationErrors.Count != 0 || _requiredPropertyController.HasErrors || !this.Validate();
			}
		}

		/// <summary>
		/// Overrides this method to extend the validation.
		/// </summary>
		/// 
		/// <returns>
		/// True if the ViewModel is correct; otherwise, false.
		/// </returns>
		protected virtual bool Validate()
		{
			return true;
		}

		#endregion
	}
}
