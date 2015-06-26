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
        /// It is recommended that the activity be executed.
        /// </summary>
        Recommended,
        
        /// <summary>
        /// It is recommended that the activity NOT be executed.
        /// </summary>
        NotRecommended,

        /// <summary>
        /// The work performed by the activity is already completed so executing the activity has no effect.
        /// </summary>
        AlreadyCompleted,

        /// <summary>
        /// The activity must execute for the workflow to succeed.
        /// </summary>
        MustExecute,

        /// <summary>
        /// The activity cannot execute in the current context
        /// </summary>
        CannotExecute,
    }

    /// <summary>
    /// Represents the result of a "What If" exeuction.
    /// </summary>
    public class WhatIfExecutionResult
    {
        #region Static Version
        #region Public Methods
        /// <summary>
        /// Gets the default description for the specified state.
        /// </summary>
        /// <param name="state">
        /// The state to get the description for.
        /// </param>
        /// <returns>
        /// The default description
        /// </returns>
        static public string GetDefaultDescription(WhatIfExecutionState state)
        {
            switch (state)
            {
                case WhatIfExecutionState.AlreadyCompleted:
                    return "Already completed";
                case WhatIfExecutionState.CannotExecute:
                    return "Cannot execute";
                case WhatIfExecutionState.MustExecute:
                    return "Must execute";
                case WhatIfExecutionState.NotRecommended:
                    return "Not recommended";
                case WhatIfExecutionState.Recommended:
                    return "Recommended";
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
        /// <param name="state">
        /// The state of the execution.
        /// </param>
        /// <param name="description">
        /// A description for the execution result or <see langword="null"/> to use the default description.
        /// </param>
        public WhatIfExecutionResult(WhatIfActivity activity, WhatIfExecutionState state, string description)
        {
            // Validate
            if (activity == null) throw new ArgumentNullException("activity");

            // Store
            this.Activity = activity;
            this.State = state;

            // What to do about description
            if (!string.IsNullOrEmpty(description))
            {
                this.Description = description;
            }
            else
            {
                Description = GetDefaultDescription(state);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="WhatIfExecutionResult"/> instance.
        /// </summary>
        /// <param name="activity">
        /// The activity that completed execution.
        /// </param>
        /// <param name="state">
        /// The state of the execution.
        /// </param>
        public WhatIfExecutionResult(WhatIfActivity activity, WhatIfExecutionState state) : this(activity, state, null) { }

        #endregion // Constructors

        #region Public Properties
        /// <summary>
        /// The activity that completed execution.
        /// </summary>
        public WhatIfActivity Activity { get; private set; }

        /// <summary>
        /// Gets a description for the state of the execution.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The state of the execution.
        /// </summary>
        public WhatIfExecutionState State { get; private set; }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
