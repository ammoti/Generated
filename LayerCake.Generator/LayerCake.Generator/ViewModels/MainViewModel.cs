// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.ViewModels
{
	using LayerCake.Generator;
	using LayerCake.Generator.Commons;
	using LayerCake.Generator.UI;

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;

	class MainViewModel : BaseViewModel
	{
		#region [ Members ]

		private Processor _processor = null;

		private readonly object _processLogsLocker = new object();

		private bool _disabledSwitchTableSelectionModePropertyChangedEvent;

		private bool _disabledTableIsCheckedPropertyChangedEvent;

		#endregion

		#region [ Properties ]

		#region [ Tables ]

		private IList<CheckedListItem<TableInfo>> _tables;
		public IList<CheckedListItem<TableInfo>> Tables
		{
			get { return _tables; }
			set { base.SetProperty(ref _tables, value); }
		}

		#endregion

		#region [ CompilationModes ]

		private IEnumerable<CompilationModeEnum> _compilationModes;
		public IEnumerable<CompilationModeEnum> CompilationModes
		{
			get
			{
				if (_compilationModes == null)
				{
					_compilationModes = new List<CompilationModeEnum>();
				}

				return _compilationModes;
			}
			set { base.SetProperty(ref _compilationModes, value); }
		}

		#endregion

		#region [ SelectedCompilationMode ]

		private CompilationModeEnum _selectedCompilationMode;
		public CompilationModeEnum SelectedCompilationMode
		{
			get { return _selectedCompilationMode; }
			set { base.SetProperty(ref _selectedCompilationMode, value); }
		}

		#endregion

		#region [ WithSqlProcedureIntegration ]

		private bool _withSqlProcedureIntegration;
		public bool WithSqlProcedureIntegration
		{
			get { return _withSqlProcedureIntegration; }
			set { base.SetProperty(ref _withSqlProcedureIntegration, value); }
		}

		#endregion

		#region [ WithBusinessIntegration ]

		private bool _withBusinessIntegration;
		public bool WithBusinessIntegration
		{
			get { return _withBusinessIntegration; }
			set { base.SetProperty(ref _withBusinessIntegration, value); }
		}

		#endregion

		#region [ ProcessLogs ]

		private ObservableCollection<ProcessMessageEventArgs> _processLogs;
		public ObservableCollection<ProcessMessageEventArgs> ProcessLogs
		{
			get
			{
				if (_processLogs == null)
				{
					_processLogs = new ObservableCollection<ProcessMessageEventArgs>();
				}

				return _processLogs;
			}
			set { base.SetProperty(ref _processLogs, value); }
		}

		#endregion

		#endregion

		#region [ Technicals ]

		#region [ OnInitializing ]

		private bool _onInitializing;
		public bool OnInitializing
		{
			get { return _onInitializing; }
			set { base.SetProperty(ref _onInitializing, value); }
		}

		#endregion

		#region [ OnProcessing ]

		private bool _onProcessing;
		public bool OnProcessing
		{
			get { return _onProcessing; }
			set { base.SetProperty(ref _onProcessing, value); }
		}

		#endregion

		#region [ CanAcceptUserInteraction ]

		private bool _canAcceptUserInteraction;
		public bool CanAcceptUserInteraction
		{
			get { return _canAcceptUserInteraction; }
			set { base.SetProperty(ref _canAcceptUserInteraction, value); }
		}

		#endregion

		#region [ Versioning ]

		private bool _canBeUpdated;
		public bool CanBeUpdated
		{
			get { return _canBeUpdated; }
			set { base.SetProperty(ref _canBeUpdated, value); }
		}

		private string _updateVersion;
		public string UpdateVersion
		{
			get { return _updateVersion; }
			set { base.SetProperty(ref _updateVersion, value); }
		}

		#endregion

		#region [ SwitchTableSelectionMode ]

		private bool _switchTableSelection;
		public bool SwitchTableSelectionMode
		{
			get { return _switchTableSelection; }
			set { base.SetProperty(ref _switchTableSelection, value); }
		}

		#endregion

		#region [ HasExcludedTables ]

		private bool _hasExcludedTables;
		public bool HasExcludedTables
		{
			get { return _hasExcludedTables; }
			set { base.SetProperty(ref _hasExcludedTables, value); }
		}

		#endregion

		#region [ ProgressBar ]

		#region [ ProcessPercentage ]

		private int _progressPercentage;
		public int ProcessPercentage
		{
			get { return _progressPercentage; }
			set { base.SetProperty(ref _progressPercentage, value); }
		}

		#endregion

		#region [ WithProcessErrors ]

		private bool _withProcessErrors;
		public bool WithProcessErrors
		{
			get { return _withProcessErrors; }
			set { base.SetProperty(ref _withProcessErrors, value); }
		}

		#endregion

		#endregion

		#endregion

		#region [ Constructor & Initialize ]

		public MainViewModel()
		{
			#region [ Commands Registering ]

			this.QuitCommand = new DelegateCommand(this.Quit, this.CanExecuteQuit);
			base.RegisterCommand(this.QuitCommand);

			this.CancelCommand = new DelegateCommand(this.Cancel, this.CanExecuteCancel);
			base.RegisterCommand(this.CancelCommand);

			this.ClearLogsCommand = new DelegateCommand(this.ClearLogs, this.CanExecuteClearLogs);
			base.RegisterCommand(this.ClearLogsCommand);

			this.CopyLogsCommand = new DelegateCommand(this.CopyLogs, this.CanExecuteCopyLogs);
			base.RegisterCommand(this.CopyLogsCommand);

			this.ProcessCommand = new DelegateCommand(this.Process, this.CanExecuteProcess);
			base.RegisterCommand(this.ProcessCommand);

			this.GenerateTranslationsCommand = new DelegateCommand(this.GenerateTranslations, this.CanExecuteGenerateTranslations);
			base.RegisterCommand(this.GenerateTranslationsCommand);

			this.GenerateCodeRefsCommand = new DelegateCommand(this.GenerateCodeRefs, this.CanExecuteGenerateCodeRefs);
			base.RegisterCommand(this.GenerateCodeRefsCommand);

			this.ShowExcludedTablesCommand = new DelegateCommand(this.ShowExcludedTables, this.CanExecuteShowExcludedTables);
			base.RegisterCommand(this.ShowExcludedTablesCommand);

			#endregion

			#region [ IHM Initialization ]

			this.SwitchTableSelectionMode = true;

			this.WithSqlProcedureIntegration = true;
			this.WithBusinessIntegration = true;

			this.SetCompilationModes();

			#endregion

			#region [ Process Initialization ]

			_processor = new Processor();

			_processor.OnProcessMessage += OnProcessorProcessMessage;
			_processor.OnProcessProgression += OnProcessorProcessProgression;

			#endregion

			AsyncHelper.FireAndForget(() => this.Initialize());

			this.CheckUpdateAsync();
		}

		public override void Initialize()
		{
			this.OnInitializing = true;

			bool isInitialized = false;

			try
			{
				isInitialized = _processor.Initialize(App.ConfigFilePath);

				if (isInitialized)
				{
					PreferenceManager.ExecutionCount++;
					PreferenceManager.LastExecutionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					PreferenceManager.LastConfigFilePath = App.ConfigFilePath;

					IList<CheckedListItem<TableInfo>> tables = new List<CheckedListItem<TableInfo>>();

					_processor.TableNames.ForEach(tableName =>
					{
						tables.Add(new CheckedListItem<TableInfo>(new TableInfo(tableName), isChecked: true));
					});

					this.Tables = tables;

					foreach (var table in this.Tables) // register PropertyChanged events
					{
						table.PropertyChanged += this.OnTableIsCheckedPropertyChanged;
					}

					this.HasExcludedTables = !_processor.ExcludedTableNames.IsNullOrEmpty();
				}
			}
			catch (TypeInitializationException)
			{
				#region [ #BUG-001 ]

				// The type initializer for 'LayerCake.Generator.App' threw an exception.
				// The calling thread must be STA, because many UI components require this.

				// 1. To reproduce this bug, go to App.xaml.cs / App() and look for #BUG-001 keyword
				// 2. Uncomment the following line

				//MessageBoxHelper.Show(x.ToString(), "TypeInitializationException", MessageBoxButton.OK, MessageBoxImage.Error);

				#endregion
			}
			catch (Exception x)
			{
				MessageBoxHelper.Show(
					x.GetFullMessage(false, "\r\n\r\n"),
					"Processor Initialization Error!",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
			finally
			{
				this.OnInitializing = false;

				if (!isInitialized)
				{
					this.CanAcceptUserInteraction = false; // locks IHM (Quit button is available)
				}
			}
		}

		#endregion

		#region [ Methods ]

		private void CheckUpdateAsync()
		{
			if (!PreferenceManager.WithCheckAutomaticUpdates)
				return;

			AsyncHelper.FireAndForget(() =>
			{
				string date, version;
				bool updateReleased = ServiceManager.CheckUpdate(out date, out version);

				if (updateReleased)
				{
					this.CanBeUpdated = true;
					this.UpdateVersion = string.Format("v{0} released!", version);
				}
			});
		}

		private void SetCompilationModes()
		{
			IList<CompilationModeEnum> compilationsModes = new List<CompilationModeEnum>();

			compilationsModes.Add(CompilationModeEnum.Debug);
			compilationsModes.Add(CompilationModeEnum.Release);

			this.CompilationModes = compilationsModes;
			this.SelectedCompilationMode = this.CompilationModes.First(i => i == CompilationModeEnum.Debug);

		}

		private string GetLogs()
		{
			StringBuilder data = new StringBuilder(4096);

			lock (_processLogsLocker)
			{
				foreach (var entry in this.ProcessLogs)
				{
					string flag = string.Empty;
					if (entry.Type == ProcessMessageType.Error) flag = "*";

					data.AppendFormat("{0}{1} | {2}{3}",
						flag.PadRight(2, ' '),
						entry.Time,
						entry.Message,
						Environment.NewLine);
				}
			}

			return data.ToString();
		}

		private string GetProcessResult()
		{
			StringBuilder data = new StringBuilder(4096);

			data.AppendFormat("File -> {0} | ", System.IO.Path.GetFileName(App.ConfigFilePath));

			lock (_processLogsLocker)
			{
				foreach (var entry in this.ProcessLogs)
				{
					if (entry.Message.ToUpperInvariant().Contains("PROCESS DURATION")) // berk...
					{
						data.Append(entry.Message);
						break;
					}
				}
			}

			return data.ToString();
		}

		private void WriteProcessLog(ProcessMessageEventArgs e)
		{
			lock (_processLogsLocker)
			{
				Application.Current.Dispatcher.Invoke(() => this.ProcessLogs.Insert(0, e));
			}
		}

		#endregion

		#region [ Commands ]

		#region [ QuitCommand ]

		public DelegateCommand QuitCommand { get; set; }

		private bool CanExecuteQuit(object parameter)
		{
			return !this.OnInitializing && !this.OnProcessing; // do not use this.CanAcceptUserInteraction here!
		}

		private void Quit(object parameter)
		{
			Environment.Exit(0);
		}

		#endregion

		#region [ CancelCommand ]

		public DelegateCommand CancelCommand { get; set; }

		private bool CanExecuteCancel(object parameter)
		{
			return this.OnProcessing;
		}

		private void Cancel(object parameter)
		{
			this.WriteProcessLog(new ProcessMessageEventArgs(ProcessMessageType.Error, "Cancelling process... Please wait."));

			_processor.Stop();

			this.ProcessPercentage = 100;
		}

		#endregion

		#region [ ClearLogsCommand ]

		public DelegateCommand ClearLogsCommand { get; set; }

		private bool CanExecuteClearLogs(object parameter)
		{
			return this.ProcessLogs.Count != 0;
		}

		private void ClearLogs(object parameter)
		{
			lock (_processLogsLocker)
			{
				this.ProcessLogs = new ObservableCollection<ProcessMessageEventArgs>();
			}
		}

		#endregion

		#region [ CopyLogsCommand ]

		public DelegateCommand CopyLogsCommand { get; set; }

		private bool CanExecuteCopyLogs(object parameter)
		{
			return this.ProcessLogs.Count != 0;
		}

		private void CopyLogs(object parameter)
		{
			Clipboard.SetDataObject(this.GetLogs(), true);
		}

		#endregion

		#region [ ProcessCommand ]

		public DelegateCommand ProcessCommand { get; set; }

		private bool CanExecuteProcess(object parameter)
		{
			return !this.OnInitializing && !this.OnProcessing && !this.Tables.IsNullOrEmpty();
		}

		private void Process(object parameter)
		{
			this.OnProcessing = true;

			Task.Factory.StartNew(() =>
			{
				try
				{
					ProcessorParameters parameters = new ProcessorParameters
					{
						CompilationMode = this.SelectedCompilationMode.ToString(),
						TableNames = this.Tables.Where(t => t.IsChecked).Select(i => i.Item.Name),
						WithSqlProcedureIntegration = this.WithSqlProcedureIntegration
					};

					_processor.Execute(new FullProcessorBehavior(), parameters);

					this.OnProcessing = false;

					if (_processor.GeneratedServices != null &&
						_processor.GeneratedServices.Count != 0)
					{
						string message = "New Services has been generated and added to the solution.{0}{0}{1}{0}{0}Do not forget to register them if you use ServiceLocator, ServiceProxy or other IoC on client-side.";

						MessageBoxHelper.Show(
							string.Format(message, Environment.NewLine, string.Join(", ", _processor.GeneratedServices.OrderBy(i => i))),
							"Warning!",
							MessageBoxButton.OK,
							MessageBoxImage.Warning);
					}
				}
				catch (Exception x)
				{
					this.WriteProcessLog(new ProcessMessageEventArgs(ProcessMessageType.Error, "Processor Execution Error -> {0}", x.Message));
				}
			},
			TaskCreationOptions.LongRunning);
		}

		#endregion

		#region [ GenerateTranslationsCommand ]

		public DelegateCommand GenerateTranslationsCommand { get; set; }

		private bool CanExecuteGenerateTranslations(object parameter)
		{
			return true;
		}

		private void GenerateTranslations(object parameter)
		{
			this.OnProcessing = true;

			Task.Factory.StartNew(() =>
			{
				try
				{
					ProcessorParameters parameters = new ProcessorParameters
					{
						WithSqlProcedureIntegration = true,
						CompilationMode = this.SelectedCompilationMode.ToString(),
					};

					_processor.Execute(new TranslationProcessorBehavior(), parameters);
				}
				catch (Exception x)
				{
					this.WriteProcessLog(new ProcessMessageEventArgs(ProcessMessageType.Error, "Translations Generation Error -> {0}", x.Message));
				}

				this.OnProcessing = false;
			},
			TaskCreationOptions.LongRunning);
		}

		#endregion

		#region [ GenerateCodeRefsCommand ]

		public DelegateCommand GenerateCodeRefsCommand { get; set; }

		private bool CanExecuteGenerateCodeRefs(object parameter)
		{
			return true;
		}

		private void GenerateCodeRefs(object parameter)
		{
			this.OnProcessing = true;

			Task.Factory.StartNew(() =>
			{
				try
				{
					ProcessorParameters parameters = new ProcessorParameters
					{
						WithSqlProcedureIntegration = true,
						CompilationMode = this.SelectedCompilationMode.ToString(),
					};

					_processor.Execute(new CodeRefProcessorBehavior(), parameters);
				}
				catch (Exception x)
				{
					this.WriteProcessLog(new ProcessMessageEventArgs(ProcessMessageType.Error, "CodeRefs Generation Error -> {0}", x.Message));
				}

				this.OnProcessing = false;
			},
			TaskCreationOptions.LongRunning);
		}

		#endregion

		#region [ ShowExcludedTablesCommand ]

		public DelegateCommand ShowExcludedTablesCommand { get; set; }

		private bool CanExecuteShowExcludedTables(object parameter)
		{
			return _processor != null && !_processor.ExcludedTableNames.IsNullOrEmpty();
		}

		private void ShowExcludedTables(object parameter)
		{
			string message = string.Format(
				"Only tables with a primary key and at least a second column are listed. Excluded: {0}",
				string.Join(", ", _processor.ExcludedTableNames));

			MessageBoxHelper.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		#endregion

		#endregion

		#region [ Events ]

		private void OnTableIsCheckedPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var item = sender as CheckedListItem<TableInfo>;

			if (item.IsChecked)
			{
				this.SwitchTableSelectionMode = this.Tables.All(i => i.IsChecked);
			}
			else
			{
				_disabledSwitchTableSelectionModePropertyChangedEvent = true;

				this.SwitchTableSelectionMode = false;

				_disabledSwitchTableSelectionModePropertyChangedEvent = false;
			}

			// Manage dependencies...

			if (!_disabledTableIsCheckedPropertyChangedEvent)
			{
				_disabledTableIsCheckedPropertyChangedEvent = true;

				foreach (var tableDep in _processor.TableDependencies.Where(t => t.Key == item.Item.Name))
				{
					this.Tables.First(t => t.Item.Name == tableDep.Value).IsChecked = item.IsChecked;
				}

				_disabledTableIsCheckedPropertyChangedEvent = false;
			}
		}

		private void OnProcessorProcessMessage(object sender, ProcessMessageEventArgs e)
		{
			this.WithProcessErrors |= e.Type == ProcessMessageType.Error;

			Application.Current.Dispatcher.InvokeAction(() =>
			{
				this.WriteProcessLog(e);

				this.ClearLogsCommand.RaiseCanExecuteChanged();
				this.CopyLogsCommand.RaiseCanExecuteChanged();
			},
			DispatcherPriority.Normal);
		}

		private void OnProcessorProcessProgression(object sender, ProcessProgressionEventArgs e)
		{
			this.ProcessPercentage = (int)e.Percentage;
		}

		protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(sender, e);

			if (e.PropertyName == base.GetPropertyName(() => SwitchTableSelectionMode))
			{
				if (!_disabledSwitchTableSelectionModePropertyChangedEvent)
				{
					if (this.Tables != null)
					{
						foreach (var table in this.Tables)
						{
							table.PropertyChanged -= this.OnTableIsCheckedPropertyChanged;

							table.IsChecked = this.SwitchTableSelectionMode;

							table.PropertyChanged += this.OnTableIsCheckedPropertyChanged;
						}
					}
				}
			}

			if (e.PropertyName == base.GetPropertyName(() => OnInitializing) ||
				e.PropertyName == base.GetPropertyName(() => OnProcessing))
			{
				this.CanAcceptUserInteraction = !this.OnInitializing && !this.OnProcessing;
			}

			if (e.PropertyName == base.GetPropertyName(() => OnInitializing))
			{
				if (!this.OnInitializing) // when the initialization finishes...
				{
					if (this.WithProcessErrors)
					{
						ServiceManager.SubmitErrorAsync(this.GetLogs());
					}
				}
			}

			if (e.PropertyName == base.GetPropertyName(() => OnProcessing))
			{
				if (this.OnProcessing)
				{
					this.ProcessPercentage = 0;
					this.WithProcessErrors = false;

					PreferenceManager.ProcessCount++;
				}

				if (!this.OnProcessing) // when the processing is finished...
				{
					if (this.WithProcessErrors)
					{
						ServiceManager.SubmitErrorAsync(this.GetLogs()); // get errors for debugging
					}
					else
					{
						ServiceManager.SubmitProcessAsync(this.GetProcessResult()); // get stats
					}
				}
			}
		}

		#endregion
	}
}
