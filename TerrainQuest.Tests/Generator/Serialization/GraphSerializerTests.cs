using System;
using System.Text;
using TerrainQuest.Generator.Graph;
using TerrainQuest.Generator.Serialization;
using Xunit;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace TerrainQuest.Tests.Generator.Serialization
{
    public class GraphSerializerTests
    {
        

        [Fact]
        public void Serialize_SerializedJsonIsValid()
        {
            // Arrange
            var node = new TestNode(99);
            var expected = new SerializedNode(node);

            string serializedNode;
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                // Act
                GraphSerializer.Serialize(writer, expected, false);
                writer.Flush();
                serializedNode = Encoding.UTF8.GetString(stream.ToArray());
            }

            using (var reader = new StringReader(serializedNode))
            {
                // Assert
                var serializer = new JsonSerializer();
                var actual = (SerializedNode)serializer.Deserialize(reader, typeof(SerializedNode));
                Assert.Equal(expected.NodeType, actual.NodeType);
                Assert.Equal(((TestNode)expected.Node).SomeValue, ((TestNode)actual.Node).SomeValue);
            }
        }

        [Fact]
        public void Serialize_SerializedPrettyJsonIsValid()
        {
            // Arrange
            var node = new TestNode(99);
            var expected = new SerializedNode(node);

            string serializedNode;
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                // Act
                GraphSerializer.Serialize(writer, expected, true);
                writer.Flush();
                serializedNode = Encoding.UTF8.GetString(stream.ToArray());
            }

            using (var reader = new StringReader(serializedNode))
            {
                // Assert
                var serializer = new JsonSerializer();
                var actual = (SerializedNode)serializer.Deserialize(reader, typeof(SerializedNode));
                Assert.Equal(expected.NodeType, actual.NodeType);
                Assert.Equal(((TestNode)expected.Node).SomeValue, ((TestNode)actual.Node).SomeValue);
            }
        }

        [Fact]
        public void Deserialize_CanDeserializeJson()
        {
            // Arrange
            var node = new TestNode(99);
            var expected = new SerializedNode(node);
            var serializedNode = GetSerializeString(expected);

            using (var reader = new StringReader(serializedNode))
            {
                // Act
                var actual = GraphSerializer.Deserialize(reader);

                // Assert
                Assert.Equal(expected.NodeType, actual.NodeType);
                Assert.Equal(((TestNode)expected.Node).SomeValue, ((TestNode)actual.Node).SomeValue);
            }
        }

        [Fact]
        public void Deserialize_CanDeserializePrettyJson()
        {
            // Arrange
            var node = new TestNode(99);
            var expected = new SerializedNode(node);
            var serializedNode = GetSerializeString(expected, true);

            using (var reader = new StringReader(serializedNode))
            {
                // Act
                var actual = GraphSerializer.Deserialize(reader);

                // Assert
                Assert.Equal(expected.NodeType, actual.NodeType);
                Assert.Equal(((TestNode)expected.Node).SomeValue, ((TestNode)actual.Node).SomeValue);
            }
        }

        private static string GetSerializeString(SerializedNode node, bool pretty = false)
        {
            string serializedNode;
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                // Act
                GraphSerializer.Serialize(writer, node, pretty);
                writer.Flush();
                serializedNode = Encoding.UTF8.GetString(stream.ToArray());
            }
            return serializedNode;
        }

        private class TestNode : HeightMapNode
        {
            public int SomeValue { get; private set; }

            public TestNode(int someValue)
            {
                SomeValue = someValue;
            }

            public TestNode(SerializationInfo info, StreamingContext context)
            {
                SomeValue = info.GetInt32(nameof(SomeValue));
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue(nameof(SomeValue), SomeValue);
            }

            protected override void Process()
            {
                throw new NotImplementedException();
            }
        }
    }
}
