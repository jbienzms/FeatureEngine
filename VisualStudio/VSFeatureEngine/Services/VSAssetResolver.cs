using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Export(typeof(IVSAssetResolver))]
    public class VSAssetResolver : IVSAssetResolver
    {
        #region Member Variables
        private IServiceStore serviceStore;
        private DTE dte;
        #endregion // Member Variables

        [ImportingConstructor]
        public VSAssetResolver(IServiceStore serviceStore)
        {
            // Validate
            if (serviceStore == null) throw new ArgumentNullException("serviceStore");

            // Get additional services
            dte = serviceStore.GetService<DTE>();
        }

        private Project GetProjectBySelection()
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

        private Project GetProjectByOpenDocument()
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


        public Project GetActiveProject()
        {
            // Try to get by selection
            var proj = GetProjectBySelection();

            // If not found, try to get by active document
            if (proj == null)
            {
                proj = GetProjectByOpenDocument();
            }

            // Return whatever was found
            return proj;
        }
    }
}
