// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.ClientCore
{
	using System;

	using VahapYigit.Test.Models;
	using VahapYigit.Test.Contracts;
	using VahapYigit.Test.Core;

	public class TranslationResolver : Singleton<TranslationResolver>
	{
		#region [ Members ]

		private static ICacheService _innerCacheService = null;
		private static readonly object _innerLocker = new object();

		#endregion

		#region [ Constructors ]

		static TranslationResolver()
		{
			CreateCache();
		}

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Retrieves the translation instance from the inner cache if the entry exists.
		/// If not, use the ITranslationService service to retrieve it from the database (and stores it into the inner cache).
		/// </summary>
		/// 
		/// <param name="translationKey">
		/// Translation key.
		/// </param>
		/// 
		/// <returns>
		/// Translation object.
		/// </returns>
		public Translation Resolve(TranslationEnum translationKey)
		{
			if (translationKey == TranslationEnum.None) // special case
			{
				return Translation.None;
			}

			string key = translationKey.ToString();

			var translation = this.GetFromCache(key);
			if (translation == null)
			{
				lock (_innerLocker)
				{
					translation = this.GetFromCache(key);
					if (translation == null)
					{
						using (var service = new ServiceProxy<ITranslationService>())
						{
							translation = service.Proxy.Resolve(ClientContext.Anonymous, translationKey);
						}

						if (translation != null)
						{
							_innerCacheService.Add(new CacheItem { Key = key, Data = translation });
						}
					}
				}
			}

			return translation;
		}

		/// <summary>
		/// Retrieves the translation instance from the inner cache if the entry exists.
		/// If not, use the ITranslationService service to retrieve it from the database (and stores it into the inner cache).
		/// </summary>
		/// 
		/// <param name="translationKey">
		/// Translation key.
		/// </param>
		/// 
		/// <returns>
		/// Translation object.
		/// </returns>
		public Translation Resolve(string translationKey)
		{
			if (translationKey == null)
			{
				ThrowException.ThrowArgumentNullException("translationKey");
			}

			TranslationEnum translationEnum;
			if (!Enum.TryParse<TranslationEnum>(translationKey, out translationEnum))
			{
				ThrowException.ThrowArgumentException(string.Format(
					"The translationKey = '{0}' parameter value cannot be converted to TranslationEnum",
					translationKey));
			}

			return this.Resolve(translationEnum);
		}

		/// <summary>
		/// Retrieves the message translation from the inner cache if the entry exists.
		/// If not, use the ITranslationService service to retrieve it from the database (and stores it into the inner cache).
		/// </summary>
		/// 
		/// <param name="translationKey">
		/// Translation key.
		/// </param>
		/// 
		/// <param name="culture">
		/// Culture (VahapYigit.Test.Core.Cultures class).
		/// </param>
		/// 
		/// <param name="args">
		/// Optional arguments to format the message.
		/// </param>
		/// 
		/// <returns>
		/// The message.
		/// </returns>
		public string GetMessage(TranslationEnum translationKey, string culture, params object[] args)
		{
			string message = "N.C.";

			if (!Cultures.IsSupported(culture))
			{
				culture = Cultures.Default;
			}

			var translation = this.Resolve(translationKey);
			if (translation != null)
			{
				message = translation.Value[culture];

				if (!args.IsNullOrEmpty() && message != null)
				{
					message = string.Format(message, args);
				}
			}

			return message;
		}

		/// <summary>
		/// Retrieves the message translation from the inner cache if the entry exists.
		/// If not, use the ITranslationService service to retrieve it from the database (and stores it into the inner cache).
		/// </summary>
		/// 
		/// <param name="translationKey">
		/// Translation key.
		/// </param>
		/// 
		/// <param name="culture">
		/// Culture (VahapYigit.Test.Core.Cultures class).
		/// </param>
		/// 
		/// <param name="args">
		/// Optional arguments to format the message.
		/// </param>
		/// 
		/// <returns>
		/// The message.
		/// </returns>
		public string GetMessage(string translationKey, string culture, params object[] args)
		{
			if (string.IsNullOrEmpty(translationKey))
			{
				ThrowException.ThrowArgumentNullException("translationKey");
			}

			TranslationEnum translationEnum;
			if (!Enum.TryParse<TranslationEnum>(translationKey, out translationEnum))
			{
				ThrowException.ThrowArgumentException(string.Format(
					"The translationKey = '{0}' parameter value cannot be converted to TranslationEnum",
					translationKey));
			}

			return this.GetMessage(translationEnum, culture, args);
		}

		/// <summary>
		/// Retrieves the message translation from the inner cache if the entry exists.
		/// If not, use the ITranslationService service to retrieve it from the database (and stores it into the inner cache).
		/// </summary>
		/// 
		/// <param name="translationKey">
		/// Translation key.
		/// </param>
		/// 
		/// <param name="args">
		/// Optional arguments to format the message.
		/// </param>
		/// 
		/// <returns>
		/// The message.
		/// </returns>
		public string GetMessage(TranslationEnum translationKey, params object[] args)
		{
			return this.GetMessage(translationKey, TranslationHelper.ContextualCulture, args);
		}

		/// <summary>
		/// Retrieves the message translation from the inner cache if the entry exists.
		/// If not, use the ITranslationService service to retrieve it from the database (and stores it into the inner cache).
		/// </summary>
		/// 
		/// <param name="translationKey">
		/// Translation key.
		/// </param>
		/// 
		/// <param name="args">
		/// Optional arguments to format the message.
		/// </param>
		/// 
		/// <returns>
		/// The message.
		/// </returns>
		public string GetMessage(string translationKey, params object[] args)
		{
			return this.GetMessage(translationKey, TranslationHelper.ContextualCulture, args);
		}

		#endregion

		#region [ Private Methods ]

		/// <summary>
		/// Retrieves the translation instance from the inner cache if the entry exists.
		/// </summary>
		/// 
		/// <param name="key">
		/// Cache key.
		/// </param>
		/// 
		/// <returns>
		/// The translation if exists; otherwise, null.
		/// </returns>
		private Translation GetFromCache(string key)
		{
			return _innerCacheService.Get<Translation>(key);
		}

		/// <summary>
		/// Remove all the Translations instances from the inner cache.
		/// </summary>
		public void PurgeTranslationCache()
		{
			ReleaseCache();
			CreateCache();
		}

		/// <summary>
		/// Creates the inner cache.
		/// </summary>
		private static void CreateCache()
		{
			ReleaseCache();

			if (_innerCacheService == null)
			{
				lock (_innerLocker)
				{
					if (_innerCacheService == null)
					{
						_innerCacheService = new LocalMemoryCacheService(); // inner cache
					}
				}
			}
		}

		/// <summary>
		/// Releases the inner cache from memory.
		/// </summary>
		private static void ReleaseCache()
		{
			if (_innerCacheService != null)
			{
				lock (_innerLocker)
				{
					if (_innerCacheService != null)
					{
						_innerCacheService.SafeDispose();
						_innerCacheService = null;
					}
				}
			}
		}

		#endregion
	}
}
