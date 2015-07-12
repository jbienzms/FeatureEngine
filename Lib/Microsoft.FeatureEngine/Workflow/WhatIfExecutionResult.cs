using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Activities;

namespace Microsoft.FeatureEngine.Workflow
{
    /// <summary>
    /// Represents the recommended executaiton state in a non "What If" scenario.
    /// </summary>
    public enum WhatIfRecommendation
    {
        /// <summary>
        /// It is recommended that the activity be executed.
        /// </summary>
        Recommended,
        
        /// <summary>
        /// It is recommended that the activity NOT be executed.
        /// </summary>
        NotRecommended,

        /// <summary>
        /// The activity is required and must execute for the workflow to succeed.
        /// </summary>
        Required,

        /// <summary>
        /// The activity is disabled and cannot execute in the current context
        /// </summary>
        Disabled,
    }

    /// <summary>
    /// Represents the result of a "What If" exeuction.
    /// </summary>
    public class WhatIfExecutionResult
    {
        #region Static Version
        #region Public Methods
        /// <summary>
        /// Gets the default description for the specified recommendation.
        /// </summary>
        /// <param name="recommendation">
        /// The recommendation to get the description for.
        /// </param>
        /// <returns>
        /// The default description
        /// </returns>
        static public string GetDefaultDescription(WhatIfRecommendation recommendation)
        {
            switch (recommendation)
            {
                case WhatIfRecommendation.Disabled:
                    return "Disabled";
                case WhatIfRecommendation.NotRecommended:
                    return "Not recommended";
                case WhatIfRecommendation.Recommended:
                    return "Recommended";
                case WhatIfRecommendation.Required:
                    return "Required";
                default:
                    return "Unknown";
            }
        }
        #endregion // Public Methods
        #endregion // Static Version

        #region Instance Version
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="WhatIfExecutionResult"/> instance.
        /// </summary>
        /// <param name="activity">
        /// The activity that completed execution.
        /// </param>
        /// <param name="recommendation">
        /// the recommended executaiton state in a non "What If" scenario.
        /// </param>
        /// <param name="recommendationDescription">
        /// A description for the recommended execution or <see langword="null"/> to use the default description.
        /// </param>
        public WhatIfExecutionResult(WhatIfActivity activity, WhatIfRecommendation recommendation, string recommendationDescription)
        {
            // Validate
            if (activity == null) throw new ArgumentNullException("activity");

            // Store
            this.Activity = activity;
            this.Recommendation = recommendation;

            // What to do about description
            if (!string.IsNullOrEmpty(recommendationDescription))
            {
                this.RecommendationDescription = recommendationDescription;
            }
            else
            {
                RecommendationDescription = GetDefaultDescription(recommendation);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="WhatIfExecutionResult"/> instance.
        /// </summary>
        /// <param name="activity">
        /// The activity that completed execution.
        /// </param>
        /// <param name="recommendation">
        /// the recommended executaiton state in a non "What If" scenario.
        /// </param>
        public WhatIfExecutionResult(WhatIfActivity activity, WhatIfRecommendation recommendation) : this(activity, recommendation, null) { }

        #endregion // Constructors

        #region Public Properties
        /// <summary>
        /// The activity that completed execution.
        /// </summary>
        public WhatIfActivity Activity { get; private set; }

        /// <summary>
        /// Gets the recommended executaiton state in a non "What If" scenario.
        /// </summary>
        public WhatIfRecommendation Recommendation { get; private set; }

        /// <summary>
        /// Gets a description for the recommended execution state.
        /// </summary>
        public string RecommendationDescription { get; private set; }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
