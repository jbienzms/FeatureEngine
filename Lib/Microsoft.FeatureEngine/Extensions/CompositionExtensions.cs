using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;

namespace VSFeatureEngine
{
    static public class CompositionExtensions
    {
        static public T TryGetService<T>(this IComponentModel componentModel) where T:class
        {
            try
            {
                return componentModel.GetService<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
