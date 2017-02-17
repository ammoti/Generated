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
	using System.ServiceModel.Description;

	public class ValidateMustUnderstandElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get { return typeof(MustUnderstandBehavior); }
		}

		protected override object CreateBehavior()
		{
			return new MustUnderstandBehavior(false);
		}
	}
}
