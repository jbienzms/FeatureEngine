using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// A helper class that can be used to track items in a solution.
    /// </summary>
    internal class SolutionTracker
    {
        #region Member Variables
        private WeakReference<Project> lastProjectWeak;
        private IVSAssetResolver resolver;
        #endregion // Member Variables

        /// <summary>
        /// Initializes a new <see cref="SolutionTracker"/> instance.
        /// </summary>
        /// <param name="resolver">
        /// The <see cref="IVSAssetResolver"/> used to resolve solution assets.
        /// </param>
        public SolutionTracker(IVSAssetResolver resolver)
        {
            // Validate
            if (resolver == null) throw new ArgumentNullException("resolver");

            // Store
            this.resolver = resolver;
        }

        /// <summary>
        /// Checks to see if the active project has changed since the last time it was queried.
        /// </summary>
        /// <param name="active">
        /// The currently active project.
        /// </param>
        /// <returns>
        /// <c>true</c> if the active project has changed since the last time it was queried; otherwise false.
        /// </returns>
        public bool QueryProjectChanged(out Project active)
        {
            // Get the currently active project
            active = resolver.GetActiveProject();

            // Try to get the last active project
            Project lastActive = null;
            if (lastProjectWeak != null)
            {
                lastProjectWeak.TryGetTarget(out lastActive);
            }

            // Changed?
            if (active != lastActive)
            {
                // Update last
                lastProjectWeak = (active != null ? new WeakReference<Project>(active) : null);

                // Changed
                return true;
            }
            else
            {
                // Didn't change
                return false;
            }
        }
    }
}
