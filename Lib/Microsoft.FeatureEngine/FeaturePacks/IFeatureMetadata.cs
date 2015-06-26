using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides metadata for a feature.
    /// </summary>
    public interface IFeatureMetadata : IMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets the actions in the package.
        /// </summary>
        IEnumerable<IFeatureActionMetadata> Actions { get; }
        #endregion // Public Properties
    }
}
