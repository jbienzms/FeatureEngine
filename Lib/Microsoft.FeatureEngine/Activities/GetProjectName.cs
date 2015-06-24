using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.FeatureEngine.Workflow;

namespace Microsoft.FeatureEngine.Activities
{
    public class GetProjectName : FeatureActivity
    {

        protected override WhatIfExecutionState ExecuteWhatIf(CodeActivityContext context)
        {
            return WhatIfExecutionState.WouldExecute;
        }

        protected override void ExecuteNonWhatIf(CodeActivityContext context)
        {
            ProjectName.Set(context, "Executed!!!");
        }

        public OutArgument<string> ProjectName { get; set; }
    }
}
