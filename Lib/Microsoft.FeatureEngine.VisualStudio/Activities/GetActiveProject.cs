using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.FeatureEngine.Activities;

namespace Microsoft.FeatureEngine.VisualStudio.Activities
{
    /// <summary>
    /// Gets the currently active project in the Visual Studio environment.
    /// </summary>
    /// <remarks>
    /// This activity does not inherit from WhatIfActivity because it must execute in WhatIf and non-WhatIf scenarios.
    /// </remarks>
    public class GetActiveProject : FeatureActivity
    {
        private Project GetBySelection(DTE dte)
        {
            // Get project array
            Array projs = dte.ActiveSolutionProjects as Array;

            // Make sure we got an array and it has at least one item
            if ((projs != null) && (projs.Length > 0))
            {
                return projs.GetValue(0) as Project;
            }
            else
            {
                return null;
            }
        }

        private Project GetByOpenDocument(DTE dte)
        {
            // Get the active document
            var doc = dte.ActiveDocument;

            // Make sure we got the document
            if (doc == null) { return null; }

            // Get the project item
            var projItem = doc.ProjectItem;

            // Make sure we got the project item
            if (projItem == null) { return null; }

            // Get the containing project
            return projItem.ContainingProject;
        }

        protected override void Execute(CodeActivityContext context)
        {
            // Get EnvDTE as a service
            var dte = GetService<DTE>(context);

            // Make sure we got the DTE
            if (dte == null) { throw new MissingServiceException<DTE>(); }

            // Try to get by selection
            var proj = GetBySelection(dte);

            // If not found, try to get by active document
            if (proj == null)
            {
                proj = GetByOpenDocument(dte);
            }

            // If still not found, task failed
            if (proj == null)
            {
                throw new ActivityFailureException(Strings.FailureFindingActiveProject);
            }

            // Success
            Project.Set(context, proj);
        }

        public OutArgument<Project> Project { get; set; }
    }
}
