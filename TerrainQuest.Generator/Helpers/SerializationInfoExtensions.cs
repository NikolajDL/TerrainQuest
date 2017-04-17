using System;
using System.Runtime.Serialization;

namespace TerrainQuest.Generator.Helpers
{
    /// <summary>
    /// Serialization helper extensions
    /// </summary>
    public static class SerializationInfoExtensions
    {
        /// <summary>
        /// The postfix added to the value-name for the type information of typed values.
        /// </summary>
        public const string TypedValuePostfix = "__Type";

        /// <summary>
        /// Add a value for serialization that also includes the type of the value, 
        /// in order to correctly deserialize the actual runtime type. 
        /// </summary>
        public static void AddTypedValue(this SerializationInfo info, string name, object value)
        {
            Check.NotEmpty(name, nameof(name));

            info.AddValue(name, value, value.GetType());
            info.AddValue(name + TypedValuePostfix, value.GetType(), typeof(Type));
        }

        /// <summary>
        /// Deserialize a value that has been added with a specific runtime type,
        /// but the type is unknown at compile type. 
        /// </summary>
        public static T GetTypedValue<T>(this SerializationInfo info, string name)
        {
            Check.NotEmpty(name, nameof(name));

            var type = (Type)info.GetValue(name + TypedValuePostfix, typeof(Type));
            return (T)info.GetValue(name, type);
        }

    }
}
