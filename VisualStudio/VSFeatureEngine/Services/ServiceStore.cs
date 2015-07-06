using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace VSFeatureEngine
{
    public class ServiceStore : IServiceStore
    {
        #region Member Variables
        private IComponentModel componentModel;
        private IServiceProvider provider;
        #endregion // Member Variables

        public ServiceStore(IServiceProvider provider)
        {
            // Validate
            if (provider == null) throw new ArgumentNullException("provider");

            // Store
            this.provider = provider;

            // Try to get component model
            this.componentModel = provider.GetService(typeof(SComponentModel)) as IComponentModel;
        }
        
        public T GetService<T>() where T:class
        {
            // Placeholder
            T service = null;

            // Try global service first
            try
            {
                service = Package.GetGlobalService(typeof(T)) as T;
            }
            catch (Exception) { }

            // Try regular service provider next
            try
            {
                service = provider.GetService(typeof(T)) as T;
            }
            catch (Exception) { }

            // Try MEF next
            if ((service == null) && (componentModel != null))
            {
                try
                {
                    service = componentModel.GetService<T>();
                }
                catch (Exception){}
            }

            // If not found in any source, service is missing
            if (service == null)
            {
                throw new MissingServiceException<T>();
            }

            // Service found
            return service;
        }
    }
}
