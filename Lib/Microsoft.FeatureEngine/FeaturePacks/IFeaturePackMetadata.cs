using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides the metadata for a feature package.
    /// </summary>
    public interface IFeaturePackMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets the authors of the package.
        /// </summary>
        IEnumerable<string> Authors { get; }

        /// <summary>
        /// Gets a description of the package.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the Id of the package.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the location where the package is installed on disk.
        /// </summary>
        string InstallPath { get; }

        /// <summary>
        /// Gets the title of the package.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        string Version { get; }
        #endregion // Public Properties
    }
}
