using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Activities;
using Microsoft.FeatureEngine.Workflow;
using VSFeatureEngine.ViewModels;

namespace VSFeatureEngine.Workflow
{
    public class WhatIfExtension : IWhatIfExtension
    {
        #region Member Variables
        private RunRecipeViewModel viewModel;
        #endregion // Member Variables

        #region Constructors
        public WhatIfExtension()
        {
            // Init
            viewModel = new RunRecipeViewModel();
        }
        #endregion // Constructors

        public void ProcessExecutionResult(WhatIfExecutionResult result)
        {
            if (result == null) throw new ArgumentNullException("result");
            viewModel.ExecutionResults.Add(result);
        }

        public void SetRootActivity(Activity activity)
        {
            if (activity == null) throw new ArgumentNullException("activity");
            viewModel.RootActivity = activity as IFeatureActivity;
        }

        public bool IsInWhatIfMode { get; set; }

        public RunRecipeViewModel ViewModel { get { return viewModel; } }
    }
}
