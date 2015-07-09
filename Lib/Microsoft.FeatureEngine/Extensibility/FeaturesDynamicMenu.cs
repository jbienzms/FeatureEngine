using System;
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

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// A <see cref="DynamicMenu{T}"/> that automatically updates its items when 
    /// the currently active project changes in Visual Studio or when the loaded 
    /// feature packs for the active project have changed.
    /// </summary>
    /// <typeparam name="T">
    /// The type of item in the list.
    /// </typeparam>
    public abstract class FeaturesDynamicMenu<T> : ProjectDynamicMenu<T>
    {
        #region Instance Version
        #region Member Variables
        private IFeatureManager featureManager;
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="FeaturePackDynamicMenu"/> instance.
        /// </summary>
        /// <param name="package">
        /// The package containing the menu and related resources.
        /// </param>
        /// <param name="menuGroup">
        /// The group where the menu will be displayed.
        /// </param>
        /// <param name="startId">
        /// The ID of the placeholder start item.
        /// </param>
        /// <remarks>
        /// For more information on how menuGroup and startId are defined see 
        /// <see href="https://msdn.microsoft.com/en-us/library/bb166492.aspx">Walkthrough: Dynamically Adding Menu Items</see>.
        /// </remarks>
        public FeaturesDynamicMenu(Package package, Guid menuGroup, int startId) : base(package, menuGroup, startId)
        {
            // Get services
            featureManager = ServiceStore.GetService<IFeatureManager>();

            // Subscribe to events
            featureManager.PackageAssociated += FeatureManager_AssociationsChanged;
            featureManager.PackageDisassociated += FeatureManager_AssociationsChanged;
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        private void FeatureManager_AssociationsChanged(object sender, FeaturePackAssociationEventArgs e)
        {
            // TODO: COM Comparisons of Project objects always return FALSE!
            /*
            // If assoications were changed in the last project we evaluated, invalidate
            if (e.Association.Project == ActiveProject)
            {
                // Associations have changed. Invalidate ourselves to be sure.
                Invalidate();
            }
            */
            Invalidate();
        }
        protected override void QueryProjectItems()
        {
            QueryFeatureItems();
        }
        #endregion // Overrides / Event Handlers

        #region Overridables / Event Triggers
        /// <summary>
        /// Called when the active project has changed or the associated features 
        /// have changed and the dynamic menu items should be recalculated.
        /// </summary>
        protected abstract void QueryFeatureItems();
        #endregion // Overridables / Event Triggers

        #region Internal Properties
        /// <summary>
        /// Gets the <see cref="IFeatureManager"/> used by the menu.
        /// </summary>
        protected IFeatureManager FeatureManager { get { return featureManager; } }
        #endregion // Internal Properties
        #endregion // Instance Version
    }
}
