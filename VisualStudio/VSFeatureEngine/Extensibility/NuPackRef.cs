using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.VisualStudio;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Represnts a reference to a specific nuget package.
    /// </summary>
    /// <remarks>
    /// Nuget events and interfaces work with <see cref="IVsPackageMetadata"/>, but internally 
    /// Nuget may recreate the reference many times. This class provides equality with the interface 
    /// without actually requiring an object reference.
    /// </remarks>
    internal class NuPackRef
    {
        #region Operators
        public static bool operator ==(NuPackRef nuRef1, NuPackRef nuRef2)
        {
            if ((((object)nuRef1) == null) && (((object)nuRef2) == null))
            {
                return true;
            }
            else if (((object)nuRef1) != null)
            {
                return nuRef1.Equals(nuRef2);
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(NuPackRef nuRef1, NuPackRef nuRef2)
        {
            if ((((object)nuRef1) == null) && (((object)nuRef2) == null))
            {
                return false;
            }
            else if (((object)nuRef1) != null)
            {
                return !nuRef1.Equals(nuRef2);
            }
            else
            {
                return true;
            }
        }

        public static NuPackRef From(IVsPackageMetadata meta)
        {
            if (meta == null) { return null; }
            return new NuPackRef()
            {
                Id = meta.Id,
                VersionString = meta.VersionString
            };
        }

        #endregion // Operators


        #region Overrides / Event Handlers
        public override bool Equals(object obj)
        {
            // If same reference, shortcut
            if (Object.ReferenceEquals(this, obj)) { return true; }

            // Known interfaces
            var nuRef = obj as NuPackRef;
            var meta = obj as IVsPackageMetadata;

            // Is it a NuPackRef?
            if (nuRef != null)
            {
                return (string.Equals(nuRef.Id, this.Id) && string.Equals(nuRef.VersionString, this.VersionString));
            }

            // Is it a nuget package metadata?
            if (meta != null)
            {
                return (string.Equals(meta.Id, this.Id) && string.Equals(meta.VersionString, this.VersionString));
            }

            // Unknown object type or null
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 12;
            if (Id != null)
            {
                hash = (hash * 7) + Id.GetHashCode();
            }
            if (VersionString != null)
            {
                hash = (hash * 7) + VersionString.GetHashCode();
            }
            return hash;
        }
        #endregion // Overrides / Event Handlers


        #region Public Properties
        /// <summary>
        /// Id of the package.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The version of the package.
        /// </summary>
        public string VersionString { get; set; }
        #endregion // Public Properties
    }
}
