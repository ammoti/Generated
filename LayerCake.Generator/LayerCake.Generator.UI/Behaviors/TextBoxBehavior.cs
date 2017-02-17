// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Behaviors
{
	using System;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Media;
	using System.Windows.Media.Animation;

	public static class TextBoxBehavior
	{
		public static readonly DependencyProperty InfoTextProperty =
			DependencyProperty.RegisterAttached(
			  "InfoText",
			  typeof(string),
			  typeof(TextBoxBehavior),
			  new UIPropertyMetadata(null, InfoTextChanged));

		public static readonly DependencyProperty InfoTextForegroundProperty =
			DependencyProperty.RegisterAttached(
				"InfoTextForeground",
				typeof(Brush),
				typeof(TextBoxBehavior),
				new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66))));

		[AttachedPropertyBrowsableForType(typeof(TextBox))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static string GetInfoText(DependencyObject obj)
		{
			return (string)obj.GetValue(InfoTextProperty);
		}

		public static void SetInfoText(DependencyObject obj, string value)
		{
			obj.SetValue(InfoTextProperty, value);
		}

		public static Brush GetInfoTextForeground(DependencyObject obj)
		{
			return (Brush)obj.GetValue(InfoTextForegroundProperty);
		}

		public static void SetInfoTextForeground(DependencyObject obj, Brush value)
		{
			obj.SetValue(InfoTextForegroundProperty, value);
		}

		private static void InfoTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var element = o as FrameworkElement;
			var textBox = o as TextBox;
			var passwordBox = o as PasswordBox;

			if (textBox == null && passwordBox == null)
				return;

			var oldValue = (string)e.OldValue;
			var newValue = (string)e.NewValue;

			if (!element.IsVisible)
			{
				DependencyPropertyChangedEventHandler handler = null;
				handler = (s, args) =>
				{
					if (element.IsVisible)
					{
						InfoTextChanged(o, e);
						element.IsVisibleChanged -= handler;
					}
				};

				element.IsVisibleChanged += handler;
				return;
			}

			if (!string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue))
			{
				if (textBox != null) RemoveInfoTextAdorner(textBox);
				else if (passwordBox != null) RemoveInfoTextAdorner(passwordBox);
			}
			else if (!string.IsNullOrEmpty(newValue) && string.IsNullOrEmpty(oldValue))
			{
				if (textBox != null) AddInfoTextAdorner(textBox);
				else if (passwordBox != null) AddInfoTextAdorner(passwordBox);
			}
		}

		private static void AddInfoTextAdorner(TextBox textBox)
		{
			InfoTextAdorner adorner = null;
			Action updateVisibility = () => adorner.UpdateVisibility(string.IsNullOrEmpty(textBox.Text));
			TextChangedEventHandler handler = delegate { updateVisibility(); };
			Action disposeAction = () => textBox.TextChanged -= handler;

			adorner = new InfoTextAdorner(textBox, disposeAction);
			textBox.TextChanged += handler;
			updateVisibility();
			AddAdorner(textBox, adorner);
		}

		private static void AddInfoTextAdorner(PasswordBox passwordBox)
		{
			InfoTextAdorner adorner = null;
			Action updateVisibility = () => adorner.UpdateVisibility(passwordBox.SecurePassword == null || passwordBox.SecurePassword.Length == 0);
			RoutedEventHandler handler = delegate { updateVisibility(); };
			Action disposeAction = () => passwordBox.PasswordChanged -= handler;

			adorner = new InfoTextAdorner(passwordBox, disposeAction);
			passwordBox.PasswordChanged += handler;
			updateVisibility();
			AddAdorner(passwordBox, adorner);
		}

		private static void RemoveInfoTextAdorner(UIElement textBox)
		{
			var layer = AdornerLayer.GetAdornerLayer(textBox);
			var adorner = layer.GetAdorners(textBox).OfType<InfoTextAdorner>().FirstOrDefault();

			if (adorner != null)
			{
				layer.Remove(adorner);
				adorner.Dispose();
			}
		}

		private static void AddAdorner(UIElement adornedElement, Adorner adorner)
		{
			var layer = AdornerLayer.GetAdornerLayer(adornedElement);
			layer.Add(adorner);
		}

		private class InfoTextAdorner : Adorner, IDisposable
		{
			private readonly DoubleAnimation _showAnimation;
			private readonly DoubleAnimation _hideAnimation;
			private readonly DoubleAnimation _gotFocusAnimation;
			private readonly DoubleAnimation _lostFocusAnimation;

			private readonly Action _disposeAction;
			private readonly TextBlock _infoTextBlock;

			private bool _isVisible = true;

			public InfoTextAdorner(UIElement adornedElement, Action disposeAction)
				: base(adornedElement)
			{
				_disposeAction = disposeAction;

				_showAnimation = new DoubleAnimation(0.67, new Duration(TimeSpan.FromSeconds(0.3)));
				_hideAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(0.3)));
				_gotFocusAnimation = new DoubleAnimation(0.33, new Duration(TimeSpan.FromSeconds(0.3)));
				_lostFocusAnimation = new DoubleAnimation(0.67, new Duration(TimeSpan.FromSeconds(0.3)));

				_infoTextBlock = new TextBlock();
				//_infoTextBlock.FontStyle = FontStyles.Italic;
				_infoTextBlock.IsHitTestVisible = false;
				_infoTextBlock.Margin = new Thickness(2, 0, 2, 0);

				AdornedElement.GotFocus += AdornedElement_GotFocus;
				AdornedElement.LostFocus += AdornedElement_LostFocus;
				SetBinding(_infoTextBlock, TextBlock.TextProperty, adornedElement, TextBoxBehavior.InfoTextProperty);
				SetBinding(_infoTextBlock, TextBlock.FontSizeProperty, adornedElement, TextElement.FontSizeProperty);
				SetBinding(_infoTextBlock, TextBlock.ForegroundProperty, adornedElement, TextBoxBehavior.InfoTextForegroundProperty);
				SetBinding(_infoTextBlock, TextBlock.TextAlignmentProperty, adornedElement, TextBlock.TextAlignmentProperty);
				SetBinding(_infoTextBlock, FrameworkElement.VerticalAlignmentProperty, adornedElement, Control.VerticalContentAlignmentProperty);
				SetBinding(_infoTextBlock, FrameworkElement.HorizontalAlignmentProperty, adornedElement, Control.HorizontalContentAlignmentProperty);
				SetBinding(_infoTextBlock, TextBlock.PaddingProperty, adornedElement, Control.PaddingProperty);

				this.AddVisualChild(_infoTextBlock);
			}

			void AdornedElement_LostFocus(object sender, RoutedEventArgs e)
			{
				if (_isVisible)
					AnimateOpacity(_lostFocusAnimation);
			}

			void AdornedElement_GotFocus(object sender, RoutedEventArgs e)
			{
				if (_isVisible)
					AnimateOpacity(_gotFocusAnimation);
			}

			private static void SetBinding(
				DependencyObject targetObject, DependencyProperty targetProperty,
				DependencyObject sourceObject, DependencyProperty sourceProperty)
			{
				BindingOperations.SetBinding(
					targetObject,
					targetProperty,
					new Binding
					{
						Source = sourceObject,
						Path = new PropertyPath(sourceProperty)
					});
			}

			public void UpdateVisibility(bool visible)
			{
				_isVisible = visible;

				AnimateOpacity(visible ? _showAnimation : _hideAnimation);
			}

			private void AnimateOpacity(DoubleAnimation animation)
			{
				_infoTextBlock.BeginAnimation(UIElement.OpacityProperty, animation);
			}

			public void Dispose()
			{
				if (_disposeAction != null)
					_disposeAction();

				AdornedElement.GotFocus -= AdornedElement_GotFocus;
				AdornedElement.LostFocus -= AdornedElement_LostFocus;
			}

			protected override Size MeasureOverride(Size constraint)
			{
				AdornedElement.Measure(constraint);
				return AdornedElement.RenderSize;
			}

			protected override Size ArrangeOverride(Size finalSize)
			{
				_infoTextBlock.Arrange(new Rect(finalSize));
				return finalSize;
			}

			protected override Visual GetVisualChild(int index)
			{
				if (index == 0)
					return _infoTextBlock;
				else
					ThrowException.ThrowArgumentOutOfRangeException("index");

				return null; // for compilation only
			}

			protected override int VisualChildrenCount
			{
				get
				{
					return 1;
				}
			}
		}
	}
}