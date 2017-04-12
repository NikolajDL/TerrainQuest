using System;

namespace TerrainQuest.Generator.Helpers
{
    /// <summary>
    /// A collection of math helpers methods
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Clamp the value to stay within the given interval.
        /// </summary>
        public static T Clamp<T>(T input, T min, T max) where T : IComparable<T>
        {
            if (input.CompareTo(min) < 0)
                return min;
            if (input.CompareTo(max) > 0)
                return max;

            return input;
        }
    }
}
