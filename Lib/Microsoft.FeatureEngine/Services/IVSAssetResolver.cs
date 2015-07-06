using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Resolves assets from the Visual Studio environment.
    /// </summary>
    public interface IVSAssetResolver
    {
        /// <summary>
        /// Gets the currently active project.
        /// </summary>
        /// <returns>
        /// The currently active project.
        /// </returns>
        /// <remarks>
        /// The active project is usually the project selected in Solution Explorer 
        /// or the project that contains the currently active document.
        /// </remarks>
        Project GetActiveProject();
    }
}
