using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides data for events that relate to feture packs.
    /// </summary>
    public class FeaturePackEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new <see cref="FeaturePackEventArgs"/> instance.
        /// </summary>
        /// <param name="package">
        /// The package that the event relates to.
        /// </param>
        public FeaturePackEventArgs(IFeaturePack package)
        {
            // Validate
            if (package == null) throw new ArgumentNullException("package");

            // Store
            Package = package;
        }

        /// <summary>
        /// Gets the package that the event relates to.
        /// </summary>
        public IFeaturePack Package { get; private set; }
    }
}
