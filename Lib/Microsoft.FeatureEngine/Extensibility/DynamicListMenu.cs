using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// A base class for a dynamic menu based on a list of items.
    /// </summary>
    /// <typeparam name="T">
    /// The type of item in the list.
    /// </typeparam>
    public abstract class DynamicListMenu<T> : DynamicMenuBase<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="DynamicListMenu"/> instance.
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
        public DynamicListMenu(Package package, Guid menuGroup, int startId) : base(package, menuGroup, startId) { }
        #endregion // Constructors

        #region Overrides / Event Handlers
        protected override T GetItem(int idx)
        {
            return InnerItems[idx];
        }

        protected override bool IsInRange(int idx)
        {
            QueryRefreshItems();
            return ((idx > -1) && (idx < InnerItems.Count));
        }
        #endregion // Overrides / Event Handlers

        #region Overrides / Event Handlers
        /// <summary>
        /// Provides inherited class the opportunity to refresh the items before 
        /// the menu is displayed.
        /// </summary>
        protected virtual void QueryRefreshItems() { }

        /// <summary>
        /// Gets the internal list of items that should be represented in the menu.
        /// </summary>
        protected abstract IList<T> InnerItems { get; }
        #endregion // Overrides / Event Handlers
    }
}