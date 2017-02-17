// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Threading.Tasks;

	/// <summary>
	/// CacheService interface.
	/// </summary>
	public interface ICacheService : IDisposable
	{
		#region [ Synchronous Methods ]

		/// <summary>
		/// Adds a CacheItem object to the cache.
		/// If the key already exists, the cache is updated with the new data.
		/// </summary>
		/// 
		/// <param name="cacheItem">
		/// CacheItem object.
		/// </param>
		bool Add(CacheItem cacheItem);

		/// <summary>
		/// Adds a new object to the cache with expiration TimeSpan.
		/// If the key already exists, the cache is updated with the new data.
		/// </summary>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <param name="data">
		/// Object to add to the cache.
		/// </param>
		/// 
		/// <param name="timeSpan">
		/// Expiration TimeSpan.
		/// </param>
		bool Add(string key, object data, TimeSpan timeSpan);

		/// <summary>
		/// Gets an object from the cache given its unique key.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the retrieved object.
		/// </typeparam>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <returns>
		/// The retrieved object. If not found returns null.
		/// </returns>
		T Get<T>(string key) where T : class;

		/// <summary>
		/// Gets an object from the cache given its unique key.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the retrieved object.
		/// </typeparam>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <param name="whenNull">
		/// Function to execute when the key is not present in the cache.
		/// </param>
		/// 
		/// <param name="expiration">
		/// Expiration DateTime.
		/// </param>
		/// 
		/// <returns>
		/// The retrieved object. If not found returns null.
		/// </returns>
		T Get<T>(string key, Func<T> whenNull, DateTime expiration) where T : class;

		/// <summary>
		/// Gets an object from the cache given its unique key.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The type of the retrieved object.
		/// </typeparam>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <param name="whenNull">
		/// Function to execute when the key is not present in the cache.
		/// </param>
		/// 
		/// <param name="expiration">
		/// Expiration TimeSpan.
		/// </param>
		/// 
		/// <returns>
		/// The retrieved object. If not found returns null.
		/// </returns>
		T Get<T>(string key, Func<T> whenNull, TimeSpan expiration) where T : class;

		/// <summary>
		/// Removes an object from the cache given its unique key.
		/// </summary>
		/// 
		/// <param name="key">
		/// Unique key.
		/// </param>
		/// 
		/// <returns>
		/// True if the object has been removed from the cache; otherwise, false.
		/// </returns>
		bool Remove(string key);

		/// <summary>
		/// Removes all the objects having the given tag value from the cache.
		/// </summary>
		/// 
		/// <param name="tag">
		/// Tag value.
		/// </param>
		/// 
		/// <returns>
		/// The number of objects removed.
		/// </returns>
		int RemoveByTag(string tag);

		#endregion

		#region [ Asynchronous Methods ]

		Task<bool> AddAsync(CacheItem cacheItem);

		Task<bool> AddAsync(string key, object data, TimeSpan timeSpan);

		Task<T> GetAsync<T>(string key) where T : class;

		Task<T> GetAsync<T>(string key, Func<T> whenNull, DateTime expiration) where T : class;

		Task<T> GetAsync<T>(string key, Func<T> whenNull, TimeSpan expiration) where T : class;

		Task<bool> RemoveAsync(string key);

		Task<int> RemoveByTagAsync(string key);

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the cache statistics.
		/// </summary>
		ICacheStatistics Statistics { get; }

		#endregion
	}
}
