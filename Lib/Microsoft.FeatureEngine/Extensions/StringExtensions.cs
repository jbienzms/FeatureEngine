using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    static public class StringExtensions
    {
        static public bool ContainsAny(this string source, IEnumerable<string> targets)
        {
            // Validate
            if (source == null) throw new ArgumentNullException("source");

            // If no targets, just say false
            if (targets == null) { return false; }

            // Search for any instance
            foreach (var target in targets)
            {
                if (source.Contains(target)) return true;
            }

            // None found
            return false;
        }
    }
}
