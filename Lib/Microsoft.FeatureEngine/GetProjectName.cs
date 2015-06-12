using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;

namespace Microsoft.FeatureEngine
{
    public class GetProjectName : CodeActivity<string>
    {
        protected override string Execute(CodeActivityContext context)
        {
            return DateTime.Now.ToString();
        }
    }
}
