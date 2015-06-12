using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Export(typeof(IFeatureManager))]
    public class FeatureManager : IFeatureManager
    {
        public Task DisablePackageAsync(string id)
        {
            return TaskHelper.CompletedTask;
        }

        public Task EnablePackageAsync(string packagePath)
        {
            // Validate Parameters
            if (string.IsNullOrEmpty(packagePath)) throw new ArgumentException("packagePath");

            // Run on new thread and handle errors
            return TaskHelper.RunWithErrorHandlingAsync(() =>
            {
                if (!Directory.Exists(packagePath)) { throw new InvalidOperationException("packagePath does not specify a valid location"); }

                // Try to get the manifest
                var manifestPath = Path.Combine(packagePath, Constants.ManifestFileName);

                // Make sure manifest exists
                if (!File.Exists(manifestPath)) { throw new InvalidOperationException(string.Format("No feature package manifest ({0}) could be found at {1}.", Constants.ManifestFileName, packagePath)); }

                // Load the document
                var manDoc = XDocument.Load(manifestPath);
                var ns = manDoc.Root.Name.Namespace;
                var manifest = manDoc.Root;

                var metadata = manifest.Element(ns + "metadata");
                var id = (string)metadata.Element(ns + "id");
                var title = (string)metadata.Element(ns + "title");

            }, TaskRunOptions.WithFailure("Could not enable feature pack"));
        }
    }
}
