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
            Project[] projects = dte.ActiveSolutionProjects;

            // Make sure we got an array and it has at least one item
            if ((projects != null) && (projects.Length > 0))
            {
                return projects[0];
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

            // Get the project item
            var projItem = doc.ProjectItem;

            // Make sure we got the project item

            // Get the containing project
            return projItem.ContainingProject;
        }

        protected override void Execute(CodeActivityContext context)
        {
            // Get EnvDTE as a service
            var dte = GetService<DTE>(context);

            // Make sure we got the DTE

            
            // Make sure we got the project

            // dte.Solution.ac
            // Microsoft.CodeAnalysis.Workspace.GetWorkspaceRegistration()
            //// Get the workspace
            //var workspace = GetService<VisualStudioWorkspace>(context);
            
            //// Get the active project
            //workspace.GetHierarchy()

        }

        public OutArgument<Project> Project { get; set; }
    }
}
