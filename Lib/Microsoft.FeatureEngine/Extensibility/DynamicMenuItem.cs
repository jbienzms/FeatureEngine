using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Represents the item for a <see cref="DynamicMenu"/>.
    /// </summary>
    public class DynamicMenuItem
    {
        /// <summary>
        /// Gets the text for the menu item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets a value that indicates if the menu item is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the item is enabled; otherwise false.
        /// </value>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a value that indicates if the menu item is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the item is visible; otherwise false.
        /// </value>
        public bool IsVisibile { get; set; }

        /// <summary>
        /// Gets or sets custom data related to the menu item.
        /// </summary>
        public object Tag { get; set; }
    }
}
