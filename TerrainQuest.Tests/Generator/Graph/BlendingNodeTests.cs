using Moq;
using System;
using System.Linq;
using System.Runtime.Serialization;
using TerrainQuest.Generator;
using TerrainQuest.Generator.Blending;
using TerrainQuest.Generator.Graph;
using TerrainQuest.Generator.Helpers;
using Xunit;

namespace TerrainQuest.Tests.Generator.Graph
{
    public class BlendingNodeTests
    {
        [Fact]
        public void Constructor_InitializesBlendmodeProperty()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;

            // Act
            var actual = new BlendingNode(blendMode);

            // Assert
            Assert.Equal(blendMode, actual.BlendMode);
            Assert.False(actual.Dimensions.HasValue);
        }

        [Fact]
        public void Constructor_InitializesDimensionsAndBlendmodeProperty()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;

            // Act
            var actual = new BlendingNode(20, 10, blendMode);

            // Assert
            Assert.Equal(blendMode, actual.BlendMode);
            Assert.Equal(20, actual.Dimensions.Value.Height);
            Assert.Equal(10, actual.Dimensions.Value.Width);
        }

        [Fact]
        public void Constructor_MissingBlendModeThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                var actual = new BlendingNode(null);
            });
        }

        [Fact]
        public void Constructor_GivenDimensionsButMissingBlendModeThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                var actual = new BlendingNode(20, 10, null);
            });
        }

        [Fact]
        public void AddDependency_CanAddANodeDependency()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;
            var expected = new Mock<HeightMapNode>().Object;
            var node = new BlendingNode(blendMode);

            // Act
            node.AddDependency(expected);

            // Assert
            Assert.Equal(1, node.Dependencies.Count());
            Assert.Equal(expected, node.Dependencies.Single());
        }

        [Fact]
        public void AddDependency_CanAddMultipleNodeDependencies()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;
            var expectedA = new Mock<HeightMapNode>().Object;
            var expectedB = new Mock<HeightMapNode>().Object;
            var node = new BlendingNode(blendMode);

            // Act
            node.AddDependency(expectedA);
            node.AddDependency(expectedB);

            // Assert
            Assert.Equal(2, node.Dependencies.Count());
            Assert.Equal(expectedA, node.Dependencies.First());
            Assert.Equal(expectedB, node.Dependencies.Skip(1).First());
        }

        [Fact]
        public void AddDependency_NullNodeThrowsArgumentNullException()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;
            var node = new BlendingNode(blendMode);
            
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                node.AddDependency(null);
            });
        }

        [Fact]
        public void ISerializable_SerializeAndDeserializeCorrectly()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;
            var source = new Mock<HeightMapNode>().Object;
            var expected = new BlendingNode(blendMode);
            expected.AddDependency(source);
            var info = MockSerializationInfo<BlendingNode>();

            // Act
            expected.GetObjectData(info, new StreamingContext());
            var actual = new BlendingNode(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.Dependencies.Count(), actual.Dependencies.Count());
            Assert.Equal(expected.Dependencies.Single(), actual.Dependencies.Single());
            Assert.Equal(expected.BlendMode, actual.BlendMode);
            Assert.Equal(expected.Dimensions, actual.Dimensions);
        }

        [Fact]
        public void ISerializable_SerializeAndDeserializeWithDimensionsCorrectly()
        {
            // Arrange
            var blendMode = new Mock<IBlendMode>().Object;
            var source = new Mock<HeightMapNode>().Object;
            var expected = new BlendingNode(20, 10, blendMode);
            expected.AddDependency(source);
            var info = MockSerializationInfo<BlendingNode>();

            // Act
            expected.GetObjectData(info, new StreamingContext());
            var actual = new BlendingNode(info, new StreamingContext());

            // Assert
            Assert.Equal(expected.Dependencies.Count(), actual.Dependencies.Count());
            Assert.Equal(expected.Dependencies.Single(), actual.Dependencies.Single());
            Assert.Equal(expected.BlendMode, actual.BlendMode);
            Assert.Equal(expected.Dimensions, actual.Dimensions);
        }

        [Fact]
        public void Execute_RunsBlendOnAllPairsOfDependencies()
        {
            // Arrange
            var blendModeMock = new Mock<IBlendMode>();
            blendModeMock.Setup(b => b.Blend(It.IsAny<HeightMap>(), It.IsAny<HeightMap>()))
                .Returns<HeightMap,HeightMap>((h1,h2) => { return h2; });
            var heightMapA = new HeightMap(10);
            var heightMapB = new HeightMap(10);
            var heightMapC = new HeightMap(10);
            var expectedAMock = new Mock<HeightMapNode>();
            expectedAMock.SetupGet(m => m.Result).Returns(heightMapA);
            var expectedBMock = new Mock<HeightMapNode>();
            expectedBMock.SetupGet(m => m.Result).Returns(heightMapB);
            var expectedCMock = new Mock<HeightMapNode>();
            expectedCMock.SetupGet(m => m.Result).Returns(heightMapC);
            var node = new BlendingNode(blendModeMock.Object);
            node.AddDependency(expectedAMock.Object);
            node.AddDependency(expectedBMock.Object);
            node.AddDependency(expectedCMock.Object);

            // Act
            node.Execute();

            // Assert
            blendModeMock.Verify(m => m.Blend(
                It.IsAny<HeightMap>(), 
                It.Is<HeightMap>(h => h.Equals(heightMapB))), Times.Once);
            blendModeMock.Verify(m => m.Blend(
                It.Is<HeightMap>(h => h.Equals(heightMapB)),
                It.Is<HeightMap>(h => h.Equals(heightMapC))), Times.Once);
        }

        [Fact]
        public void Execute_GivenSourcesAreBlended()
        {
            // Arrange
            var data = new double[,] { { 0d, 0.25d, 0.5d, 0.75d, 1d } };
            var sourceMockA = new Mock<HeightMapNode>();
            sourceMockA.SetupGet(h => h.Result).Returns(new HeightMap(data));
            var sourceMockB = new Mock<HeightMapNode>();
            sourceMockB.SetupGet(h => h.Result).Returns(new HeightMap(data));
            var blendModeMock = new Mock<IBlendMode>();
            blendModeMock.Setup(b => b.Blend(It.IsAny<HeightMap>(), It.IsAny<HeightMap>()))
                .Returns<HeightMap, HeightMap>(MockBlendMode);
            var expected = MockBlendMode(sourceMockA.Object.Result, 
                sourceMockB.Object.Result).Data;
            var node = new BlendingNode(blendModeMock.Object);
            node.AddDependency(sourceMockA.Object);
            node.AddDependency(sourceMockB.Object);

            // Act
            node.Execute();
            var actual = node.Result.Data;

            // Assert
            Assert.Equal(expected[0, 0], actual[0, 0]);
            Assert.Equal(expected[0, 1], actual[0, 1]);
            Assert.Equal(expected[0, 2], actual[0, 2]);
            Assert.Equal(expected[0, 3], actual[0, 3]);
            Assert.Equal(expected[0, 4], actual[0, 4]);
        }

        private static HeightMap MockBlendMode(HeightMap a, HeightMap b)
        {
            var result = a.Clone();
            result.ForEach((r, c) => {
                result[r, c] += b.IsInRange(r, c) ? b[r, c] : 0;
            });
            return result;
        }

        private static SerializationInfo MockSerializationInfo<T>()
        {
            var formatterConverter = new Mock<IFormatterConverter>().Object;
            return new SerializationInfo(typeof(T), formatterConverter);
        }
    }
}
