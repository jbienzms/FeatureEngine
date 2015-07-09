using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSFeatureEngine
{
    /// <summary>
    /// Helper functions for working with Visual Studio
    /// </summary>
    internal class VSUtil
    {
        #region Member Variables
        private IVsSolution solution;
        #endregion // Member Variables

        #region Constructors
        public VSUtil(IServiceStore serviceStore)
        {
            // Validate
            if (serviceStore == null) throw new ArgumentNullException("serviceStore");

            // Get services
            solution = (IVsSolution)serviceStore.GetService<SVsSolution>();
        }
        #endregion // Constructors

        #region Public Methods
        public Guid GetProjectGuid(Project project)
        {
            // Validate
            if (project == null) throw new ArgumentNullException("project");

            IVsHierarchy hierarchy;
            solution.GetProjectOfUniqueName(project.FullName, out hierarchy);
            if (hierarchy != null)
            {
                Guid projectGuid;

                ErrorHandler.ThrowOnFailure(hierarchy.GetGuidProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ProjectIDGuid, out projectGuid));

                if (projectGuid != null)
                {
                    return projectGuid;
                }
            }

            throw new InvalidOperationException(Strings.CouldNotObtainProjectGUID);
        }
        #endregion // Public Methods
    }
}
