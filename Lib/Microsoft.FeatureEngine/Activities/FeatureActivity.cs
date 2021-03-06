﻿using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine.Activities
{
    /// <summary>
    /// The base class for an activity that is part of a feature pack.
    /// </summary>
    public abstract class FeatureActivity : CodeActivity, IFeatureActivity
    {
        #region Static Version
        #region Internal Methods
        /// <summary>
        /// Gets the service of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of service to obtain.
        /// </typeparam>
        /// <param name="context">
        /// The context of the activity.
        /// </param>
        /// <returns>
        /// The service instance.
        /// </returns>
        static protected T GetService<T>(CodeActivityContext context) where T:class
        {
            if (context == null) throw new ArgumentNullException("context");
            return context.GetExtension<IServiceStore>().GetService<T>();
        }
        #endregion // Internal Methods
        #endregion // Static Version

        #region Instance Version
        #region Member Variables
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="FeatureActivity"/> instance.
        /// </summary>
        public FeatureActivity()
        {
            // Defaults
            Description = string.Empty;
            IsEnabled = true;
            Title = string.Empty;
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.RequireExtension<IServiceStore>();
        }
        #endregion // Overrides / Event Handlers

        #region Public Properties
        /// <summary>
        /// Gets a description for the activity.
        /// </summary>
        /// <remarks>
        /// The description may be displayed in a user interface that allows the user to enable or disable the activity.
        /// </remarks>
        [DefaultValue("")]
        public string Description { get; set; }

        /// <summary>
        /// Indicates if the activity is enabled. The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        [Description("Whether or not the activity will execute.")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a link to a resource that provides more information about the activity.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        [Description("A link to a resource that provides more information about the activity itself.")]
        public Link MoreInfoLink { get; set; }

        /// <summary>
        /// Gets a title for the activity.
        /// </summary>
        /// <remarks>
        /// The title may be displayed in a user interface that allows the user to enable or disable the activity.
        /// </remarks>
        [DefaultValue("")]
        public string Title { get; set; }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
