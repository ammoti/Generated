// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Propagates notification that operations should be paused or resumed.
	/// </summary>
	public interface IPauseToken
	{
		/// <summary>
		/// Gets whether pause has been requested for this token.
		/// </summary>
		bool IsPauseRequested { get; }

		/// <summary>
		/// Blocks the current thread (if pause has been requested) until the current System.Threading.WaitHandle receives a resume signal.
		/// </summary>
		void WaitOnPause();
	}
}
