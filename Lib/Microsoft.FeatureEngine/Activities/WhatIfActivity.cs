using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Workflow;

namespace Microsoft.FeatureEngine.Activities
{
    /// <summary>
    /// Represents an activity that can participate in a "What If" scenario.
    /// </summary>
    public abstract class WhatIfActivity : FeatureActivity
    {
        #region Static Version
        #region Internal Methods
        /// <summary>
        /// Gets a value that indicates if the activity is executing in "What If" mode.
        /// </summary>
        /// <param name="context">
        /// The context for the activity.
        /// </param>
        /// <returns>
        /// <c>true</c> if the activity is executing in "What If" mode; otherwise false.
        /// </returns>
        static protected bool IsInWhatIfMode(CodeActivityContext context)
        {
            var whatIf = context.GetExtension<IWhatIfExtension>();
            return whatIf.IsInWhatIfMode;
        }
        #endregion // Internal Methods
        #endregion // Static Version

        #region Instance Version
        #region Overrides / Event Handlers
        protected override void Execute(CodeActivityContext context)
        {
            var whatIf = context.GetExtension<IWhatIfExtension>();
            if (whatIf.IsInWhatIfMode)
            {
                var result = ExecuteWhatIf(context);
                whatIf.ProcessExecutionResult(result);
            }
            else
            {
                ExecuteNonWhatIf(context);
            }
        }
        #endregion // Overrides / Event Handlers

        #region Overrides / Event Handlers
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.RequireExtension<IWhatIfExtension>();
        }
        /// <summary>
        /// Executes the non "What If" (or primary) function of the activity.
        /// </summary>
        /// <param name="context">
        /// The context for the activity.
        /// </param>
        protected abstract void ExecuteNonWhatIf(CodeActivityContext context);

        /// <summary>
        /// Executes a "What If" test for the activity.
        /// </summary>
        /// <param name="context">
        /// The context for the activity.
        /// </param>
        /// <returns>
        /// A <see cref="WhatIfExecutionResult"/> that indicates the result of the What-If.
        /// </returns>
        protected abstract WhatIfExecutionResult ExecuteWhatIf(CodeActivityContext context);
        #endregion // Overrides / Event Handlers
        #endregion // Instance Version
    }
}
