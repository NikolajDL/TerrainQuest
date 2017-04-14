namespace TerrainQuest.Generator.Blending
{
    /// <summary>
    /// A <see cref="IBlendMode"/> factory class
    /// </summary>
    public static class BlendModes
    {
        /// <summary>
        /// Add blend mode
        /// </summary>
        public static readonly AddBlend Add = new AddBlend();

        /// <summary>
        /// Subtract blend mode
        /// </summary>
        public static readonly SubtractBlend Subtract = new SubtractBlend();

        /// <summary>
        /// Multiply blend mode
        /// </summary>
        public static readonly MultiplyBlend Multiply = new MultiplyBlend();

        /// <summary>
        /// Screen blend mode
        /// </summary>
        public static readonly ScreenBlend Screen = new ScreenBlend();

        /// <summary>
        /// Darken blend mode
        /// </summary>
        public static readonly DarkenBlend Darken = new DarkenBlend();

        /// <summary>
        /// Lighten blend mode
        /// </summary>
        public static readonly LightenBlend Lighten = new LightenBlend();

        /// <summary>
        /// Overlay blend mode
        /// </summary>
        public static readonly OverlayBlend Overlay = new OverlayBlend();

        /// <summary>
        /// Difference blend mode
        /// </summary>
        public static readonly DifferenceBlend Difference = new DifferenceBlend();
    }
}
