// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Defines methods to release allocated resources.
	/// </summary>
	public static class DisposableExtensions
	{
		/// <summary>
		/// Performs application-defined tasks associated with safe freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// 
		/// <param name="disposable">
		/// Object that implements IDisposable.
		/// </param>
		public static void SafeDispose(this IDisposable disposable)
		{
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
