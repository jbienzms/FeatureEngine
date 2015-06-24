using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides data for events that relate to feture packs.
    /// </summary>
    public class FeaturePackEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new <see cref="FeaturePackEventArgs"/> instance.
        /// </summary>
        /// <param name="package">
        /// The package that the event relates to.
        /// </param>
        public FeaturePackEventArgs(IFeaturePackMetadata package)
        {
            // Validate
            if (package == null) throw new ArgumentNullException("package");

            // Store
            Package = package;
        }

        /// <summary>
        /// Gets the package that the event relates to.
        /// </summary>
        public IFeaturePackMetadata Package { get; private set; }
    }

    /// <summary>
    /// Public interface for the feature manager.
    /// </summary>
    public interface IFeatureManager
    {
        #region Public Methods
        /// <summary>
        /// Loads a package at the specified path.
        /// </summary>
        /// <param name="packagePath">
        /// The path to the root of the package.
        /// </param>
        void LoadPackage(string packagePath);

        /// <summary>
        /// Unloads the package with the specified id.
        /// </summary>
        /// <param name="id">
        /// The ID of the package to unload.
        /// </param>
        void UnloadPackage(string id);
        #endregion // Public Methods


        #region Public Events
        /// <summary>
        /// Raised when a feature package is loaded.
        /// </summary>
        event EventHandler<FeaturePackEventArgs> PackageLoaded;

        /// <summary>
        /// Raised when a feature package is unloaded.
        /// </summary>
        event EventHandler<FeaturePackEventArgs> PackageUnloaded;
        #endregion // Public Events
    }
}
