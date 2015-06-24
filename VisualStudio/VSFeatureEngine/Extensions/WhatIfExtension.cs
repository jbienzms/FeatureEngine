using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Activities;
using Microsoft.FeatureEngine.Workflow;

namespace VSFeatureEngine.Extensions
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

        public bool IsInWhatIfMode { get; set; }

        public ObservableCollection<WhatIfExecutionResult> Results { get; private set; } 
    }
}
