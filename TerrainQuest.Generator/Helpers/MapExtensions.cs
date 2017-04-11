using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainQuest.Generator.Helpers
{
    /// <summary>
    /// Extension methods for the <see cref="Map"/> class.
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// Run the given predicate for each data point in the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="action">
        ///     The predicate to run for each data point. 
        ///     Passes (row, column, index) as arguments.
        /// </param>
        public static void ForEach<TData>(this Map<TData> map, Action<int, int, int> action)
        {
            for (int r = 0; r < map.Height; r++)
                for (int c = 0; c < map.Width; c++)
                {
                    action(r, c, r * map.Width + c);
                }
        }

        /// <summary>
        /// Run the given predicate for each data point in the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="action">
        ///     The predicate to run for each data point. 
        ///     Passes (row, column, index) as arguments.
        /// </param>
        public static void ForEach<TData>(this Map<TData> map, Action<int, int> action)
            => ForEach(map, (r, c, i) => action(r, c));
    }
}
