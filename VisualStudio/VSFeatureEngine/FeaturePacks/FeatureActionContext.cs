using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class FeatureActionContext : IFeatureActionContext
    {
        public FeatureActionContext(IServiceContainer serviceContainer)
        {
            // Validate
            if (serviceContainer == null) throw new ArgumentNullException("serviceContainer");

            // Store
            this.ServiceContainer = serviceContainer;

            // Defualts
            IsInteractive = true;
        }

        public bool IsInteractive { get; set; }

        public IServiceContainer ServiceContainer { get; }
    }
}
