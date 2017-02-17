// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel;
	using System.Windows;

	public static class INotifyPropertyChangedExtentions
	{
		public static void PropertyChangedAddHandler(this INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (handler == null)
			{
				ThrowException.ThrowArgumentNullException("handler");
			}

			WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(source, "PropertyChanged", handler);
		}

		public static void PropertyChangedRemoveHandler(this INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (handler == null)
			{
				ThrowException.ThrowArgumentNullException("handler");
			}

			WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(source, "PropertyChanged", handler);
		}
	}
}