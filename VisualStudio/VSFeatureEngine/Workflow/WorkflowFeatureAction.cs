using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.FeatureEngine;
using Microsoft.FeatureEngine.Workflow;
using VSFeatureEngine.FeaturePacks;
using VSFeatureEngine.ViewModels;

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
        private IExecutionContext executionContext;
        private IServiceStore serviceStore;
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
        private void EditAndRunRecipe(RunRecipeViewModel viewModel)
        {
            // Must run from the UI thread
            ThreadHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                // Create and show the dialog
                var dlg = new RunRecipeDialog();
                dlg.DataContext = viewModel;
                var m = dlg.ShowModal();

                // If user canceled, bail
                if (m != true) { return; }

                // User didn't bail. Run again with updated definition and not in what-if mode.
                RunRecipe(false);
            }));
        }

        private void RunRecipe(bool whatIf)
        {
            // Make sure we have the activity instantiated
            if (activity == null)
            {
                // Create the activity
                activity = (Activity)Activator.CreateInstance(activityType);
            }

            // Create workflow app
            var wfApp = new WorkflowApplication(activity);

            // Wire up events
            wfApp.Aborted = OnAborted;
            wfApp.Completed = OnCompleted;
            wfApp.OnUnhandledException = OnUnhandledException;
            wfApp.PersistableIdle = OnPersistableIdle;
            wfApp.Unloaded = OnUnloaded;

            // Create extensions
            var whatIfExtension = new WhatIfExtension() { IsInWhatIfMode = whatIf };

            // Add extensions
            wfApp.Extensions.Add<IServiceStore>(() => serviceStore);
            wfApp.Extensions.Add<IWhatIfExtension>(() => whatIfExtension);

            // Run it!
            wfApp.Run();
        }
        #endregion // Internal Methods

        #region Overridables / Event Triggers
        protected virtual void OnAborted(WorkflowApplicationAbortedEventArgs e)
        {

        }

        protected virtual void OnCompleted(WorkflowApplicationCompletedEventArgs e)
        {
            // If it's not done yet, ignore
            if (e.CompletionState != ActivityInstanceState.Closed) { return; }

            // Try to get the WhatIf extension
            var whatIfExtension = e.GetInstanceExtensions<IWhatIfExtension>().FirstOrDefault() as WhatIfExtension;

            // If the recipe was running in What-If mode, allow for edits and possibly run again
            if ((whatIfExtension != null) && (whatIfExtension.IsInWhatIfMode))
            {
                EditAndRunRecipe(whatIfExtension.ViewModel);
            }
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
            // Store the context
            executionContext = context;

            // Update the reference to the service store in case it changes
            serviceStore = context.ServiceStore;

            // Run the recipe. If we're in interactive, we'll run it with What-If.
            RunRecipe(context.IsInteractive);
        }
        #endregion // Overrides / Event Handlers
    }
}
