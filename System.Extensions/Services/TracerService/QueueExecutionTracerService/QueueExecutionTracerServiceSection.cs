// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Xml;

	/// <summary>
	/// QueueExecutionTracerServiceSection class.
	/// </summary>
	public class QueueExecutionTracerServiceSection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public QueueExecutionTracerServiceSection()
		{
		}

		/// <summary>
		/// Loads the queueExecutionTracerServiceConfiguration Xml section.
		/// </summary>
		/// 
		/// <param name="sectionNode">
		/// The queueExecutionTracerServiceConfiguration root node.
		/// </param>
		internal void Load(XmlNode sectionNode)
		{
			if (sectionNode.Attributes["isEnabled"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute on queueExecutionTracerServiceConfiguration section");
			}

			if (sectionNode.Attributes["withDebugTrace"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'withDebugTrace' attribute on queueExecutionTracerServiceConfiguration section");
			}

			if (sectionNode.Attributes["maxItems"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'maxItems' attribute on queueExecutionTracerServiceConfiguration section");
			}

			if (sectionNode.Attributes["rollTimer"] == null)
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'rollTimer' attribute on queueExecutionTracerServiceConfiguration section");
			}

			string sIsEnabled = sectionNode.Attributes["isEnabled"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'isEnabled' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			string sWithDebugTrace = sectionNode.Attributes["withDebugTrace"].Value;
			if (string.IsNullOrEmpty(sIsEnabled))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'withDebugTrace' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			string sMaxItems = sectionNode.Attributes["maxItems"].Value;
			if (string.IsNullOrEmpty(sMaxItems))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'maxItems' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			string sRollTimer = sectionNode.Attributes["rollTimer"].Value;
			if (string.IsNullOrEmpty(sRollTimer))
			{
				ThrowException.ThrowConfigurationErrorsException(
					"Cannot find the 'rollTimer' attribute value on queueExecutionTracerServiceConfiguration section");
			}

			this.IsEnabled = Convert.ToBoolean(sIsEnabled);

			this.MaxItems = Convert.ToInt32(sMaxItems);

			if (this.MaxItems < 1)
				this.MaxItems = 1024;

			this.RollTimer = Convert.ToInt32(sRollTimer);

			if (this.RollTimer < 1)
				this.RollTimer = 1;

#if DEBUG
			this.WithDebugTrace = Convert.ToBoolean(sWithDebugTrace);
#else
			this.WithDebugTrace = false;
#endif
		}

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the value indicating whether the tracer is enabled.
		/// </summary>
		public bool IsEnabled
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating whether the DEBUG tracer is disabled.
		/// In RELEASE mode the WithDebugTrace value is always false.
		/// </summary>
		public bool WithDebugTrace
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value of the limit of the queue.
		/// </summary>
		public int MaxItems
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the limit value of the timer of the queue.
		/// </summary>
		public int RollTimer
		{
			get;
			set;
		}

		#endregion
	}
}
