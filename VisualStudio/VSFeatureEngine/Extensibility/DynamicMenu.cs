using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace VSFeatureEngine
{
    public abstract class DynamicMenu<T>
    {
        private readonly Package package;
        private int startId;

        public DynamicMenu(Package package, Guid menuGroup, int startId)
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
            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            // Add it to the command service
            commandService.AddCommand(dynamicMenuCommand);
        }
        private bool IsInRange(int idx)
        {
            return ((idx > -1) && (idx < Items.Count));
        }

        private bool IsValidItem(int commandId)
        {
            int idx = ((commandId - startId) - 1);
            return IsInRange(idx);
        }


        private void ItemInvoked(object sender, EventArgs args)
        {
            DynamicMenuCommand invokedCommand = (DynamicMenuCommand)sender;
            var commandId = invokedCommand.MatchedCommandId;
            int idx = ((commandId - startId) - 1);
            if (IsInRange(idx))
            {
                OnItemInvoked(Items[idx]);
            }
        }

        private void BeforeQueryStatus(object sender, EventArgs args)
        {
            DynamicMenuCommand matchedCommand = (DynamicMenuCommand)sender;

            bool isRootItem = (matchedCommand.MatchedCommandId == 0);
            if (!isRootItem)
            {
                // Get the item index
                int idx = ((matchedCommand.MatchedCommandId - startId) - 1);

                // If in range, update and set the title
                if (IsInRange(idx))
                {
                    // Enable
                    matchedCommand.Enabled = true;
                    matchedCommand.Visible = true;

                    // Get the item
                    T item = Items[idx];

                    // Set the title
                    matchedCommand.Text = GetItemTitle(item);
                }
            }
        }

        protected abstract string GetItemTitle(T item);

        protected abstract void OnItemInvoked(T item);

        protected abstract IList<T> Items { get; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

    }
}