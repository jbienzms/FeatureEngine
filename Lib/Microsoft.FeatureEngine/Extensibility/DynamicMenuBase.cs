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
    /// A base class that helps build dynamic menus for Visual Studio.
    /// </summary>
    /// <typeparam name="T">
    /// The custom item type used to represent each entry in the list.
    /// </typeparam>
    public abstract class DynamicMenuBase<T>
    {
        #region Member Variables
        private readonly Package package;
        private int startId;
        #endregion // Member Variables

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
        public DynamicMenuBase(Package package, Guid menuGroup, int startId)
        {
            // Validate
            if (package == null) throw new ArgumentNullException("package");

            // Store
            this.package = package;
            this.startId = startId;

            // Create the start command ID
            var startCommandId = new CommandID(menuGroup, startId);

            // Create the dynamic command
            DynamicMenuCommand dynamicMenuCommand = new DynamicMenuCommand(startCommandId, IsValidItem, ItemInvoked, BeforeQueryStatus);

            // Get the command service
            OleMenuCommandService commandService = ServiceStore.GetService<IMenuCommandService>() as OleMenuCommandService;

            // Add it to the command service
            commandService.AddCommand(dynamicMenuCommand);
        }
        #endregion // Constructors


        #region Internal Methods
        /// <summary>
        /// Called when it's time for the dynamic menu to generate commands 
        /// for its items.
        /// </summary>
        private void BeforeQueryStatus(object sender, EventArgs args)
        {
            // Attempt to cast to our own custom command.
            var matchedCommand = sender as DynamicMenuCommand;

            // If not our own custom command, bail
            if (matchedCommand == null) { return; }

            // Get ID
            var commandId = matchedCommand.MatchedCommandId;

            // Find out if it's the root item
            bool isRoot = (commandId == 0);

            // Get the item index
            int idx = (isRoot ? 0 : (commandId - startId));

            // If in range, update
            if (IsInRange(idx))
            {
                // Get the item
                T item = GetItem(idx);

                // Update enabled and visibility
                matchedCommand.Enabled = GetEnabled(item);
                matchedCommand.Visible = GetVisible(item);

                // Update title
                matchedCommand.Text = GetText(item);
            }
            else
            {
                // Not in range, hide
                matchedCommand.Enabled = false;
                matchedCommand.Visible = false;
            }
        }

        /// <summary>
        /// Determines if the command ID is valid. That is greater than the start ID 
        /// and less than the max number of items in the list.
        /// </summary>
        /// <param name="commandId">
        /// The command ID to verify.
        /// </param>
        /// <returns>
        /// <c>true</c> if the command ID is valid; otherwise false.
        /// </returns>
        /// <remarks>
        /// This base implementation converts the call into a check to see if the command 
        /// ID represents a valid index instead by subtracting the start ID and 1 then 
        /// calling the <see cref="IsInRange"/> override.
        /// </remarks>
        private bool IsValidItem(int commandId)
        {
            int idx = (commandId - startId);
            return IsInRange(idx);
        }

        /// <summary>
        /// Called when the dynamic menu item is actually clicked.
        /// </summary>
        /// <param name="sender">
        /// The menu itself.
        /// </param>
        /// <param name="args">
        /// Event args for the event.
        /// </param>
        /// <remarks>
        /// This method looks up the underlying item and then calls the <see cref="OnItemInvoked"/> override.
        /// </remarks>
        private void ItemInvoked(object sender, EventArgs args)
        {
            // Attempt to cast to our special command
            var invokedCommand = sender as DynamicMenuCommand;

            // If we didn't get our special command, ignore
            if (invokedCommand == null) { return; }

            // What is the command
            var commandId = invokedCommand.MatchedCommandId;

            // Is it the root?
            var isRoot = commandId == 0;

            // Convert command ID to index
            int idx = (isRoot ? 0 : (commandId - startId));

            // Check to see if we have a valid index
            if (IsInRange(idx))
            {
                // It's a valid index, get the represented item
                var item = GetItem(idx);

                // Notify inheriters that the item was invoked
                OnItemInvoked(item);
            }
        }
        #endregion // Internal Methods

        #region Overrides / Event Handlers
        /// <summary>
        /// Gets a value that indicates if the item is enabled.
        /// </summary>
        /// <param name="item">
        /// The item to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the item is enabled; otherwise false.
        /// </returns>
        protected virtual bool GetEnabled(T item)
        {
            return true;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="idx">
        /// The index of the item to obtain.
        /// </param>
        /// <returns>
        /// The item.
        /// </returns>
        protected abstract T GetItem(int idx);

        /// <summary>
        /// Gets the text to display for the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to obtain the text for.
        /// </param>
        /// <returns>
        /// The text of the item.
        /// </returns>
        protected abstract string GetText(T item);

        /// <summary>
        /// Gets a value that indiciates if the item is visible.
        /// </summary>
        /// <param name="item">
        /// The item to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the item is visible; otherwise false.
        /// </returns>
        protected virtual bool GetVisible(T item)
        {
            return true;
        }

        /// <summary>
        /// Gets a value that indicates if the index is a valid index in the item collection.
        /// </summary>
        /// <param name="idx">
        /// The index to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the index is valid; otherwise false.
        /// </returns>
        protected abstract bool IsInRange(int idx);

        /// <summary>
        /// Occurs when the specified item is invoked.
        /// </summary>
        /// <param name="item">
        /// The item that was invoked.
        /// </param>
        protected abstract void OnItemInvoked(T item);
        #endregion // Overrides / Event Handlers


        #region Internal Properties
        /// <summary>
        /// Gets the service store from the owner package.
        /// </summary>
        protected IServiceStore ServiceStore
        {
            get
            {
                return (IServiceStore)((IServiceProvider)this.package).GetService(typeof(IServiceStore));
            }
        }
        #endregion // Internal Properties
    }
}