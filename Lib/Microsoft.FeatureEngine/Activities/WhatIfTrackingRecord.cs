using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine.Activities
{
    /// <summary>
    /// A custom tracking record for activities that inherit from <see cref="WhatIfActivity"/>.
    /// </summary>
    public class WhatIfTrackingRecord : CustomTrackingRecord
    {
        #region Constants
        private const string RecordName = "WhatIf";
        private const string ResultValueName = "ExecutionResult";
        #endregion // Constants


        #region Constructors
        public WhatIfTrackingRecord(WhatIfExecutionResult executionResult) : base(RecordName)
        {
            ExecutionResult = executionResult;
            Data[ResultValueName] = executionResult.ToString();
        }
        #endregion // Constructors


        #region Public Properties
        /// <summary>
        /// Gets the execution result of the tracking record.
        /// </summary>
        public WhatIfExecutionResult ExecutionResult { get; private set; }
        #endregion // Public Properties
    }
}
