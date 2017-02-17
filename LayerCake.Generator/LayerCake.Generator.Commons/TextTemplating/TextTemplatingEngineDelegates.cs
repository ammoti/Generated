// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{

	/// <summary>
	/// When the TextTemplatingEngine starts to process a T4 Template.
	/// </summary>
	/// 
	/// <param name="sender">
	/// Sender.
	/// </param>
	/// 
	/// <param name="e">
	/// EventArgs.
	/// </param>
	public delegate void TextTemplatingEngineProcessingDelegate(object sender, TextTemplatingEngineTemplateEventArgs e);

	/// <summary>
	/// When the TextTemplatingEngine has succesfully processed the T4 Template.
	/// </summary>
	/// 
	/// <param name="sender">
	/// Sender.
	/// </param>
	/// 
	/// <param name="e">
	/// EventArgs.
	/// </param>
	public delegate void TextTemplatingEngineProcessedDelegate(object sender, TextTemplatingEngineTemplateEventArgs e);

	/// <summary>
	/// When the TextTemplatingEngine does not process the T4 Template.
	/// </summary>
	/// 
	/// <param name="sender">
	/// Sender.
	/// </param>
	/// 
	/// <param name="e">
	/// EventArgs.
	/// </param>
	public delegate void TextTemplatingEngineNotProcessedDelegate(object sender, TextTemplatingEngineTemplateEventArgs e);

	/// <summary>
	/// When the TextTemplatingEngine failed to process the T4 Template.
	/// </summary>
	/// 
	/// <param name="sender">
	/// Sender.
	/// </param>
	/// 
	/// <param name="e">
	/// EventArgs.
	/// </param>
	public delegate void TextTemplatingEngineProcessFailedDelegate(object sender, TextTemplatingEngineTemplateEventArgs e);

	/// <summary>
	/// When the TextTemplatingEngine has generated the output file.
	/// </summary>
	/// 
	/// <param name="sender">
	/// Sender.
	/// </param>
	/// 
	/// <param name="e">
	/// EventArgs.
	/// </param>
	public delegate void TextTemplatingEngineGeneratedFileDelegate(object sender, TextTemplatingEngineFileGeneratedEventArgs e);
}
