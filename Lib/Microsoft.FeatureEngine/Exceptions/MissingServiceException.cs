using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// An exception that is thrown when a service is missing.
    /// </summary>
    public class MissingServiceException : Exception
    {
        public MissingServiceException(Type serviceType) : base(string.Format("The service type '{0}' could not be found.", serviceType.Name))
        {
            // Store
            this.ServiceType = serviceType;
        }

        /// <summary>
        /// Gets the missing service type
        /// </summary>
        public Type ServiceType { get; }
    }

    public class MissingServiceException<T> : MissingServiceException
    {
        public MissingServiceException() : base(typeof(T)) { }
    }
}
