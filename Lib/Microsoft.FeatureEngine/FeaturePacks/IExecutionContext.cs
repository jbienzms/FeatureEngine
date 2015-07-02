using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Provides context about the environment during execution. Primarily used in <see cref="IAction.Execute"/>.
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        /// Gets a value that indicates if the action is being run in interactive mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if the action is being run in interactive mode; otherwise false.
        /// </value>
        /// <remarks>
        /// If this memeber returns true it means that the action is being performed in a context 
        /// where it is okay to present the user with interactive UI. If this member returns false 
        /// the action should execute without prompting the user.
        /// </remarks>
        bool IsInteractive { get; }

        /// <summary>
        /// Gets the service container for the action.
        /// </summary>
        IServiceContainer ServiceContainer { get; }
    }
}
