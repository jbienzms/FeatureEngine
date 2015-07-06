using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides data for events that relate to feture pack associations.
    /// </summary>
    public class FeaturePackAssociationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new <see cref="FeaturePackAssociationEventArgs"/> instance.
        /// </summary>
        /// <param name="package">
        /// The package that the event relates to.
        /// </param>
        /// <param name="project">
        /// The project the event relates to.
        /// </param>
        public FeaturePackAssociationEventArgs(IFeaturePack package, Project project)
        {
            // Validate
            if (package == null) throw new ArgumentNullException("package");
            if (project == null) throw new ArgumentNullException("project");

            // Store
            Package = package;
            Project = project;
        }

        /// <summary>
        /// Gets the package that the event relates to.
        /// </summary>
        public IFeaturePack Package { get; private set; }

        /// <summary>
        /// Gets the project that the event relates to.
        /// </summary>
        public Project Project { get; private set; }
    }
}
