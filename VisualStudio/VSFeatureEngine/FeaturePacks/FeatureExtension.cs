using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VSFeatureEngine.FeaturePacks
{
    /// <summary>
    /// Represents an extension included in a feature pack.
    /// </summary>
    public class FeatureExtension
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="FeatureExtension"/>.
        /// </summary>
        /// <param name="id">
        /// The unique ID of the extension.
        /// </param>
        /// <param name="assembly">
        /// The assembly that contains the extension.
        /// </param>
        public FeatureExtension(string id, Assembly assembly)
        {
            // Validate
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id");
            if (assembly == null) throw new ArgumentNullException("assembly");

            // Store
            this.Id = id;
            this.Assembly = assembly;
        }
        #endregion // Constructors

        #region Public Properties
        /// <summary>
        /// Gets the assembly that contains the extension.
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Gets the unique ID of the extension.
        /// </summary>
        public string Id { get; private set; }
        #endregion // Public Properties
    }
}
