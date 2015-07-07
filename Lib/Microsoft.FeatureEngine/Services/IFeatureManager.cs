using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EnvDTE;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Public interface for the feature manager.
    /// </summary>
    public interface IFeatureManager
    {
        #region Public Methods
        /// <summary>
        /// Get the list of feature packages in the current solution.
        /// </summary>
        IEnumerable<IFeaturePack> GetPackages();

        /// <summary>
        /// Get the list of feature packages associated with the specified project.
        /// </summary>
        /// <param name="project">
        /// The project to get associated packages from.
        /// </param>
        IEnumerable<IFeaturePack> GetPackages(Project project);
        #endregion // Public Methods


        #region Public Events
        /// <summary>
        /// Raised when a feature package is associated.
        /// </summary>
        event EventHandler<FeaturePackAssociationEventArgs> PackageAssociated;

        /// <summary>
        /// Raised when a feature package is disassociated.
        /// </summary>
        event EventHandler<FeaturePackAssociationEventArgs> PackageDisassociated;

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
