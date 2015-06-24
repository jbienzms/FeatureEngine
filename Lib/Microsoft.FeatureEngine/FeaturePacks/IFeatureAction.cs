using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// The interface for an action that can be performed as part of a feature.
    /// </summary>
    public interface IFeatureAction
    {
        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="context">
        /// The context for the operation.
        /// </param>
        void Execute(IFeatureActionContext context);
    }
}
