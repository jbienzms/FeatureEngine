using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSLangProj;
using VSLangProj2;
using VSLangProj80;
using VslangProj90;
using VslangProj100;
using VSLangProj110;
using VSLangProj140;
using System.Activities;
using EnvDTE;

namespace Microsoft.FeatureEngine.Activities
{
    public class GetProjectInfo : FeatureActivity
    {

        protected override void Execute(CodeActivityContext context)
        {
            // Get the project value
            var proj = Project.Get(context);

            // Get the configuration
            var config = proj.ConfigurationManager.ActiveConfiguration;

            // Get the properties
            var props = config.Properties;

            // Create our return value
            var info = new ProjectInfo();

            // Get project kind
            var kind = proj.Kind;
            switch (kind)
            {
                case PrjKind.prjKindCSharpProject:
                    info.Kind = ProjectKind.CSharp;
                    break;
                case PrjKind.prjKindVBProject:
                    info.Kind = ProjectKind.VisualBasic;
                    break;
                default:
                    info.Kind = kind;
                    break;
            }
            
            // Get platform target
            var platTarget = props.Item("PlatformTarget");
            if (platTarget != null)
            {
                info.PlatformTarget = platTarget.Value.ToString();
            }

            // Store our return value
            Info.Set(context, info);
        }

        [RequiredArgument]
        public InArgument<Project> Project { get; set; }

        public OutArgument<ProjectInfo> Info { get; set; }
    }
}
