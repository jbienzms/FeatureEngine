using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public abstract class Action : Metadata, IAction
    {
        #region Constructors
        public Action(string id)
        {
            // Validate
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id");

            // Store
            this.Id = id;

            // Defaults
            Title = string.Empty;
            Description = string.Empty;
        }
        #endregion // Constructors

        #region Overridables / Event Triggers
        public abstract void Execute(IExecutionContext context);
        #endregion // Overridables / Event Triggers
    }
}
