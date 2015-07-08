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
    /// the currently active project changes in Visual Studio.
    /// </summary>
    /// <typeparam name="T">
    /// The type of item in the list.
    /// </typeparam>
    public abstract class ProjectDynamicMenu<T> : DynamicMenu<T>
    {
        #region Instance Version
        #region Member Variables
        private SolutionTracker tracker;
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="ProjectDynamicMenu"/> instance.
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
        public ProjectDynamicMenu(Package package, Guid menuGroup, int startId) : base(package, menuGroup, startId)
        {
            // Get services
            var resolver = ServiceStore.GetService<IVSAssetResolver>();

            // Create tracker
            tracker = new SolutionTracker(resolver);
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        protected override void QueryRefreshItems()
        {
            // Did the active project change?
            Project activeProject = null;
            if (tracker.QueryProjectChanged(out activeProject))
            {
                // Query
                QueryProjectItems(activeProject);
            }

            // Pass to base
            base.QueryRefreshItems();
        }
        #endregion // Overrides / Event Handlers

        #region Overridables / Event Triggers
        /// <summary>
        /// Called when the active project has changed and the dynamic 
        /// menu items should be recalculated.
        /// </summary>
        /// <param name="activeProject"></param>
        protected abstract void QueryProjectItems(Project activeProject);
        #endregion // Overridables / Event Triggers
        #endregion // Instance Version
    }
}
