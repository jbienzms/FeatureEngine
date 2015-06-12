using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Public interface for the feature manager.
    /// </summary>
    public interface IFeatureManager
    {
        /// <summary>
        /// Disables the package with the specified id.
        /// </summary>
        /// <param name="id">
        /// The ID of the package to disable.
        /// </param>
        Task DisablePackageAsync(string id);

        /// <summary>
        /// Enables a package at the specified path.
        /// </summary>
        /// <param name="packagePath">
        /// The path to the root of the package.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that represents the operation.
        /// </returns>
        Task EnablePackageAsync(string packagePath);
    }
}
