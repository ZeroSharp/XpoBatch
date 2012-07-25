using System;
using System.Text;

namespace XpoTests
{
    public static class Randomizer
    {
        private static Random _Random = new Random((int)DateTime.Now.Ticks);

        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static int RandomInteger(int size)
        {
            return _Random.Next(size);
        }

        public static T RandomEnum<T>()
        {
            T[] values = (T[]) Enum.GetValues(typeof(T));
            return values[new Random().Next(0,values.Length)];
        }
    }
}
