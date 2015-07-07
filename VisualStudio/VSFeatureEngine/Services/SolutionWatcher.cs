using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.FeatureEngine;
using NuGet.VisualStudio;

namespace VSFeatureEngine
{
    [Export]
    public class SolutionWatcher
    {
        #region Member Variables
        private Project activeProject;
        private DTE dte;
        private IFeatureManager featureManager;
        private IServiceStore serviceStore;
        private IVSAssetResolver vsResolver;
        #endregion // Member Variables

        #region Constructors
        [ImportingConstructor]
        public SolutionWatcher(IServiceStore serviceStore)
        {
            // Validate
            if (serviceStore == null) throw new ArgumentNullException("serviceStore");

            // Store
            this.serviceStore = serviceStore;
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        private void VS_SolutionSelectionChange()
        {
            HandleSelectionChange();
        }
        #endregion // Overrides / Event Handlers

        #region Internal Methods
        private void HandleSelectionChange()
        {
            // Get the currently active project
            var newActiveProject = vsResolver.GetActiveProject();

            // If not changing, ignore
            if (newActiveProject == activeProject) { return; }

            // Update current active
            activeProject = newActiveProject;

            // Get feature packs for project
            var packs = featureManager.GetPackages(activeProject);

            // Update actions
            UpdateActions(packs);
        }

        private void UpdateActions(IEnumerable<IFeaturePack> packs)
        {
            // Get the actions menu item
            var actions = ActionsMenu.Instance.Actions;

            // Clear current actions
            actions.Clear();

            // Show all actions for all feature packages
            foreach (var pack in packs)
            {
                foreach (var feature in pack.Features)
                {
                    foreach (var action in feature.Actions)
                    {
                        if (!actions.Contains(action))
                        {
                            actions.Add(action);
                        }
                    }
                }
            }
        }
        #endregion // Internal Methods


        #region Public Methods
        public void Initialize()
        {
            // Obtain additional services
            dte = serviceStore.GetService<DTE>();
            featureManager = serviceStore.GetService<IFeatureManager>();
            vsResolver = serviceStore.GetService<IVSAssetResolver>();

            // Subscribe to events
            var VS = dte.Events.SelectionEvents;
            VS.OnChange += VS_SolutionSelectionChange;

            // Trigger a selection change
            HandleSelectionChange();
        }
        #endregion // Public Methods
    }
}
