using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides metadata for an item template.
    /// </summary>
    public interface IItemTemplate : IMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets the location where the template is installed on disk.
        /// </summary>
        string InstallPath { get; }
        #endregion // Public Properties
    }
}
