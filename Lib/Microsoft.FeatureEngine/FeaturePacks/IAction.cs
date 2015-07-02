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
    public interface IAction : IMetadata
    {
        #region Public Properties
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="context">
        /// The context for the operation.
        /// </param>
        void Execute(IExecutionContext context);
        #endregion // Public Properties
    }
}
