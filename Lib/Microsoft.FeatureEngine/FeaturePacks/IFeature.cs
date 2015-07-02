using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides metadata for a feature.
    /// </summary>
    public interface IFeature : IMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets the actions in the package.
        /// </summary>
        IEnumerable<IAction> Actions { get; }

        /// <summary>
        /// Gets the item templates in the package.
        /// </summary>
        IEnumerable<IItemTemplate> ItemTemplates { get; }

        /// <summary>
        /// Gets the project templates in the package.
        /// </summary>
        IEnumerable<IProjectTemplate> ProjectTemplates { get; }
        #endregion // Public Properties
    }
}
