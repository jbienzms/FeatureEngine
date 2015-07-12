using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Represents a link to a resource including URL, title and alternate text.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Link
    {
        #region Member Variables
        private string title;
        #endregion // Member Variables

        #region Public Properties
        /// <summary>
        /// Gets a value that indicates if the link is valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if the link is valid; otherwise false.
        /// </value>
        /// <remarks>
        /// The link is considered valid as long as the Url is not null, empty or whitespace.
        /// </remarks>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Url);
            }
        }

        /// <summary>
        /// Gets or sets the title of the link.
        /// </summary>
        /// <remarks>
        /// If no title is specified, the URL is returned automatically.
        /// </remarks>
        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return Url;
                }
                else
                {
                    return title;
                }
            }
            set
            {
                title = value;
            }
        }

        /// <summary>
        /// The URL that the link points to.
        /// </summary>
        public string Url { get; set; }
        #endregion // Public Properties
    }
}
