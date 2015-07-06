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
        /// Asssociates a package with a project.
        /// </summary>
        /// <param name="packageId">
        /// The ID of the package to associate.
        /// </param>
        /// <param name="project">
        /// The project to associate the package with.
        /// </param>
        /// <remarks>
        /// If the package is already associated, this method is ignored.
        /// </remarks>
        void Associate(string packageId, Project project);

        /// <summary>
        /// Disasssociates a package from a project.
        /// </summary>
        /// <param name="packageId">
        /// The ID of the package to disassociate.
        /// </param>
        /// <param name="project">
        /// The project to disassociate from the package.
        /// </param>
        /// <remarks>
        /// If the package is not associated, this method is ignored.
        /// </remarks>
        void Dissociate(string packageId, Project project);

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

        /// <summary>
        /// Loads the feature package at the specified path.
        /// </summary>
        /// <param name="packagePath">
        /// The path to the root of the package.
        /// </param>
        /// <remarks>
        /// If the package is already loaded this method is ignored.
        /// </remarks>
        void LoadPackage(string packagePath);

        /// <summary>
        /// Unloads the package with the specified id.
        /// </summary>
        /// <param name="packageId">
        /// The ID of the package to unload.
        /// </param>
        void UnloadPackage(string packageId);
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
