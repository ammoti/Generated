// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System;
	using System.ServiceModel.Configuration;

	public class WcfMessageDumpElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get
			{
				return typeof(WcfMessageDumpInspector);
			}
		}

		protected override object CreateBehavior()
		{
			return new WcfMessageDumpInspector();
		}
	}
}
