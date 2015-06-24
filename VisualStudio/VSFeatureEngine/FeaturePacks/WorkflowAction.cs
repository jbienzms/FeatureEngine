using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class WorkflowAction : FeatureAction
    {
        private Activity activity;
        private WorkflowApplication wfApp;

        public WorkflowAction(string name, Activity activity) : base(name)
        {
            // Validate
            if (activity == null) throw new ArgumentNullException("activity");

            // Store
            this.activity = activity;
        }

        private void EnsureWFInitialized()
        {
            if (wfApp != null) { return; }

            wfApp = new WorkflowApplication(activity);
        }

        public override void Execute(IFeatureActionContext context)
        {
            // Ensure that workflow has been initialized
            EnsureWFInitialized();
        }
    }
}
