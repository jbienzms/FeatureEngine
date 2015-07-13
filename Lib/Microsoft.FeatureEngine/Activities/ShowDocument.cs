using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine.Workflow;

namespace Microsoft.FeatureEngine.Activities
{
    public class ShowDocument : WhatIfActivity
    {
        static private readonly string[] exeExtensions = { ".exe", ".bat", ".cmd", ".ps", ".ps1", ".ps2", ".vbs"};
        static private void ValidateUrl(string url)
        {
            // Test for empty or null
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException(Strings.UrlCannotBeEmpty);
            }

            // Test for executables

            if (url.ToLower().ContainsAny(exeExtensions))
            {
                throw new InvalidOperationException(Strings.ExecutablesNotSupported);
            }
        }

        protected override void ExecuteNonWhatIf(CodeActivityContext context)
        {
            // Get the url
            var url = Url.Get(context);

            // Validate
            ValidateUrl(url);

            // Show the url
            System.Diagnostics.Process.Start(url);
        }

        protected override WhatIfExecutionResult ExecuteWhatIf(CodeActivityContext context)
        {
            return new WhatIfExecutionResult(this, WhatIfRecommendation.Recommended, "It's always good to view documentation.");
        }

        [RequiredArgument]
        public InArgument<string> Url { get; set; }
    }
}
