using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides data for events related to <see cref="DynamicMenuItem"/> objects.
    /// </summary>
    public class DynamicMenuItemEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="DynamicMenuItemEventArgs"/> instance.
        /// </summary>
        /// <param name="item">
        /// The item for the event.
        /// </param>
        public DynamicMenuItemEventArgs(DynamicMenuItem item)
        {
            // Validate
            if (item == null) throw new ArgumentNullException("item");

            // Store
            Item = item;
        }
        #endregion // Constructors


        #region Public Properties
        /// <summary>
        /// Gets the item for the event.
        /// </summary>
        public DynamicMenuItem Item { get; private set; }
        #endregion // Public Properties
    }
}
