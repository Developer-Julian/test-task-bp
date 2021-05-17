using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace TestTaskBP.Provider.Utility
{
    internal static class Guard
    {
        [DebuggerStepThrough]
        public static void ArgumentNotNull(object argumentValue, string argumentName, string message = null)
        {
            if (argumentValue == null)
            {
                if (message == null)
                {
                    throw new ArgumentNullException(argumentName);
                }

                throw new ArgumentNullException(argumentName, message);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty<TArg>(
            IEnumerable<TArg> argumentValue,
            string argumentName,
            string message = null)
        {
            ArgumentNotNull(argumentValue, argumentName, message);

            if (!argumentValue.Any())
            {
                throw new ArgumentException(message ?? "Argument must not be empty", argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(
            string argumentValue,
            string argumentName,
            string message = null)
        {
            ArgumentNotNull(argumentValue, argumentName, message);

            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentException(message ?? "Argument must not be empty", argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentPropertyNotNull(object propertyValue, string argumentName, string propertyName)
        {
            ArgumentNotNull(argumentName, nameof(argumentName));
            ArgumentNotNull(propertyName, nameof(propertyName));

            if (propertyValue == null)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "The property {0} must not be null", propertyName),
                    argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentPropertyNotNullOrEmpty(
            string propertyValue,
            string argumentName,
            string propertyName)
        {
            ArgumentNotNull(argumentName, nameof(argumentName));
            ArgumentNotNull(propertyName, nameof(propertyName));

            if (string.IsNullOrEmpty(propertyValue))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "The property {0} must not be null or empty",
                        propertyName),
                    argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentPropertyNotNullOrEmpty<TItem>(
            IEnumerable<TItem> propertyValue,
            string argumentName,
            string propertyName)
        {
            ArgumentNotNull(argumentName, nameof(argumentName));
            ArgumentNotNull(propertyName, nameof(propertyName));

            if (propertyValue == null || !propertyValue.Any())
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "The property {0} must not be null or empty",
                        propertyName),
                    argumentName);
            }
        }
    }
}