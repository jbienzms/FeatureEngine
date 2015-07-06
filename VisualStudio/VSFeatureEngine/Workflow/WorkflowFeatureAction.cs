using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
using Microsoft.FeatureEngine.Workflow;
using VSFeatureEngine.FeaturePacks;

namespace VSFeatureEngine.Workflow
{
    /// <summary>
    /// A <see cref="FeatureAction"/> that executes a workflow.
    /// </summary>
    public class WorkflowFeatureAction : FeatureAction
    {
        #region Member Variables
        private Activity activity;
        private Type activityType;
        private IServiceStore serviceStore;
        private WhatIfExtension whatIfExtension;
        private WorkflowApplication wfApp;
        #endregion // Member Variables


        #region Constructors
        public WorkflowFeatureAction(string id, Type activityType) : base(id)
        {
            // Validate
            if (activityType == null) throw new ArgumentNullException("activityType");
            if (!(typeof(Activity).IsAssignableFrom(activityType)))
            {
                var msg = string.Format("The type {0} in does not inherit from {1}", activityType.Name, typeof(Activity).Name);
                throw new ArgumentException(msg, "activityType");
            }
            
            // Store
            this.activityType = activityType;
        }
        #endregion // Constructors

        #region Internal Methods
        private void EnsureWFInitialized()
        {
            if (wfApp != null) { return; }

            // Create the activity
            activity = (Activity)Activator.CreateInstance(activityType);

            // Create workflow app
            wfApp = new WorkflowApplication(activity);

            // Wire up events
            wfApp.Aborted = OnAborted;
            wfApp.Completed = OnCompleted;
            wfApp.OnUnhandledException = OnUnhandledException;
            wfApp.PersistableIdle = OnPersistableIdle;
            wfApp.Unloaded = OnUnloaded;

            // Create extensions
            whatIfExtension = new WhatIfExtension();

            // Add extensions
            wfApp.Extensions.Add<IServiceStore>(() => serviceStore);
            wfApp.Extensions.Add<IWhatIfExtension>(() => whatIfExtension);
        }
        #endregion // Internal Methods

        #region Overridables / Event Triggers
        protected virtual void OnAborted(WorkflowApplicationAbortedEventArgs e)
        {

        }

        protected virtual void OnCompleted(WorkflowApplicationCompletedEventArgs e)
        {

        }

        protected virtual PersistableIdleAction OnPersistableIdle(WorkflowApplicationIdleEventArgs e)
        {
            return PersistableIdleAction.None;
        }

        protected virtual UnhandledExceptionAction OnUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs e)
        {
            return UnhandledExceptionAction.Terminate;
        }

        private void OnUnloaded(WorkflowApplicationEventArgs obj)
        {

        }
        #endregion // Overridables / Event Triggers

        #region Overrides / Event Handlers
        public override void Execute(IExecutionContext context)
        {
            // Update the reference to the service store in case it changes
            serviceStore = context.ServiceStore;

            // Ensure that workflow has been initialized
            EnsureWFInitialized();

            // Interactive mode?
            if (false) // context.IsInteractive)
            {
                // Run the workflow in what-if mode
                whatIfExtension.IsInWhatIfMode = true;
                wfApp.Run();

                // TODO: Show UI

                // TODO: Allow enable / disable

                // Run the workflow again in non what-if mode
                whatIfExtension.IsInWhatIfMode = false;
                wfApp.Run();
            }
            else
            {
                // Run the workflow
                wfApp.Run();
            }
        }
        #endregion // Overrides / Event Handlers
    }
}
