using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Activities;
using Microsoft.FeatureEngine.Workflow;

namespace VSFeatureEngine.Workflow
{
    public class WhatIfExtension : IWhatIfExtension
    {
        public WhatIfExtension()
        {
            Results = new ObservableCollection<WhatIfExecutionResult>();
        }

        public void ProcessExecutionResult(WhatIfExecutionResult result)
        {
            if (result == null) throw new ArgumentNullException("result");
            Results.Add(result);
        }

        public void SetRootActivity(Activity activity)
        {
            if (activity == null) throw new ArgumentNullException("activity");
            RootActivity = activity;
        }

        public bool IsInWhatIfMode { get; set; }

        public ObservableCollection<WhatIfExecutionResult> Results { get; private set; } 

        public Activity RootActivity { get; private set; }
    }
}
