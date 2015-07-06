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
    public class Feature : Metadata, IFeature
    {
        #region Constructors
        public Feature()
        {
            Actions = new Collection<FeatureAction>();
            Extensions = new Collection<FeatureExtension>();
            ItemTemplates = new Collection<ItemTemplate>();
            ProjectTemplates = new Collection<ProjectTemplate>();
        }
        #endregion // Constructors

        #region Public Properties
        public Collection<FeatureAction> Actions { get; set; }

        public Collection<FeatureExtension> Extensions { get; set; }

        public Collection<ItemTemplate> ItemTemplates { get; set; }

        public Collection<ProjectTemplate> ProjectTemplates { get; set; }
        #endregion // Public Properties

        #region IFeatureMetadata Implementation
        IEnumerable<IFeatureAction> IFeature.Actions
        {
            get
            {
                return this.Actions;
            }
        }

        IEnumerable<IItemTemplate> IFeature.ItemTemplates
        {
            get
            {
                return this.ItemTemplates;
            }
        }

        IEnumerable<IProjectTemplate> IFeature.ProjectTemplates
        {
            get
            {
                return this.ProjectTemplates;
            }
        }
        #endregion // IFeaturePackMetadata Implementation

    }
}
