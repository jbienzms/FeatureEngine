using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// A base class for a dynamic menu based on a list of custom items.
    /// </summary>
    /// <typeparam name="T">
    /// The type of item in the list.
    /// </typeparam>
    public abstract class DynamicMenu<T> : DynamicMenuBase<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="DynamicMenu"/> instance.
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
        public DynamicMenu(Package package, Guid menuGroup, int startId) : base(package, menuGroup, startId) { }
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

    /// <summary>
    /// A fully implemented <see cref="DynamicMenu{T}"/> where the items are <see cref="DynamicMenuItem"/>.
    /// </summary>
    public class DynamicMenu : DynamicMenu<DynamicMenuItem>
    {
        #region Member Variables
        private Collection<DynamicMenuItem> items = new Collection<DynamicMenuItem>();
        #endregion // Member Variables

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
        public DynamicMenu(Package package, Guid menuGroup, int startId) : base(package, menuGroup, startId) { }
        #endregion // Constructors

        #region Overrides / Event Handlers
        protected override bool GetEnabled(DynamicMenuItem item)
        {
            return item.IsEnabled;
        }

        protected override string GetText(DynamicMenuItem item)
        {
            return item.Text;
        }

        protected override bool GetVisible(DynamicMenuItem item)
        {
            return item.IsVisibile;
        }

        protected override IList<DynamicMenuItem> InnerItems
        {
            get
            {
                return items;
            }
        }

        protected override void OnItemInvoked(DynamicMenuItem item)
        {
            if (ItemInvoked != null)
            {
                ItemInvoked(this, new DynamicMenuItemEventArgs(item));
            }
        }
        #endregion // Overrides / Event Handlers

        #region Public Properties
        /// <summary>
        /// Gets the collection of dynamic menu items.
        /// </summary>
        public Collection<DynamicMenuItem> Items
        {
            get
            {
                return items;
            }
        }
        #endregion // Public Properties

        #region Public Events
        /// <summary>
        /// Occurs when a menu item is invoked (usually by being clicked).
        /// </summary>
        public event EventHandler<DynamicMenuItemEventArgs> ItemInvoked;
        #endregion // Public Events
    }
}
