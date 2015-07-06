using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// The interface for service storage.
    /// </summary>
    public interface IServiceStore
    {
        /// <summary>
        /// Gets the service of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of service to obtain.
        /// </typeparam>
        /// <returns>
        /// The service instance.
        /// </returns>
        T GetService<T>() where T:class;
    }
}
