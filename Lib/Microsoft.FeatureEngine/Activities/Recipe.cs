using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.FeatureEngine.ActivityDesigners;
using Microsoft.FeatureEngine.Workflow;

namespace Microsoft.FeatureEngine.Activities
{
    [ContentProperty("Activities")]
    [Designer(typeof(RecipeDesigner))]
    public class Recipe : NativeActivity, IFeatureActivity
    {
        
        #region Member Variables
        private Sequence innerSequence = new Sequence();
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="Recipe"/> instance.
        /// </summary>
        public Recipe()
        {
            // Defaults
            Description = string.Empty;
            IsEnabled = true;
            Title = string.Empty;
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddImplementationChild(innerSequence);
        }

        protected override void Execute(NativeActivityContext context)
        {
            // Try to get the "What If" extension from the context
            var whatIf = context.GetExtension<IWhatIfExtension>();

            // If found, notify that we're the root
            if (whatIf != null)
            {
                whatIf.SetRootActivity(this);
            }

            // Now, if we're enabled, carry out child tasks
            if (IsEnabled)
            {
                context.ScheduleActivity(innerSequence);
            }
        }
        #endregion // Overrides / Event Handlers

        #region Public Properties
        [Browsable(false)]
        [DependsOn("Variables")]
        public Collection<Activity> Activities
        {
            get
            {
                return innerSequence.Activities;
            }
        }

        /// <summary>
        /// Gets a description for the activity.
        /// </summary>
        /// <remarks>
        /// The description may be displayed in a user interface that allows the user to enable or disable the activity.
        /// </remarks>
        [DefaultValue("")]
        public string Description { get; set; }

        /// <summary>
        /// Indicates if the activity is enabled. The default is <c>true</c>.
        /// </summary>
        [DefaultValue(true)]
        [Description("Whether or not the recipe will execute.")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a link to a resource that provides more information about the activity.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        [Description("A link to a resource that provides more information about the recipe.")]
        public Link MoreInfoLink { get; set; }

        /// <summary>
        /// Gets a title for the activity.
        /// </summary>
        /// <remarks>
        /// The title may be displayed in a user interface that allows the user to enable or disable the activity.
        /// </remarks>
        [DefaultValue("")]
        public string Title { get; set; }


        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                return innerSequence.Variables;
            }
        }
        #endregion // Public Properties
    }
}
