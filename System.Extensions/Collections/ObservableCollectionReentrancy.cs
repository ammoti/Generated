// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Runtime.Serialization;
	using System.Windows.Threading;

	[Serializable]
	[CollectionDataContract(Namespace = "System.Extensions.ObservableCollectionReentrancy")]
	public class ObservableCollectionReentrancy<T> : ObservableCollection<T>
	{
		public ObservableCollectionReentrancy()
			: base()
		{
		}

		public ObservableCollectionReentrancy(IEnumerable<T> collection)
			: base(collection)
		{
		}

		public override event NotifyCollectionChangedEventHandler CollectionChanged;

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			using (base.BlockReentrancy())
			{
				NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;
				if (eventHandler != null)
				{
					Delegate[] delegates = eventHandler.GetInvocationList();
					if (delegates != null)
					{
						foreach (NotifyCollectionChangedEventHandler handler in delegates)
						{
							DispatcherObject dispatcherObject = handler.Target as DispatcherObject;
							if (dispatcherObject != null && !dispatcherObject.CheckAccess())
							{
								dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind, handler, this, e);
							}
							else
							{
								handler(this, e);
							}
						}
					}
				}
			}
		}
	}

	public static class ObservableCollectionReentrancyExtensions
	{
		public static ObservableCollectionReentrancy<T> ToObservableCollectionReentrancy<T>(this IEnumerable<T> source)
		{
			ObservableCollectionReentrancy<T> data = new ObservableCollectionReentrancy<T>();
			foreach (var item in source)
			{
				data.Add(item);
			}

			return data;
		}
	}
}
