using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.FeatureEngine;
using VSFeatureEngine.FeaturePacks;

namespace VSFeatureEngine
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Export(typeof(IFeatureManager))]
    public class FeatureManager : IFeatureManager
    {
        #region Member Variables
        private Collection<FeaturePack> loadedPackages = new Collection<FeaturePack>();
        #endregion // Member Variables

        #region Overridables / Event Triggers

        protected virtual void OnPackageLoaded(FeaturePack pack)
        {
            if (PackageLoaded != null)
            {
                PackageLoaded(this, new FeaturePackEventArgs(pack));
            }
        }

        protected virtual void OnPackageUnloaded(FeaturePack pack)
        {
            if (PackageUnloaded != null)
            {
                PackageUnloaded(this, new FeaturePackEventArgs(pack));
            }
        }
        #endregion // Overridables / Event Triggers


        #region Public Methods
        public void LoadPackage(string packagePath)
        {
            // Validate Parameters
            if (string.IsNullOrEmpty(packagePath)) throw new ArgumentException("packagePath");

            // Check to see if the package is already loaded
            var loaded = loadedPackages.Where(p => p.InstallPath.StartsWith(packagePath, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            // If already loaded, bail
            if (loaded != null) { return; }

            // Run with error handling
            TaskHelper.RunWithErrorHandling(() =>
            {
                if (!Directory.Exists(packagePath)) { throw new InvalidOperationException("packagePath does not specify a valid location"); }

                // Try to get the manifest
                var manifestPath = Path.Combine(packagePath, Constants.ManifestFileName);

                // Make sure manifest exists
                if (!File.Exists(manifestPath)) { throw new InvalidOperationException(string.Format("No feature package manifest ({0}) could be found at {1}.", Constants.ManifestFileName, packagePath)); }

                // Create the FeaturePack instance that we'll fill out
                var pack = new FeaturePack();

                // Load the document
                var manDoc = XDocument.Load(manifestPath);
                var ns = manDoc.Root.Name.Namespace;
                var manifest = manDoc.Root;

                var metadata = manifest.Element(ns + "metadata");
                pack.Id = (string)metadata.Element(ns + "id");
                pack.Title = (string)metadata.Element(ns + "title");

                // Notify listeners
                OnPackageLoaded(pack);

            }, TaskRunOptions.WithFailure("Could not enable feature pack"));
        }

        public void UnloadPackage(string id)
        {
            // Check to see if the package is loaded
            var pack = loadedPackages.Where(p => p.Id == id).FirstOrDefault();

            // If loaded, unload
            if (pack != null)
            {
                loadedPackages.Remove(pack);
                OnPackageUnloaded(pack);
            }
        }
        #endregion // Public Methods

        #region Public Events
        public event EventHandler<FeaturePackEventArgs> PackageLoaded;
        public event EventHandler<FeaturePackEventArgs> PackageUnloaded;
        #endregion // Public Events
    }
}
