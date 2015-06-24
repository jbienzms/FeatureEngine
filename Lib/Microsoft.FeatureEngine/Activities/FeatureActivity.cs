using System;
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
    public abstract class FeatureActivity : WhatIfActivity
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
            return context.GetExtension<IServiceContainer>().GetService<T>();
        }
        #endregion // Internal Methods
        #endregion // Static Version

        #region Instance Version
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="FeatureActivity"/> instance.
        /// </summary>
        public FeatureActivity()
        {
            IsEnabled = true;
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.RequireExtension<IServiceContainer>();
        }

        protected override void Execute(CodeActivityContext context)
        {
            bool whatIfMode = IsInWhatIfMode(context);

            // Only pass to base if execution is enabled or if we are testing what-if.
            if (IsEnabled || whatIfMode)
            {
                base.Execute(context);
            }
        }
        #endregion // Overrides / Event Handlers

        #region Public Properties
        /// <summary>
        /// Indicates if the activity is enabled. The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
