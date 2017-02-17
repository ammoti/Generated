// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    [DefaultValue(RandomPattern.All)]
    public enum RandomPattern
    {
        All,
        OnlyLetters,
        OnlyNumbers,
    }

    /// <summary>
    /// Static class for random manipulations.
    /// </summary>
    public static class RandomHelper
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Get random number.
        /// </summary>
        /// 
        /// <param name="minValue">
        /// Min value.
        /// </param>
        /// 
        /// <param name="maxValue">
        /// Max value.
        /// </param>
        /// 
        /// <returns>
        /// The generated number.
        /// </returns>
        public static int GetNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Get random string (without '1', 'I', '0' and 'O' characters).
        /// </summary>
        /// 
        /// <param name="length">
        /// Length of the string.
        /// </param>
        /// 
        /// <param name="randomPattern">
        /// Pattern of the string.
        /// </param>
        /// 
        /// <returns>
        /// The generated string.
        /// </returns>
        public static string GetRandomString(int length, RandomPattern randomPattern = RandomPattern.All)
        {
            if (length < 0) length = 1;

            var letters = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M",
                "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };

            var numbers = new List<int>
            {
                2, 3, 4, 5, 6, 7, 8, 9
            };

            var sbString = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                if (randomPattern == RandomPattern.OnlyLetters)
                {
                    sbString.Append(letters[_random.Next(0, letters.Count - 1)]);
                }
                else if (randomPattern == RandomPattern.OnlyNumbers)
                {
                    sbString.Append(_random.Next(0, numbers.Count - 1));
                }
                else
                {
                    int value = _random.Next(0, numbers.Count - 1);
                    if (value % 2 == 0)
                    {
                        sbString.Append(letters[_random.Next(0, letters.Count - 1)]);
                    }
                    else
                    {
                        sbString.Append(numbers[value].ToString());
                    }
                }
            }

            return sbString.ToString();
        }

        /// <summary>
        /// Get a new random value.
        /// </summary>
        /// 
        /// <param name="type">
        /// Type of the current value.
        /// </param>
        /// 
        /// <param name="currentValue">
        /// Current value.
        /// </param>
        /// 
        /// <returns>
        /// The new value.
        /// </returns>
        public static object GetNewValue(Type type, object currentValue)
        {
            if (type == typeof(string))
            {
                string value = (string)currentValue;
                if (value == null)
                {
                    value = "A";
                }

                return RandomHelper.GetRandomString(value.Length);
            }

            if (type == typeof(int) || type == typeof(int?))
            {
                int? value = (int?)currentValue;
                if (value == null)
                {
                    value = 0;
                }

                if (value.Value == int.MaxValue) return value.Value - 1;
                if (value.Value == int.MinValue) return value.Value + 1;

                return value.Value + 1;
            }

            if (type == typeof(long) || type == typeof(long?))
            {
                long? value = (long?)currentValue;
                if (value == null)
                {
                    value = 0;
                }

                if (value.Value == long.MaxValue) return value.Value - 1;
                if (value.Value == long.MinValue) return value.Value + 1;

                return value.Value + 1;
            }

            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                decimal? value = (decimal?)currentValue;
                if (value == null)
                {
                    value = 0;
                }

                if (value.Value == decimal.MaxValue) return value.Value - 1;
                if (value.Value == decimal.MinValue) return value.Value + 1;

                return value.Value + 1;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                DateTime? value = (DateTime?)currentValue;
                if (value == null)
                {
                    value = DateTime.Now;
                }

                return value.Value.AddMinutes(1);
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                bool? value = (bool?)currentValue;
                if (value == null)
                {
                    value = false;
                }

                return !value.Value;
            }

            if (type == typeof(Guid))
            {
                return DateTime.Now.ToLongTimeString().ToGuid();
            }

            ThrowException.Throw(
                "RandomHelper::GetNewValue, type '{0}' not supported!",
                (currentValue != null) ? currentValue.GetType().FullName : "null");

            return null; // for compilation only
        }
    }
}
