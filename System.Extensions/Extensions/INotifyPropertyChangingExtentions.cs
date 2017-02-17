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

	public static class INotifyPropertyChangingExtentions
	{
		public static void PropertyChangingAddHandler(this INotifyPropertyChanging source, EventHandler<PropertyChangingEventArgs> handler)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (handler == null)
			{
				ThrowException.ThrowArgumentNullException("handler");
			}

			WeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs>.AddHandler(source, "PropertyChanging", handler);
		}

		public static void PropertyChangingRemoveHandler(this INotifyPropertyChanging source, EventHandler<PropertyChangingEventArgs> handler)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (handler == null)
			{
				ThrowException.ThrowArgumentNullException("handler");
			}

			WeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs>.RemoveHandler(source, "PropertyChanging", handler);
		}
	}
}