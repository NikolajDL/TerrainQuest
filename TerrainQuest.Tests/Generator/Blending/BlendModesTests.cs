using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Blending;
using Xunit;

namespace TerrainQuest.Tests.Generator.Blending
{
    public class BlendModesTests
    {
        [Fact]
        public void Add_CreatesAddBlendEffect()
        {
            // Act
            var actual = BlendModes.Add;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<AddBlend>(actual);
        }

        [Fact]
        public void Subtract_CreatesSubtractBlendEffect()
        {
            // Act
            var actual = BlendModes.Subtract;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<SubtractBlend>(actual);
        }

        [Fact]
        public void Multiply_CreatesMultiplyBlendEffect()
        {
            // Act
            var actual = BlendModes.Multiply;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<MultiplyBlend>(actual);
        }

        [Fact]
        public void Screen_CreatesScreenBlendEffect()
        {
            // Act
            var actual = BlendModes.Screen;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<ScreenBlend>(actual);
        }

        [Fact]
        public void Darken_CreatesDarkenBlendEffect()
        {
            // Act
            var actual = BlendModes.Darken;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<DarkenBlend>(actual);
        }

        [Fact]
        public void Lighten_CreatesLightenBlendEffect()
        {
            // Act
            var actual = BlendModes.Lighten;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<LightenBlend>(actual);
        }

        [Fact]
        public void Overlay_CreatesOverlayBlendEffect()
        {
            // Act
            var actual = BlendModes.Overlay;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<OverlayBlend>(actual);
        }

        [Fact]
        public void Difference_CreatesDifferenceBlendEffect()
        {
            // Act
            var actual = BlendModes.Difference;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<DifferenceBlend>(actual);
        }

        [Fact]
        public void Mask_CreatesMaskBlendEffect()
        {
            // Act
            var actual = BlendModes.Mask;

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<MaskBlend>(actual);
        }
    }
}
