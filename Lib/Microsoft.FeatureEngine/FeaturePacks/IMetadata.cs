using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Properties common to all metadata classes.
    /// </summary>
    public interface IMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets a description of the package.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the Id of the package.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the title of the package.
        /// </summary>
        string Title { get; }
        #endregion // Public Properties
    }
}
