﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio.Shell;
using VSFeatureEngine.FeaturePacks;

namespace VSFeatureEngine
{
    public class ActionsMenu : DynamicListMenu<IFeatureAction>
    {
        #region Static Version
        #region Constants
        private const int StartCommandId = 0x0102;
        #endregion // Constants

        #region Public Methods
        /// <summary>
        /// Initializes the singleton instance of the menu.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            if (Instance == null)
            {
                Instance = new ActionsMenu(package);
            }
        }
        #endregion // Public Methods

        #region Public Properties
        /// <summary>
        /// Gets the instance of the menu.
        /// </summary>
        public static ActionsMenu Instance
        {
            get;
            private set;
        }
        #endregion // Public Properties
        #endregion // Static Version


        #region Instance Version
        #region Member Variables
        private Collection<IFeatureAction> actions = new Collection<IFeatureAction>();
        private ExecutionContext context;
        private IFeatureManager featureManager;
        private WeakReference<Project> lastActiveProject;
        private IVSAssetResolver vsResolver;
        #endregion // Member Variables

        public ActionsMenu(Package package) : base(package, MenuGuids.FeatureEngineCommandSet, StartCommandId)
        {
            // Get services
            featureManager = ServiceStore.GetService<IFeatureManager>();
            vsResolver = ServiceStore.GetService<IVSAssetResolver>();

            // Create the execution context
            context = new ExecutionContext(ServiceStore);
        }

        #region Overrides / Event Handlers
        protected override string GetText(IFeatureAction item)
        {
            return item.Title;
        }

        protected override IList<IFeatureAction> InnerItems
        {
            get
            {
                return actions;
            }
        }

        protected override void OnItemInvoked(IFeatureAction item)
        {
            Debug.WriteLine("Invoked: " + item.Title);
            TaskHelper.RunWithErrorHandling(() =>
            {
                item.Execute(context);
            }, TaskRunOptions.WithFailure(Strings.CouldNotCompleteAction));
        }

        protected override void QueryRefreshItems()
        {
            // Get the currently active project
            var activeProject = vsResolver.GetActiveProject();

            // If there is no new project, clear and bail
            if (activeProject == null)
            {
                lastActiveProject = null;
                actions.Clear();
                return;
            }

            // If new and current are same, nothing to refresh
            Project last = null;
            if ((lastActiveProject != null) && (lastActiveProject.TryGetTarget(out last)))
            {
                if (activeProject == last) { return; }
            }
            
            // Update last
            lastActiveProject = new WeakReference<Project>(activeProject);

            // Clear current actions
            actions.Clear();

            // Get feature packs for project
            var packs = featureManager.GetPackages(activeProject);

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

            base.QueryRefreshItems();
        }
        #endregion // Overrides / Event Handlers
        #endregion // Instance Version
    }
}
