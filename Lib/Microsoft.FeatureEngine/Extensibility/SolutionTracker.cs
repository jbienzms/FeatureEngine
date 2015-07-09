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

        #region Constructors
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
        #endregion // Constructors

        #region Public Methods
        /// <summary>
        /// Checks to see if the active project has changed since the last time it was queried.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the active project has changed since the last time it was queried; otherwise false.
        /// </returns>
        public bool HasProjectChanged()
        {
            // Get the currently active project
            Project active = resolver.GetActiveProject();

            // Try to get the last project we accessed
            var lastActive = LastProject;

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
        #endregion // Public Methods


        #region Public Properties
        /// <summary>
        /// Gets the <see cref="Project"/> that was last accessed.
        /// </summary>
        public Project LastProject
        {
            get
            {
                Project lastActive = null;
                if (lastProjectWeak != null)
                {
                    lastProjectWeak.TryGetTarget(out lastActive);
                }
                return lastActive;
            }
        }
        #endregion // Public Properties
    }
}
