using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class FeaturePack : IFeaturePackMetadata
    {
        public FeaturePack()
        {
            Authors = new Collection<string>();
            Description = string.Empty;
            Id = string.Empty;
            InstallPath = string.Empty;
            Title = string.Empty;
            Version = string.Empty;
        }
        public Collection<string> Authors { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        public string InstallPath { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        #region IFeaturePackMetadata Implementation
        IEnumerable<string> IFeaturePackMetadata.Authors
        {
            get
            {
                return this.Authors;
            }
        }
        #endregion // IFeaturePackMetadata Implementation
    }
}
