using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine.Activities
{
    /// <summary>
    /// Provides information about activities that are part of a feature.
    /// </summary>
    public interface IFeatureActivity
    {
        /// <summary>
        /// Gets a description for the activity.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Indicates if the activity is enabled. The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        bool IsEnabled { get; set; }


        /// <summary>
        /// Gets a link to a resource that provides more information about the activity.
        /// </summary>
        Link MoreInfoLink { get; }

        /// <summary>
        /// Gets the title of the activity.
        /// </summary>
        string Title { get; }
    }
}
