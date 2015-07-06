using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    static public class ProjectKind
    {
        public const string Cordova = "Cordova";
        public const string CSharp = "CSharp";
        public const string VisualBasic = "VisualBasic";
        public const string WinJS = "WinJS";
    }

    public class ProjectInfo
    {
        public ProjectInfo()
        {
            // PrjKind
        }

        /// <summary>
        /// Gets the kind of project (e.g. CSharp, VisualBasic, etc.)
        /// </summary>
        /// <remarks>
        /// This member is interpreted from Project.Kind and the ProjKind enumeration. 
        /// Well-known values for this member can be found in the <see cref="ProjectKind"/> 
        /// static class.
        /// </remarks>
        public string Kind { get; set; }

        /// <summary>
        /// Gets the platform target for the project. 
        /// </summary>
        public string PlatformTarget { get; set; }
    }
}
