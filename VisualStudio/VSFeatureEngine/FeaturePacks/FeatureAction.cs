using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public abstract class FeatureAction : MetadataBase, IFeatureAction, IFeatureActionMetadata
    {
        #region Constructors
        public FeatureAction(string id)
        {
            // Validate
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id");

            // Store
            this.Id = id;

            // Defaults
            Title = string.Empty;
            Description = string.Empty;
        }
        #endregion // Constructors

        #region Overridables / Event Triggers
        public abstract void Execute(IFeatureActionContext context);
        #endregion // Overridables / Event Triggers
    }
}
