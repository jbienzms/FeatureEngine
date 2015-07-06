using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// An exception that indicates the failure of an activity.
    /// </summary>
    public class ActivityFailureException : Exception
    {
        /// <summary>
        /// Initializes a new <see cref="ActivityFailureException"/> instance.
        /// </summary>
        /// <param name="message">
        /// A message that describes the failure.
        /// </param>
        public ActivityFailureException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new <see cref="ActivityFailureException"/> instance.
        /// </summary>
        /// <param name="message">
        /// A message that describes the failure.
        /// </param>
        /// <param name="innerException">
        /// The internal exception that caused the failure.
        /// </param>
        public ActivityFailureException(string message, Exception innerException) : base(message, innerException) { }
    }
}
