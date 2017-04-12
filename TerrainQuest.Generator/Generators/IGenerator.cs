namespace TerrainQuest.Generator.Generators
{
    /// <summary>
    /// Describes a class that can generate <see cref="HeightMap"/>s
    /// </summary>
    public interface IGenerator : IGenerator<HeightMap, double>
    { }

    /// <summary>
    /// Describes a class that can generate maps
    /// </summary>
    public interface IGenerator<TMap, TMapData>
        where TMap : Map<TMapData> where TMapData : struct
    {
        TMap Generate();
    }
}
