// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

// From http://stackoverflow.com/questions/1356045/set-focus-on-textbox-in-wpf-from-view-model-c

namespace LayerCake.Generator.UI.Behaviors
{
	using System;
	using System.Windows;

	public static class FocusBehavior
	{
		public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
			"IsFocused",
			typeof(bool?),
			typeof(FocusBehavior),
			new FrameworkPropertyMetadata(IsFocusedChanged));

		public static bool? GetIsFocused(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			return (bool?)element.GetValue(IsFocusedProperty);
		}

		public static void SetIsFocused(DependencyObject element, bool? value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			element.SetValue(IsFocusedProperty, value);
		}

		private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var fe = (FrameworkElement)d;

			if (e.OldValue == null)
			{
				fe.GotFocus += FrameworkElement_GotFocus;
				fe.LostFocus += FrameworkElement_LostFocus;
			}

			if (!fe.IsVisible)
			{
				fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(fe_IsVisibleChanged);
			}

			if ((bool)e.NewValue)
			{
				fe.Focus();
			}
		}

		private static void fe_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var fe = (FrameworkElement)sender;
			if (fe.IsVisible && (bool)((FrameworkElement)sender).GetValue(IsFocusedProperty))
			{
				fe.IsVisibleChanged -= fe_IsVisibleChanged;
				fe.Focus();
			}
		}

		private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
		}

		private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
		}
	}
}
