// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Web;

	public static partial class TranslationHelper
	{
		/// <summary>
		/// Tries to retrieve the user's culture from Items, Session, Cookies or Browser's languages.
		/// </summary>
		public static string ContextualCulture
		{
			get
			{
				string culture = null;

				do
				{
					if (HttpContext.Current != null) // Web context
					{
						#region [ Items ]

						if (HttpContext.Current.Items != null)
						{
							var userContext = HttpContext.Current.Items["UserContext"] as IUserContext;
							if (userContext != null)
							{
								culture = userContext.Culture;

								if (Cultures.IsSupported(culture))
									break;
							}

							culture = HttpContext.Current.Items["UserCulture"] as string;
							if (Cultures.IsSupported(culture))
								break;
						}

						#endregion

						#region [ Session ]

						if (HttpContext.Current.Session != null)
						{
							var userContext = HttpContext.Current.Session["UserContext"] as IUserContext;
							if (userContext != null)
							{
								culture = userContext.Culture;

								if (Cultures.IsSupported(culture))
									break;
							}

							culture = HttpContext.Current.Session["UserCulture"] as string;
							if (Cultures.IsSupported(culture))
								break;
						}

						#endregion

						#region [ Cookies ]

						if (HttpContext.Current.Request != null)
						{
							var cookie = HttpContext.Current.Request.Cookies["UserCulture"];
							if (cookie != null)
							{
								culture = cookie.Value;

								if (Cultures.IsSupported(culture))
									break;
							}
						}

						#endregion

						#region [ Browser ]

						if (!Cultures.IsSupported(culture))
						{
							var cultures = CultureHelper.GetBowserCultures();
							foreach (var ci in cultures)
							{
								if (Cultures.IsSupported(ci.Name))
								{
									culture = ci.Name;
									break;
								}
							}
						}

						#endregion
					}
				}
				while (false);

				if (!Cultures.IsSupported(culture))
				{
					culture = Cultures.Default;
				}

				return culture.ToUpperInvariant();
			}
		}
	}
}
