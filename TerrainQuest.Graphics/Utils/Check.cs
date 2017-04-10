using System;
using System.Collections.Generic;

namespace TerrainQuest
{
    public class PreconditionException : Exception
    {
        public PreconditionException(string message = null)
            : base(message)
        { }
    }

    public class PostconditionException : Exception
    {
        public PostconditionException(string message = null)
            : base(message)
        { }
    }

    /// <summary>
    /// A design-by-contract inspired Check class, used to quickly add runtime assertions to the code.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Asserts that a required precondition is true.
        /// </summary>
        public static void Requires(bool assertion, string message = null)
        {
            if (!assertion)
                throw new PreconditionException(message ?? "Precondition contract failed.");
        }

        /// <summary>
        /// Asserts whether a postcondition is true
        /// </summary>
        /// <param name="assertion"></param>
        /// <param name="message"></param>
        public static void Ensures(bool assertion, string message = null)
        {
            if (!assertion)
                throw new PostconditionException(message ?? "Postcondition contract failed.");
        }

        /// <summary>
        /// Assert whether an argument is null
        /// </summary>
        public static T NotNull<T>(T value, string parameterName)
        {
            if (!ReferenceEquals(value, null))
                return value;

            NotEmpty(parameterName, "parameterName");
            throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Assert whether a property of an argument is null.
        /// </summary>
        public static T NotNull<T>(T value, string parameterName, string propertyName)
        {
            if (!ReferenceEquals(value, null))
                return value;

            NotEmpty(parameterName, "parameterName");
            NotEmpty(propertyName, "propertyName");
            throw new ArgumentException($"The property '{propertyName}' of the argument '{parameterName}' cannot be null.");
        }

        /// <summary>
        /// Assert whether a string is null or empty
        /// </summary>
        public static string NotEmpty(string value, string parameterName)
        {
            Exception e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException($"The string argument '{parameterName}' cannot be empty.");
            }

            if (e == null)
                return value;

            NotEmpty(parameterName, "parameterName");
            throw e;
        }

        /// <summary>
        /// Assert whether a collection is null or empty
        /// </summary>
        public static ICollection<T> NotEmpty<T>(ICollection<T> value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count != 0)
                return value;

            NotEmpty(parameterName, "parameterName");
            throw new ArgumentException($"The collection argument '{parameterName}' must contain at least one element.");
        }
    }

}
