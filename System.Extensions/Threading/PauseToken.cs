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
	public class PauseToken : IPauseToken
	{
		#region [ Members ]

		private PauseTokenSource _source = null;

		#endregion

		#region [ Constructor ]

		/// <summary>
		/// Initializes the Token.
		/// </summary>
		/// 
		/// <param name="source">
		/// The PauseTokenSource.
		/// </param>
		public PauseToken(PauseTokenSource source)
		{
			_source = source;
		}

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Returns an empty Token value.
		/// </summary>
		public static PauseToken None
		{
			get { return default(PauseToken); }
		}

		#endregion

		#region [ IPauseToken Implementation ]

		/// <summary>
		/// Gets whether pause has been requested for this token.
		/// </summary>
		public bool IsPauseRequested
		{
			get { return _source != null && _source.IsPauseRequested; }
		}

		/// <summary>
		/// Blocks the current thread (if pause has been requested) until the current System.Threading.WaitHandle receives a resume signal.
		/// </summary>
		public void WaitOnPause()
		{
			if (_source != null)
			{
				_source.WaitOnPause();
			}
		}

		#endregion
	}
}
