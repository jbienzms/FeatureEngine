using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio.ComponentModelHost;

namespace VSFeatureEngine.Services
{
    [Export(typeof(IServiceContainer))]
    public class ServiceContainer : IServiceContainer
    {
        #region Member Variables
        private IComponentModel componentModel;
        #endregion // Member Variables

        [ImportingConstructor]
        public ServiceContainer(IComponentModel componentModel)
        {
            // Validate
            if (componentModel == null) throw new ArgumentNullException("componentModel");

            // Store
            this.componentModel = componentModel;
        }
        
        public T GetService<T>() where T:class
        {
            return componentModel.GetService<T>();
        }
    }
}
