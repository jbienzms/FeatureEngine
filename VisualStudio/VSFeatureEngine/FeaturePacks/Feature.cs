using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    /// <summary>
    /// Represents a feature in a feature pack.
    /// </summary>
    public class Feature : MetadataBase, IFeatureMetadata
    {
        #region Constructors
        public Feature()
        {
            Actions = new Collection<FeatureAction>();
            Extensions = new Collection<FeatureExtension>();
        }
        #endregion // Constructors

        #region Public Properties
        public Collection<FeatureAction> Actions { get; set; }

        public Collection<FeatureExtension> Extensions { get; set; }

        #endregion // Public Properties

        #region IFeatureMetadata Implementation
        IEnumerable<IFeatureActionMetadata> IFeatureMetadata.Actions
        {
            get
            {
                return this.Actions;
            }
        }
        #endregion // IFeaturePackMetadata Implementation

    }
}
