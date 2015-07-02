using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides metadata for a feature package.
    /// </summary>
    public interface IFeaturePack : IMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets the authors of the package.
        /// </summary>
        IEnumerable<string> Authors { get; }

        /// <summary>
        /// Gets the features in the package.
        /// </summary>
        IEnumerable<IFeature> Features { get; }

        /// <summary>
        /// Gets the location where the package is installed on disk.
        /// </summary>
        string InstallPath { get; }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        string Version { get; }
        #endregion // Public Properties
    }
}
