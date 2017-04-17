using Moq;
using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Graph;
using TerrainQuest.Generator.Serialization;
using Xunit;

namespace TerrainQuest.Tests.Generator.Serialization
{
    public class SerializedNodeTests
    {
        [Fact]
        public void Constructor_InitializesNode()
        {
            // Arrange
            var node = MockNode();

            // Act
            var actual = new SerializedNode<INode>(node);

            // Assert
            Assert.Equal(node, actual.Node);
        }

        [Fact]
        public void Constructor_InitializesNodeType()
        {
            // Arrange
            var node = MockNode();

            // Act
            var actual = new SerializedNode<INode>(node);

            // Assert
            Assert.Equal(node.GetType(), actual.NodeType);
        }

        [Fact]
        public void Constructor_InitializesHeightMapNode()
        {
            // Arrange
            var node = MockHeightMapNode();

            // Act
            var actual = new SerializedNode(node);

            // Assert
            Assert.Equal(node, actual.Node);
        }

        [Fact]
        public void Constructor_InitializesHeightMapNodeType()
        {
            // Arrange
            var node = MockHeightMapNode();

            // Act
            var actual = new SerializedNode(node);

            // Assert
            Assert.Equal(node.GetType(), actual.NodeType);
        }

        [Fact]
        public void Execute_CallsNodeExecuteMethod()
        {
            // Arrange
            var mock = new Mock<INode>();
            mock.Setup(m => m.Execute());
            var actual = new SerializedNode<INode>(mock.Object);

            // Act
            actual.Execute();

            // Assert
            mock.Verify(m => m.Execute(), Times.Once());
        }

        [Fact]
        public void DeserializeConstructor_InitializesFromSerializationInfo()
        {
            // Arrange
            var node = MockNode();
            var expected = new SerializedNode<INode>(node);
            var info = new SerializationInfo(expected.GetType(), MockFormatterConverter());
            expected.GetObjectData(info, new StreamingContext());

            // Act
            var actual = new SerializedNode<INode>(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.NodeType, actual.NodeType);
            Assert.Equal(expected.Node, actual.Node);
        }

        [Fact]
        public void GetObjectData_SetsEnoughSerializationInfoToDeserialize()
        {
            // Arrange
            var node = MockNode();
            var expected = new SerializedNode<INode>(node);
            var info = new SerializationInfo(expected.GetType(), MockFormatterConverter());

            // Act
            expected.GetObjectData(info, new StreamingContext());

            // Assert
            var actualNode = info.GetValue(nameof(expected.Node), expected.Node.GetType());
            var actualNodeType = (Type)info.GetValue(nameof(expected.NodeType), typeof(Type));
            Assert.Equal(expected.Node, actualNode);
            Assert.Equal(expected.NodeType, actualNodeType);
        }

        private static HeightMapNode MockHeightMapNode()
        {
            var mock = new Mock<HeightMapNode>();
            return mock.Object;
        }

        private static INode MockNode()
        {
            var mock = new Mock<INode>();
            return mock.Object;
        }
        private static IFormatterConverter MockFormatterConverter()
        {
            var mock = new Mock<IFormatterConverter>();
            return mock.Object;
        }
    }
}
