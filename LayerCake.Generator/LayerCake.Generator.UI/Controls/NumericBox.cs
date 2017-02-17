// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Controls
{
	using System;
	using System.Windows;
	using System.Windows.Controls;

	public class NumericBox : TextBox
	{
		public NumericBox()
		{
			this.TextChanged += OnTextChanged;
		}

		public static readonly DependencyProperty DefaultValueProperty =
			DependencyProperty.Register(
				"DefaultValue",
				typeof(int?),
				typeof(NumericBox),
				null);

		public int? DefaultValue
		{
			get { return (int?)GetValue(DefaultValueProperty); }
			set { SetValue(DefaultValueProperty, value); }
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;

			int selectionStart = textBox.SelectionStart;
			int selectionLength = textBox.SelectionLength;

			string newText = string.Empty;

			foreach (char c in textBox.Text.ToCharArray())
			{
				if (char.IsDigit(c))
				{
					newText += c;
				}
			}

			int? defaultValue = (this.DefaultValue != null) ? this.DefaultValue.Value : (int?)null;

			textBox.Text = (!string.IsNullOrEmpty(newText)) ? newText : Convert.ToString(defaultValue);
			textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
		}
	}
}
