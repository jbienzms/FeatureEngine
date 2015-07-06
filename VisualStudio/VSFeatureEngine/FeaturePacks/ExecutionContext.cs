using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class ExecutionContext : IExecutionContext
    {
        public ExecutionContext(IServiceStore serviceStore)
        {
            // Validate
            if (serviceStore == null) throw new ArgumentNullException("serviceContainer");

            // Store
            this.ServiceStore = serviceStore;

            // Defualts
            IsInteractive = true;
        }

        public bool IsInteractive { get; set; }

        public IServiceStore ServiceStore { get; }
    }
}
