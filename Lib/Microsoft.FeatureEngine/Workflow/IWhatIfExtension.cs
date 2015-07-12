using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Activities;

namespace Microsoft.FeatureEngine.Workflow
{
    /// <summary>
    /// The interface for the "WhatIf" extension used in feature workflows.
    /// </summary>
    public interface IWhatIfExtension
    {
        /// <summary>
        /// Gets a value that indicates if activities should execute in "What If" mode.
        /// </summary>
        bool IsInWhatIfMode { get; }

        /// <summary>
        /// Processes the result of a "What If" execution.
        /// </summary>
        /// <param name="result">
        /// The result of the execution.
        /// </param>
        void ProcessExecutionResult(WhatIfExecutionResult result);

        /// <summary>
        /// Sets the activity as the root activity for the current "What If" scenario.
        /// </summary>
        /// <param name="activity">
        /// The activity to set as the root.
        /// </param>
        void SetRootActivity(Activity activity);
    }
}
