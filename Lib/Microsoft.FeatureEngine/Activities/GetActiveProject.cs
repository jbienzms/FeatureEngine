using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Microsoft.FeatureEngine.Activities
{
    /// <summary>
    /// Gets the currently active project in the Visual Studio environment.
    /// </summary>
    /// <remarks>
    /// This activity does not inherit from WhatIfActivity because it must execute in WhatIf and non-WhatIf scenarios.
    /// </remarks>
    public class GetActiveProject : FeatureActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            // Get EnvDTE as a service
            var resolver = GetService<IVSAssetResolver>(context);

            // Try to get the active project
            var proj = resolver.GetActiveProject();

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
