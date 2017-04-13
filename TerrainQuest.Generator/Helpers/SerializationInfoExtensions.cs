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
        /// Add a value for serialization that also includes the type of the value, 
        /// in order to correctly deserialize the actual runtime type. 
        /// </summary>
        public static void AddTypedValue(this SerializationInfo info, string name, object value)
        {
            info.AddValue(name, value, value.GetType());
            info.AddValue(name + "__Type", value.GetType(), typeof(Type));
        }

        /// <summary>
        /// Deserialize a value that has been added with a specific runtime type,
        /// but the type is unknown at compile type. 
        /// </summary>
        public static T GetTypedValue<T>(this SerializationInfo info, string name)
        {
            var type = (Type)info.GetValue(name + "__Type", typeof(Type));
            return (T)info.GetValue(name, type);
        }

    }
}
