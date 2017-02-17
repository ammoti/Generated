// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using System.Security.Permissions;

	[Serializable]
	public class EntityValidationException : Exception, ISerializable
	{
		#region [ Constructors ]

		public EntityValidationException()
			: base()
		{
			this.Translations = new List<TranslationEnum>();
		}

		public EntityValidationException(TranslationEnum translation)
			: base(ToMessage(translation))
		{
			this.Translations = new List<TranslationEnum>();

			this.Translations.Add(translation);
		}

		public EntityValidationException(IList<TranslationEnum> translations)
			: base(ToMessage(translations))
		{
			this.Translations = translations;
		}

		protected EntityValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Translations = (IList<TranslationEnum>)info.GetValue("Translations", typeof(IList<TranslationEnum>));
		}

		#endregion

		#region [ Properties ]

		public IList<TranslationEnum> Translations { get; set; }

		#endregion

		#region [ ISerializable Implementation ]

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("Translations", this.Translations);
		}

		#endregion

		#region [ Helpers ]

		private static string ToMessage(TranslationEnum translation)
		{
			return translation.ToString();
		}

		private static string ToMessage(IEnumerable<TranslationEnum> translations)
		{
			return string.Join(", ", translations);
		}

		#endregion
	}
}
