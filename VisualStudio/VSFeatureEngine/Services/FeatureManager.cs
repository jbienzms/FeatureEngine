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
using EnvDTE;
using Microsoft.FeatureEngine;
using NuGet.VisualStudio;
using VSFeatureEngine.FeaturePacks;
using VSFeatureEngine.Workflow;

namespace VSFeatureEngine
{
    [Export(typeof(FeatureManager))]
    [Export(typeof(IFeatureManager))]
    [Export(typeof(IFeatureAssetResolver))]
    public class FeatureManager : IFeatureManager, IFeatureAssetResolver
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

        #region Static Version
        #region Internal Methods
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
                    string title = (string)actionElement.Attribute(ns.title);
                    string description = (string)actionElement.Attribute(ns.description);
                    string extensionId = (string)actionElement.Attribute(ns.extension);
                    string typeName = (string)actionElement.Attribute(ns.type);

                    // Test for missing attributes
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new InvalidOperationException(string.Format(Strings.RequiredAttributeForAction, ns.id));
                    }

                    if (string.IsNullOrEmpty(title))
                    {
                        throw new InvalidOperationException(string.Format(Strings.RequiredAttributeForAction, ns.title));
                    }

                    // Placeholder
                    FeatureAction action = null;

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
                            action = wfAction;
                            break;
                    }

                    // If action was created, set metadata and add it
                    if (action != null)
                    {
                        // Load metadata
                        LoadMetadata(context, actionElement, action);

                        // Add it to the collection
                        feature.Actions.Add(action);

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

        static private void LoadMetadata(LoadContext context, XElement element, Metadata metadata)
        {
            var ns = context.NameSpace;
            metadata.Id = (string)element.Attribute(ns.id);
            metadata.Title = (string)element.Attribute(ns.title);
            metadata.Description = (string)element.Attribute(ns.description);
        }
        #endregion // Internal Methods
        #endregion // Static Version

        #region Instance Version
        #region Member Variables
        private DTE dte;
        private Collection<FeaturePack> loadedPackages = new Collection<FeaturePack>();
        private IVsPackageInstallerEvents nugetEvents;
        private IVsPackageInstallerServices nugetService;
        private Dictionary<IVsPackageMetadata, FeaturePack> packagesByNuPack = new Dictionary<IVsPackageMetadata, FeaturePack>();
        private Dictionary<Project, Collection<FeaturePack>> packagesByProject = new Dictionary<Project, Collection<FeaturePack>>();
        private IServiceStore serviceStore;
        #endregion // Member Variables

        #region Constructors
        [ImportingConstructor]
        public FeatureManager(IServiceStore serviceStore)
        {
            // Validate
            if (serviceStore == null) throw new ArgumentNullException("serviceStore");

            // Store
            this.serviceStore = serviceStore;
        }
        #endregion // Constructors

        #region Internal Methods
        private void Associate(FeaturePack fePack, Project project)
        {
            // Validate
            if (fePack == null) throw new ArgumentNullException("fePack");
            if (project == null) throw new ArgumentNullException("project");

            // Get lookup table
            var lookup = FindOrCreateLookup(project);

            // Add if not already in table
            if (!lookup.Contains(fePack))
            {
                lookup.Add(fePack);
            }

            // Notify
            if (PackageAssociated != null)
            {
                PackageAssociated(this, new FeaturePackAssociationEventArgs(fePack, project));
            }
        }

        private void Dissociate(FeaturePack fePack, Project project)
        {
            // Validate
            if (fePack == null) throw new ArgumentNullException("pack");
            if (project == null) throw new ArgumentNullException("project");

            // Get lookup table
            var lookup = FindOrCreateLookup(project);

            // Remove
            lookup.Remove(fePack);

            // Notify
            if (PackageDisassociated != null)
            {
                PackageDisassociated(this, new FeaturePackAssociationEventArgs(fePack, project));
            }
        }

        private Collection<FeaturePack> FindOrCreateLookup(Project project)
        {
            // Validate
            if (project == null) throw new ArgumentNullException("project");

            // Find or create lookup
            if (packagesByProject.ContainsKey(project))
            {
                return packagesByProject[project];
            }
            else
            {
                var col = new Collection<FeaturePack>();
                packagesByProject[project] = col;
                return col;
            }
        }

        private FeaturePack LoadPackage(string featurePackPath)
        {
            // Validate Parameters
            if (string.IsNullOrEmpty(featurePackPath)) throw new ArgumentException("packagePath");

            // Check to see if the package is already loaded
            var loaded = loadedPackages.Where(p => p.InstallPath.StartsWith(featurePackPath, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            // If already loaded, bail
            if (loaded != null) { return loaded; }

            // Make sure path is valid
            if (!Directory.Exists(featurePackPath)) { throw new InvalidOperationException(string.Format(Strings.InvalidPath, featurePackPath)); }

            // Try to get the manifest
            var manifestPath = Path.Combine(featurePackPath, Constants.ManifestFileName);

            // Make sure manifest exists
            if (!File.Exists(manifestPath)) { throw new InvalidOperationException(string.Format(Strings.MissingFeatureManifestAtPath, Constants.ManifestFileName, featurePackPath)); }

            // Create the FeaturePack instance that we'll fill out
            var pack = new FeaturePack();

            // Store the location
            pack.InstallPath = featurePackPath;

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

            // Add to cache
            loadedPackages.Add(pack);

            // Notify listeners
            OnPackageLoaded(pack);

            // Return loaded package
            return pack;
        }

        private void LoadProjectPackages(Project project)
        {
            // Placeholders
            FeaturePack fePack = null;

            // Get all nuget packages for the specified project
            foreach (var nuPack in nugetService.GetInstalledPackages(project))
            {
                // Try to load as a feature pack
                if (TryLoadPackage(nuPack, out fePack))
                {
                    // Associate.
                    Associate(fePack, project);
                }
            }
        }

        private void LoadSolutionPackages()
        {
            // Get all nuget packages in the current solution
            foreach (var nuPack in nugetService.GetInstalledPackages())
            {
                // Try to load as a feature pack
                TryLoadPackage(nuPack);
            }

            // If a solution is open, load all packages in open projects
            if (dte.Solution != null)
            {
                // Enum all open projects
                foreach (var project in dte.Solution.Projects.Cast<Project>())
                {
                    // Load packages in project
                    LoadProjectPackages(project);
                }
            }
        }

        private void Shutdown()
        {
            // Remove all lookup tables
            packagesByProject.Clear();
            packagesByNuPack.Clear();

            // Unload all packages
            for (int i = loadedPackages.Count - 1; i >= 0; i--)
            {
                UnloadPackage(loadedPackages[i]);
            }
        }

        /// <summary>
        /// Loads a feature pack from a nuget pacakge.
        /// </summary>
        /// <param name="nuPack">
        /// The nuget package to load the feature pack from.
        /// </param>
        /// <param name="fePack">
        /// The loaded feature pack if successful; otherwise <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the nuget package was loaded as a feature pack; otherwise false.
        /// </returns>
        private bool TryLoadPackage(IVsPackageMetadata nuPack, out FeaturePack fePack)
        {
            // Must be set at least once
            fePack = null;

            // See if it's already loaded by nuPack lookup
            if (packagesByNuPack.ContainsKey(nuPack))
            {
                fePack = packagesByNuPack[nuPack];
                return true;
            }

            // Calculate path to package
            var featurePackPath = Path.Combine(nuPack.InstallPath, Constants.FeaturePackPath);

            // Caclulate path to manifest
            var manifestPath = Path.Combine(featurePackPath, Constants.ManifestFileName);

            // See if the file exists
            if (File.Exists(manifestPath))
            {
                // Load the package
                fePack = LoadPackage(featurePackPath);

                // Cache in lookup
                packagesByNuPack[nuPack] = fePack;

                // Success
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Loads a feature pack from a nuget pacakge.
        /// </summary>
        /// <param name="nuPack">
        /// The nuget package to load the feature pack from.
        /// </param>
        private void TryLoadPackage(IVsPackageMetadata nuPack)
        {
            FeaturePack p = null;
            TryLoadPackage(nuPack, out p);
        }

        private void UnloadPackage(FeaturePack pack)
        {
            // Validate
            if (pack == null) throw new ArgumentNullException("pack");

            // Remove from Project -> Package Lookup
            foreach (var projLU in packagesByProject)
            {
                projLU.Value.Remove(pack);
            }

            // Remove from NuGet -> Package Lookup
            var nuPak = packagesByNuPack.FirstOrDefault(x => x.Value == pack).Key;
            if (nuPak != null)
            {
                packagesByNuPack.Remove(nuPak);
            }

            // Remove
            loadedPackages.Remove(pack);

            // Notify
            OnPackageUnloaded(pack);
        }
        #endregion // Internal Methods

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

        #region Overrides / Event Handlers
        private void NuGet_PackageInstalled(IVsPackageMetadata metadata)
        {
            // Run with error handling
            TaskHelper.RunWithErrorHandling(() =>
            {
                // Load the package
                TryLoadPackage(metadata);

            }, TaskRunOptions.WithFailure(Strings.CouldNotLoadFeaturePack));
        }

        private void NuGet_PackageReferenceAdded(IVsPackageMetadata metadata)
        {
            // TODO: Associate
        }

        private void NuGet_PackageReferenceRemoved(IVsPackageMetadata metadata)
        {
            // TODO: Disassociate
        }

        private void NuGet_PackageUninstalling(IVsPackageMetadata metadata)
        {
            // Run with error handling
            TaskHelper.RunWithErrorHandling(() =>
            {
                // Is this NuGet package a loaded feature pack?
                if (packagesByNuPack.ContainsKey(metadata))
                {
                    // Yup, get the associated feature pack
                    var pack = packagesByNuPack[metadata];

                    // Disassociate
                    packagesByNuPack.Remove(metadata);

                    // Unload it
                    UnloadPackage(pack);
                }
            }, TaskRunOptions.WithFailure(Strings.CouldNotUnloadFeaturePack));
        }

        private void VS_ProjectRemoved(Project project)
        {
            // Remove lookup
            packagesByProject.Remove(project);
        }

        private void VS_ProjectAdded(Project project)
        {
            // Run with error handling
            TaskHelper.RunWithErrorHandling(() =>
            {
                LoadProjectPackages(project);

            }, TaskRunOptions.WithFailure(Strings.CouldNotDetermineFeaturesInProject));
        }

        private void VS_SolutionClosing()
        {
            // Run with error handling
            TaskHelper.RunWithErrorHandling(() =>
            {
                Shutdown();
            }, TaskRunOptions.WithFailure(Strings.CouldNotUnloadAllPacks));
        }
        #endregion // Overrides / Event Handlers


        #region Public Methods
        public IFeatureAction GetAction(string featureId, string actionId)
        {
            // Get the feature
            var feature = GetFeature(featureId);

            // Try to find the action
            var action = feature.Actions.Where(a => a.Id == actionId).FirstOrDefault();

            // Verify we got the action
            if (action == null) { throw new InvalidOperationException(string.Format(Strings.ActionWithIdNotFoundInFeatureId, actionId, featureId)); }

            // Return
            return action;
        }

        public IFeature GetFeature(string featureId)
        {
            // Try to get the feature
            var feature = loadedPackages.SelectMany(p => p.Features).Where(f => f.Id == featureId).FirstOrDefault();

            // Verify we got the feature
            if (feature == null) { throw new InvalidOperationException(string.Format(Strings.FeatureWithIdNotFound, featureId)); }

            // Return
            return feature;
        }

        public IItemTemplate GetItemTemplate(string featureId, string templateId)
        {
            // Get the feature
            var feature = GetFeature(featureId);

            // Try to find the template
            var template = feature.ItemTemplates.Where(t => t.Id == templateId).FirstOrDefault();

            // Verify we got the template
            if (template == null) { throw new InvalidOperationException(string.Format(Strings.ItemTemplateWithIdNotFoundInFeatureId, templateId, featureId)); }

            // Return
            return template;
        }

        public IEnumerable<IFeaturePack> GetPackages()
        {
            return loadedPackages;
        }

        public IEnumerable<IFeaturePack> GetPackages(Project project)
        {
            // Validate
            if (project == null) throw new ArgumentNullException("project");

            // Return the lookup table for the project
            return FindOrCreateLookup(project);
        }

        public IProjectTemplate GetProjectTemplate(string featureId, string templateId)
        {
            // Get the feature
            var feature = GetFeature(featureId);

            // Try to find the template
            var template = feature.ProjectTemplates.Where(t => t.Id == templateId).FirstOrDefault();

            // Verify we got the template
            if (template == null) { throw new InvalidOperationException(string.Format(Strings.ProjectTemplateWithIdNotFoundInFeatureId, templateId, featureId)); }

            // Return
            return template;
        }

        public void Initialize()
        {
            // Obtain additional services
            dte = serviceStore.GetService<DTE>();
            nugetEvents = serviceStore.GetService<IVsPackageInstallerEvents>();
            nugetService = serviceStore.GetService<IVsPackageInstallerServices>();

            // Subscribe to VS Events
            var VS = dte.Events.SolutionEvents;
            VS.BeforeClosing += VS_SolutionClosing;
            VS.ProjectAdded += VS_ProjectAdded;
            VS.ProjectRemoved += VS_ProjectRemoved;

            // Subscribe to NuGet events
            nugetEvents.PackageInstalled += NuGet_PackageInstalled;
            nugetEvents.PackageReferenceAdded += NuGet_PackageReferenceAdded;
            nugetEvents.PackageReferenceRemoved += NuGet_PackageReferenceRemoved;
            nugetEvents.PackageUninstalling += NuGet_PackageUninstalling;
        }
        #endregion // Public Methods

        #region Public Events
        public event EventHandler<FeaturePackAssociationEventArgs> PackageAssociated;
        public event EventHandler<FeaturePackAssociationEventArgs> PackageDisassociated;
        public event EventHandler<FeaturePackEventArgs> PackageLoaded;
        public event EventHandler<FeaturePackEventArgs> PackageUnloaded;
        #endregion // Public Events
        #endregion // Instance Version
    }
}
