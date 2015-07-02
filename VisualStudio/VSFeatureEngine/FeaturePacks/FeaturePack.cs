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
    /// Represents a feature pack.
    /// </summary>
    public class FeaturePack : Metadata, IFeaturePack
    {
        #region Constructors
        public FeaturePack()
        {
            Authors = new Collection<string>();
            Features = new Collection<Feature>();
            InstallPath = string.Empty;
            Version = string.Empty;
        }
        #endregion // Constructors

        #region Public Properties
        public Collection<string> Authors { get; set; }

        public Collection<Feature> Features { get; set; }

        public string InstallPath { get; set; }

        public string Version { get; set; }
        #endregion // Public Properties

        #region IFeaturePackMetadata Implementation
        IEnumerable<string> IFeaturePack.Authors
        {
            get
            {
                return this.Authors;
            }
        }

        IEnumerable<IFeature> IFeaturePack.Features
        {
            get
            {
                return this.Features;
            }
        }
        #endregion // IFeaturePackMetadata Implementation
    }
}
