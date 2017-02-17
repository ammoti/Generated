// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

// Based on https://raw.githubusercontent.com/Burtsev-Alexey/net-object-deep-copy/master/ObjectExtensions.cs

namespace System
{
	public static class ArrayExtensions
	{
		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if (array.LongLength == 0)
			{
				return;
			}

			var walker = new ArrayTraverse(array);

			do
			{
				action(array, walker.Position);
			}
			while (walker.Step());
		}
	}

	internal class ArrayTraverse
	{
		#region [ Members ]

		private int[] _maxLengths;

		#endregion

		#region [ Properties ]

		public int[] Position;

		#endregion

		#region [ Constructors ]

		public ArrayTraverse(Array array)
		{
			_maxLengths = new int[array.Rank];

			for (int i = 0; i < array.Rank; ++i)
			{
				_maxLengths[i] = array.GetLength(i) - 1;
			}

			this.Position = new int[array.Rank];
		}

		#endregion

		#region [ Methods ]

		public bool Step()
		{
			for (int i = 0; i < this.Position.Length; ++i)
			{
				if (this.Position[i] < _maxLengths[i])
				{
					this.Position[i]++;

					for (int j = 0; j < i; j++)
					{
						this.Position[j] = 0;
					}

					return true;
				}
			}

			return false;
		}

		#endregion
	}
}
