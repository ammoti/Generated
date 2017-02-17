// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.ServiceModel.Configuration;

	public class TechnicalExceptionElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get
			{
				return typeof(TechnicalExceptionBehaviorAttribute);
			}
		}

		protected override object CreateBehavior()
		{
			return new TechnicalExceptionBehaviorAttribute();
		}
	}
}
