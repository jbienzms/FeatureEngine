using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace VSFeatureEngine
{
    [Export(typeof(ServiceStore))]
    [Export(typeof(IServiceStore))]
    public class ServiceStore : IServiceStore
    {
        #region Member Variables
        private IComponentModel componentModel;
        private IServiceContainer container;
        private IServiceProvider provider;
        #endregion // Member Variables

        public ServiceStore()
        {
        }

        public void Initialize(Package package)
        {
            // Validate
            if (package == null) throw new ArgumentNullException("package");

            // Store
            this.container = (IServiceContainer)package;
            this.provider = (IServiceProvider)package;

            // Try to get component model
            this.componentModel = provider.GetService(typeof(SComponentModel)) as IComponentModel;

            // Register with regular service container
            container.AddService(typeof(IServiceStore), this);
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
            if (service == null)
            {
                try
                {
                    service = provider.GetService(typeof(T)) as T;
                }
                catch (Exception) { }
            }

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
