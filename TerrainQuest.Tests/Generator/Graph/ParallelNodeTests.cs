using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Graph;
using Xunit;

namespace TerrainQuest.Tests.Generator.Graph
{
    public class ParallelNodeTests
    {
        [Fact]
        public void Process_DependenciesAreExecutedBeforeProcess()
        {
            // Arrange
            var sourceMockA = new Mock<INode>();
            sourceMockA.Setup(m => m.Execute());
            var sourceMockB = new Mock<INode>();
            sourceMockB.Setup(m => m.Execute());

            var node = new TestNode();
            node.Sources.Add(sourceMockA.Object);
            node.Sources.Add(sourceMockB.Object);

            // Act
            node.Execute();

            // Assert
            sourceMockA.Verify(m => m.Execute(), Times.Once());
            sourceMockB.Verify(m => m.Execute(), Times.Once());
        }

        [Fact]
        public void IsDone_ReturnsFalseByDefault()
        {
            // Act
            var node = new TestNode();

            // Assert
            Assert.False(node.IsDone);
        }

        [Fact]
        public void IsDependenciesDone_ReturnsFalseByDefault()
        {
            // Act
            var node = new TestNode();

            // Assert
            Assert.False(node.IsDependenciesDone);
        }

        [Fact]
        public void IsExecuting_ReturnsFalseByDefault()
        {
            // Act
            var node = new TestNode();

            // Assert
            Assert.False(node.IsExecuting);
        }

        [Fact]
        public void IsDone_ReturnsTrueWhenExecuteIsFinished()
        {
            // Arrange
            var node = new TestNode();

            // Act
            node.Execute();

            // Assert
            Assert.True(node.IsDone);
        }

        [Fact]
        public void IsDone_ReturnsFalseWhenProcessIsCalled()
        {
            // Arrange
            var mock = new Mock<ParallelNode>();
            mock.Protected()
                .Setup("Process").Callback(() => {
                    // Assert
                    Assert.False(mock.Object.IsDone);
                });

            // Act
            mock.Object.Execute();
        }

        [Fact]
        public void IsDependenciesDone_ReturnsTrueWhenProcessIsCalled()
        {
            // Arrange
            var mock = new Mock<ParallelNode>();
            mock.Protected()
                .Setup("Process").Callback(() => {
                    // Assert
                    Assert.True(mock.Object.IsDependenciesDone);
                });

            // Act
            mock.Object.Execute();
        }

        [Fact]
        public void IsExecuting_ReturnsTrueWhenDependenciesExecute()
        {
            // Arrange
            var sourceMockA = new Mock<INode>();

            var node = new TestNode();
            node.Sources.Add(sourceMockA.Object);

            sourceMockA.Setup(m => m.Execute()).Callback(() => {
                // Assert
                Assert.True(node.IsExecuting);
            });

            // Act
            node.Execute();
        }

        [Fact]
        public void IsExecuting_ReturnsTrueWhenProcessIsRun()
        {
            // Arrange
            var mock = new Mock<ParallelNode>();
            mock.Protected()
                .Setup("Process").Callback(() => {
                    // Assert
                    Assert.True(mock.Object.IsExecuting);
                });

            // Act
            mock.Object.Execute();
        }

        [Fact]
        public void IsExecuting_ReturnsFalseWhenExecuteIsFinished()
        {
            // Arrange
            var node = new TestNode();

            // Act
            node.Execute();

            // Assert
            Assert.False(node.IsExecuting);
        }

        [Fact]
        public void Dependencies_IsEmptyByDefault()
        {
            // Arrange
            var mock = new Mock<ParallelNode>();

            // Assert
            Assert.NotNull(mock.Object.Dependencies);
            Assert.Empty(mock.Object.Dependencies);
        }

        private class TestNode : ParallelNode
        {
            public List<INode> Sources = new List<INode>();

            public override IEnumerable<INode> Dependencies
            {
                get { return Sources; }
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {}

            protected override void Process()
            { }
        }
    }
}
