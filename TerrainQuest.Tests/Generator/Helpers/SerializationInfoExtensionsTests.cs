using Moq;
using System;
using System.Runtime.Serialization;
using TerrainQuest.Generator.Helpers;
using Xunit;

namespace TerrainQuest.Tests.Generator.Helpers
{
    public class SerializationInfoExtensionsTests
    {
        [Fact]
        public void AddTypedValue_AddsBothValueAndTypeToInfo()
        {
            // Arrange
            var expectedValue = new TestClass { Value = 1 };
            var info = new SerializationInfo(expectedValue.GetType(), MockFormatterConverter());

            // Act
            info.AddTypedValue(nameof(TestClass), expectedValue);

            // Assert
            Assert.Equal(2, info.MemberCount);
            var actualValue = info.GetValue(nameof(TestClass), typeof(TestClass)) as TestClass;
            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedValue.Value, actualValue.Value);
            var actualType = info.GetValue(
                nameof(TestClass) + SerializationInfoExtensions.TypedValuePostfix, 
                typeof(Type));
            Assert.NotNull(actualType);
            Assert.Equal(typeof(TestClass), actualType);
        }

        [Fact]
        public void AddTypedValue_TypeCanBeUsedToDeserializeTypedValue()
        {
            // Arrange
            var expectedValue = new TestClass { Value = 1 };
            var info = new SerializationInfo(expectedValue.GetType(), MockFormatterConverter());
            info.AddTypedValue(nameof(TestClass), expectedValue);
            var actualType = info.GetValue(
                nameof(TestClass) + SerializationInfoExtensions.TypedValuePostfix,
                typeof(Type)) as Type;

            // Act
            var actualValue = info.GetValue(nameof(TestClass), actualType) as TestClass;

            // Assert
            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedValue.Value, actualValue.Value);
        }

        [Fact]
        public void AddTypedValue_NullNameThrowsArgumentNullException()
        {
            // Arrange
            var info = new SerializationInfo(typeof(TestClass), MockFormatterConverter());

            // Assert
            Assert.Throws<ArgumentNullException>(() => {
                // Act
                info.AddTypedValue(null, null /* Null is perfectly fine here */);
            });
        }

        [Fact]
        public void AddTypedValue_EmptyNameThrowsArgumentException()
        {
            // Arrange
            var info = new SerializationInfo(typeof(TestClass), MockFormatterConverter());

            // Assert
            Assert.Throws<ArgumentException>(() => {
                // Act
                info.AddTypedValue(string.Empty, null /* Null is perfectly fine here */);
            });
        }

        [Fact]
        public void GetTypedValue_ReturnsCorrectValue()
        {
            // Arrange
            var expectedValue = new TestClass { Value = 1 };
            var info = new SerializationInfo(expectedValue.GetType(), MockFormatterConverter());
            info.AddTypedValue(nameof(TestClass), expectedValue);

            // Act
            var actual = info.GetTypedValue<TestClass>(nameof(TestClass));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedValue, actual);
            Assert.Equal(expectedValue.Value, actual.Value);
        }

        [Fact]
        public void GetTypedValue_ReturnsCorrectlyTypedObject()
        {
            // Arrange
            var expectedValue = new TestClass { Value = 1 };
            var info = new SerializationInfo(expectedValue.GetType(), MockFormatterConverter());
            info.AddTypedValue(nameof(TestClass), expectedValue);

            // Act
            var actual = info.GetTypedValue<TestClass>(nameof(TestClass));

            // Assert
            Assert.Equal(expectedValue.GetType(), actual.GetType());
        }

        [Fact]
        public void GetTypedValue_NullNameThrowsArgumentNullException()
        {
            // Arrange
            var info = new SerializationInfo(typeof(TestClass), MockFormatterConverter());

            // Assert
            Assert.Throws<ArgumentNullException>(() => {
                // Act
                info.GetTypedValue<TestClass>(null);
            });
        }

        [Fact]
        public void GetTypedValue_EmptyNameThrowsArgumentException()
        {
            // Arrange
            var info = new SerializationInfo(typeof(TestClass), MockFormatterConverter());

            // Assert
            Assert.Throws<ArgumentException>(() => {
                // Act
                info.GetTypedValue<TestClass>(string.Empty);
            });
        }

        private static IFormatterConverter MockFormatterConverter()
        {
            var mock = new Mock<IFormatterConverter>();
            return mock.Object;
        }

        private class TestClass
        {
            public int Value { get; set; }
        }
    }
}
