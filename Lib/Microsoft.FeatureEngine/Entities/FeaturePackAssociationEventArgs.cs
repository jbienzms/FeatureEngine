using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides data for events that relate to feture pack associations.
    /// </summary>
    public class FeaturePackAssociationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new <see cref="FeaturePackAssociationEventArgs"/> instance.
        /// </summary>
        /// <param name="association">
        /// The association that the event relates to.
        /// </param>
        public FeaturePackAssociationEventArgs(FeaturePackAssociation association)
        {
            // Validate
            if (association == null) throw new ArgumentNullException("association");

            // Store
            Association = association;
        }

        /// <summary>
        /// Gets the package that the event relates to.
        /// </summary>
        public FeaturePackAssociation Association { get; private set; }
    }
}
