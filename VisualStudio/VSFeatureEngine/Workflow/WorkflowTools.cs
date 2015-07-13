using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFeatureEngine.Workflow
{
    static internal class WorkflowTools
    {
        static public void CompileExpressions(Activity activity)
        {
            // Validate
            if (activity == null) throw new ArgumentNullException("activity");
            var dynamicActivity = activity as DynamicActivity;

            // Get the activity type
            var aType = activity.GetType();

            // If type implements ICompiledExpressionRoot it already has expressions compiled
            if (typeof(ICompiledExpressionRoot).IsAssignableFrom(aType))
            {
                return;
            }

            // activityName is the Namespace.Type of the activity that contains the
            // C# expressions.
            string activityName = (dynamicActivity != null ? dynamicActivity.Name : aType.ToString());

            // Split activityName into Namespace and Type.Append _CompiledExpressionRoot to the type name
            // to represent the new type that represents the compiled expressions.
            // Take everything after the last . for the type name.
            string activityType = activityName.Split('.').Last() + "_CompiledExpressionRoot";
            // Take everything before the last . for the namespace.
            string activityNamespace = string.Join(".", activityName.Split('.').Reverse().Skip(1).Reverse());

            // Create a TextExpressionCompilerSettings.
            TextExpressionCompilerSettings settings = new TextExpressionCompilerSettings
            {
                Activity = activity,
                Language = "C#",
                ActivityName = activityType,
                ActivityNamespace = activityNamespace,
                RootNamespace = null,
                GenerateAsPartialClass = false,
                AlwaysGenerateSource = true,
                ForImplementation = (dynamicActivity != null)
            };

            // Compile the C# expression.
            TextExpressionCompilerResults results =
                new TextExpressionCompiler(settings).Compile();

            // Any compilation errors are contained in the CompilerMessages.
            if (results.HasErrors)
            {
                throw new InvalidOperationException("Expression compilation failed.");
            }

            // Create an instance of the new compiled expression type.
            ICompiledExpressionRoot compiledExpressionRoot =
                Activator.CreateInstance(results.ResultType,
                    new object[] { activity }) as ICompiledExpressionRoot;

            // Attach it to the activity.
            if (dynamicActivity != null)
            {
                CompiledExpressionInvoker.SetCompiledExpressionRootForImplementation(dynamicActivity, compiledExpressionRoot);
            }
            else
            {
                CompiledExpressionInvoker.SetCompiledExpressionRoot(activity, compiledExpressionRoot);
            }
        }
    }
}
