using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Activities;

namespace Microsoft.FeatureEngine.Workflow
{
    /// <summary>
    /// Represents the state of an activity if it were to execute in a non "What If" scenario.
    /// </summary>
    public enum WhatIfExecutionState
    {
        /// <summary>
        /// The activity would execute in a non what-if scenario.
        /// </summary>
        WouldExecute,

        /// <summary>
        /// The activity would not execute in a non what-if scenario.
        /// </summary>
        WouldNotExecute
    }

    /// <summary>
    /// Represents the result of a "What If" exeuction.
    /// </summary>
    public class WhatIfExecutionResult
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="WhatIfExecutionResult"/> instance.
        /// </summary>
        /// <param name="activity">
        /// The activity that completed execution.
        /// </param>
        /// <param name="state">
        /// The state of the execution.
        /// </param>
        public WhatIfExecutionResult(WhatIfActivity activity, WhatIfExecutionState state)
        {
            // Validate
            if (activity == null) throw new ArgumentNullException("activity");

            // Store
            this.Activity = activity;
            this.State = state;
        }
        #endregion // Constructors

        #region Public Properties
        /// <summary>
        /// The activity that completed execution.
        /// </summary>
        public WhatIfActivity Activity { get; private set; }

        /// <summary>
        /// The state of the execution.
        /// </summary>
        public WhatIfExecutionState State { get; private set; }
        #endregion // Public Properties
    }
}
