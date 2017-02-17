// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Runtime.Serialization
{
	/// <summary>
	/// Generates IDs for objects.
	/// </summary>
	public static class ObjectIDGeneratorExtensions
	{
		/// <summary>
		/// Returns the ID for the specified object, generating a new ID if the specified
		/// object has not already been identified by the System.Runtime.Serialization.ObjectIDGenerator.
		/// </summary>
		///
		/// <param name="source">
		/// ObjectIDGenerator source.
		/// </param>
		///
		/// <param name="obj">
		/// The object you want an ID for.
		/// </param>
		///
		/// <returns>
		/// The object's ID is used for serialization.
		/// </returns>
		public static long GetId(this ObjectIDGenerator source, object obj)
		{
			bool firstTime;
			return source.GetId(obj, out firstTime);
		}

		/// <summary>
		/// Determines whether an object has already been assigned an ID.
		/// </summary>
		///
		/// <param name="source">
		/// ObjectIDGenerator source.
		/// </param>
		///
		/// <param name="obj">
		/// The object you are asking for.
		/// </param>
		/// 
		/// <param name="withForceId">
		/// HasId() does not set the Id by default.
		/// This value indicates whether the Id is set if HasId() returns zero at first time.
		/// </param>
		///
		/// <returns>
		/// The object ID of obj if previously known to the System.Runtime.Serialization.ObjectIDGenerator; otherwise, zero.
		/// </returns>
		public static long HasId(this ObjectIDGenerator source, object obj, bool withForceId = false)
		{
			bool firstTime;
			long id = source.HasId(obj, out firstTime);

			if (id == 0 && withForceId)
			{
				source.GetId(obj);
			}

			return id;
		}
	}
}
