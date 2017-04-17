using Newtonsoft.Json;
using System.IO;

namespace TerrainQuest.Generator.Serialization
{
    /// <summary>
    /// A static class used to serialize and deserialize a terrain generation graph structure
    /// represented by a <see cref="SerializedNode"/> object.
    /// </summary>
    public static class GraphSerializer
    {

        /// <summary>
        /// Serialize a terrain generation <see cref="SerializedNode"/> as a JSON encoded file to the given stream.
        /// </summary>
        /// <param name="writer">Stream to write the serialized data to</param>
        /// <param name="graph">Graph to serialize</param>
        /// <param name="prettyJson">
        /// Should format the JSON with identation and newline characters for better readability
        /// </param>
        public static void Serialize(StreamWriter writer, SerializedNode graph, bool prettyJson = false)
        {
            var serializer = new JsonSerializer();
            if(prettyJson)
                serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.Serialize(writer, graph);
        }

        /// <summary>
        /// Serialize a terrain generation <see cref="SerializedNode"/> as a JSON encoded file
        /// and saves it as the given filename. 
        /// If the given file already exists, it'll be overridden, otherwise a new file is created.
        /// </summary>
        /// <param name="filename">Path and filename of the file to serialize to.</param>
        /// <param name="graph">Graph to serialize</param>
        /// <param name="prettyJson">
        /// Should format the JSON with identation and newline characters for better readability
        /// </param>
        public static void Serialize(string filename, SerializedNode graph, bool prettyJson = false)
        {
            using (var writer = new StreamWriter(filename, false))
            {
                Serialize(writer, graph, prettyJson);
            }
        }

        /// <summary>
        /// Deserializes a JSON encoded file to a terrain generation <see cref="SerializedNode"/>
        /// </summary>
        /// <param name="reader">Stream containing the JSON file to serialize</param>
        /// <returns>A terrain generation graph structure represented by a <see cref="SerializedNode"/></returns>
        public static SerializedNode Deserialize(TextReader reader)
        {
            var serializer = new JsonSerializer();
            return (SerializedNode)serializer.Deserialize(reader, typeof(SerializedNode));
        }

        /// <summary>
        /// Deserializes a JSON encoded file to a terrain generation <see cref="SerializedNode"/>
        /// </summary>
        /// <param name="filename">Path and name of the JSON file to deserialize</param>
        /// <returns>A terrain generation graph structure represented by a <see cref="SerializedNode"/></returns>
        public static SerializedNode Deserialize(string filename)
        {
            using (var reader = new StreamReader(filename, false))
            {
                return Deserialize(reader);
            }
        }
    }
}
