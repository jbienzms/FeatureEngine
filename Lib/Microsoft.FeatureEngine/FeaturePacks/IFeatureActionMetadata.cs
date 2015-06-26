using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides metadata for a feature action.
    /// </summary>
    public interface IFeatureActionMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets a description of the action.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the Id of the action.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the title of the action.
        /// </summary>
        string Title { get; }
        #endregion // Public Properties
    }
}
