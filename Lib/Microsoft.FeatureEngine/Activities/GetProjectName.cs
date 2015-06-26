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
        protected override WhatIfExecutionResult ExecuteWhatIf(CodeActivityContext context)
        {
            return new WhatIfExecutionResult(this, WhatIfExecutionState.Recommended);
        }

        protected override void ExecuteNonWhatIf(CodeActivityContext context)
        {
            ProjectName.Set(context, "Executed!!!");
        }

        public OutArgument<string> ProjectName { get; set; }
    }
}
