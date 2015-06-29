using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.FeatureEngine;
using VSFeatureEngine.FeaturePacks;
using VSFeatureEngine.Workflow;

namespace VSFeatureEngine
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Export(typeof(IFeatureManager))]
    public class FeatureManager : IFeatureManager
    {
        #region Embedded Classes
        private class FMNamespace
        {
            private XNamespace root;

            public FMNamespace(XNamespace root)
            {
                this.root = root;
            }

            public XName Actions { get { return root + "Actions"; } }

            public XName Extensions { get { return root + "Extensions"; } }

            public XName Features { get { return root + "Features"; } }


            public XName description { get { return XNamespace.None + "description"; } }

            public XName extension { get { return XNamespace.None + "extension"; } }

            public XName id { get { return XNamespace.None + "id"; } }

            public XName metadata { get { return XNamespace.None + "metadata"; } }

            public XName src { get { return XNamespace.None + "src"; } }

            public XName title { get { return XNamespace.None + "title"; } }

            public XName type { get { return XNamespace.None + "type"; } }
        }

        private class LoadContext
        {
            public FMNamespace NameSpace { get; set; }
            public FeaturePack Pack { get; set; }
        }
        #endregion // Embedded Classes

        private static void LoadActions(LoadContext context, XElement featureElement, Feature feature)
        {
            var ns = context.NameSpace;
            var actionsElement = featureElement.Element(ns.Actions);
            if (actionsElement != null)
            {
                // Load each action
                foreach (var actionElement in actionsElement.Elements())
                {
                    // Get attributes for the action
                    string id = (string)actionElement.Attribute(ns.id);
                    string extensionId = (string)actionElement.Attribute(ns.extension);
                    string typeName = (string)actionElement.Attribute(ns.type);

                    // Test for missing attributes
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new InvalidOperationException(string.Format(Strings.RequiredAttributeForAction, ns.id));
                    }

                    switch (actionElement.Name.LocalName.ToLower())
                    {
                        case "workflowaction":
                            // Additional validation for workflow actions
                            if (string.IsNullOrEmpty(extensionId))
                            {
                                throw new InvalidOperationException(string.Format(Strings.RequiredAttributeForCodeAction, ns.extension));
                            }
                            if (string.IsNullOrEmpty(typeName))
                            {
                                throw new InvalidOperationException(string.Format(Strings.RequiredAttributeForCodeAction, ns.type));
                            }

                            // Try to get the extension
                            var extension = feature.Extensions.Where(e => e.Id == extensionId).FirstOrDefault();
                            if (extension == null)
                            {
                                throw new InvalidOperationException(string.Format(Strings.ExtensionIdNotFound, extensionId));
                            }

                            // Try to get the type from the extension
                            var activityType = extension.Assembly.GetType(typeName);
                            if (activityType == null)
                            {
                                throw new InvalidOperationException(string.Format(Strings.TypeNameNotFoundInAssembly, typeName, extension.Assembly.FullName));
                            }

                            // Create the action
                            var wfAction = new WorkflowFeatureAction(id, activityType);

                            // Add it to the collection
                            feature.Actions.Add(wfAction);
                            break;
                    }
                }
            }
        }

        private static void LoadExtensions(LoadContext context, XElement featureElement, Feature feature)
        {
            var ns = context.NameSpace;
            var installPath = context.Pack.InstallPath;

            var extensionsElement = featureElement.Element(ns.Extensions);
            if (extensionsElement != null)
            {
                // Load each extension
                foreach (var extensionElement in extensionsElement.Elements())
                {
                    // Get the ID for the extension
                    string id = (string)extensionElement.Attribute(ns.id);

                    // Get the source
                    string src = (string)extensionElement.Attribute(ns.src);

                    // Make sure source is valid
                    if (string.IsNullOrEmpty(src))
                    {
                        throw new InvalidOperationException(string.Format(Strings.RequiredAttributeForExtension, ns.src));
                    }

                    // Convert relative to absolute
                    var loadPath = Path.Combine(installPath, src);

                    // Load the assembly
                    var asm = Assembly.LoadFrom(loadPath);

                    // Create and add the extension
                    feature.Extensions.Add(new FeatureExtension(id, asm));
                }
            }
        }

        static private void LoadFeatures(LoadContext context, XElement manifest)
        {
            var ns = context.NameSpace;
            var features = manifest.Element(ns.Features);
            if (features != null)
            {
                // Load each feature
                foreach (var featureElement in features.Elements())
                {
                    // Create feature object
                    var feature = new Feature();

                    // Load metadata
                    LoadMetadata(context, featureElement, feature);

                    // Add it to the package
                    context.Pack.Features.Add(feature);

                    // Load extensions
                    LoadExtensions(context, featureElement, feature);

                    // Load actions
                    LoadActions(context, featureElement, feature);
                }
            }
        }

        static private bool LoadMetadata(LoadContext context, XElement element, MetadataBase metadata)
        {
            var ns = context.NameSpace;
            var md = element.Element(ns.metadata);
            if (md == null) { return false; }
            metadata.Id = (string)md.Element(ns.id);
            metadata.Title = (string)md.Element(ns.title);
            metadata.Description = (string)md.Element(ns.description);
            return true;
        }

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
                if (!Directory.Exists(packagePath)) { throw new InvalidOperationException(string.Format(Strings.InvalidPath, packagePath)); }

                // Try to get the manifest
                var manifestPath = Path.Combine(packagePath, Constants.ManifestFileName);

                // Make sure manifest exists
                if (!File.Exists(manifestPath)) { throw new InvalidOperationException(string.Format(Strings.MissingFeatureManifestAtPath, Constants.ManifestFileName, packagePath)); }

                // Create the FeaturePack instance that we'll fill out
                var pack = new FeaturePack();

                // Store the location
                pack.InstallPath = packagePath;

                // Load the document
                var manDoc = XDocument.Load(manifestPath);
                var manifest = manDoc.Root;

                // Create the load context
                var context = new LoadContext()
                {
                    NameSpace = new FMNamespace(manifest.Name.Namespace),
                    Pack = pack,
                };

                // Read metadata
                LoadMetadata(context, manifest, pack);

                // Load features
                LoadFeatures(context, manifest);

                // Notify listeners
                OnPackageLoaded(pack);

            }, TaskRunOptions.WithFailure(Strings.CouldNotEnableFeaturePack));
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
