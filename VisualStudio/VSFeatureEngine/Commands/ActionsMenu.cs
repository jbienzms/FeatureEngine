using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using VSFeatureEngine.FeaturePacks;

namespace VSFeatureEngine
{
    public class ActionsMenu : DynamicMenu<FeatureAction>
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
        private Collection<FeatureAction> actions;
        #endregion // Member Variables

        public ActionsMenu(Package package) : base(package, MenuGuids.FeatureEngineCommandSet, StartCommandId)
        {
            actions = new Collection<FeatureAction>();
        }

        #region Overrides / Event Handlers
        protected override string GetItemTitle(FeatureAction item)
        {
            return item.Title;
        }

        protected override void OnItemInvoked(FeatureAction item)
        {
            Debug.WriteLine("Invoked: " + item.Title);
        }

        protected override IList<FeatureAction> Items
        {
            get
            {
                return actions;
            }
        }
        #endregion // Overrides / Event Handlers

        public Collection<FeatureAction> Actions { get { return actions; } }
        #endregion // Instance Version
    }
}
