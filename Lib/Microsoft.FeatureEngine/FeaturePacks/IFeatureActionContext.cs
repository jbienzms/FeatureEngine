using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides context information for a feature action.
    /// </summary>
    public interface IFeatureActionContext
    {
        /// <summary>
        /// Gets the service container for the action.
        /// </summary>
        IServiceContainer ServiceContainer { get; }
    }
}
