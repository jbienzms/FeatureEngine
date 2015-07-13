using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
// using GalaSoft.MvvmLight;
using Microsoft.FeatureEngine.Activities;
using Microsoft.FeatureEngine.Workflow;

namespace VSFeatureEngine.ViewModels
{
    public class RunRecipeViewModel : ObservableObject
    {
        #region Member Variables
        private ObservableCollection<WhatIfExecutionResult> executionResults = new ObservableCollection<WhatIfExecutionResult>();
        private IFeatureActivity rootActivity;
        #endregion // Member Variables

        /// <summary>
        /// Gets or sets the executionResults of the <see cref="RunRecipeViewModel"/>.
        /// </summary>
        /// <value>
        /// The executionResults of the <c>RunRecipeViewModel</c>.
        /// </value>
        public ObservableCollection<WhatIfExecutionResult> ExecutionResults
        {
            get { return executionResults; }
            set { Set(ref executionResults, value); }
        }

        private WhatIfExecutionResult selectedExecutionResult;
        /// <summary>
        /// Gets or sets the selectedExecutionResult of the <see cref="RunRecipeViewModel"/>.
        /// </summary>
        /// <value>
        /// The selectedExecutionResult of the <c>RunRecipeViewModel</c>.
        /// </value>
        public WhatIfExecutionResult SelectedExecutionResult
        {
            get { return selectedExecutionResult; }
            set { Set(ref selectedExecutionResult, value); }
        }

        /// <summary>
        /// Gets or sets the rootActivity of the <see cref="RunRecipeViewModel"/>.
        /// </summary>
        /// <value>
        /// The rootActivity of the <c>RunRecipeViewModel</c>.
        /// </value>
        public IFeatureActivity RootActivity
        {
            get { return rootActivity; }
            set { Set(ref rootActivity, value); }
        }
    }
}
