using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace VSFeatureEngine
{
    static internal class ThreadHelper
    {
        static private Dispatcher uiDispatcher;

        static public void SetUIDispatcher(Dispatcher dispatcher)
        {
            //// Validate
            //if (dispatcher == null) throw new ArgumentNullException("dispatcher");

            // Store
            uiDispatcher = dispatcher;
        }
        static public Dispatcher UIDispatcher
        {
            get
            {
                if (uiDispatcher == null)
                {
                    // Try to get the application
                    var app = Application.Current;

                    // Try to get the dispatcher
                    if (app != null)
                    {
                        uiDispatcher = app.Dispatcher;
                    }
                }
                return uiDispatcher;
            }
        }
    }
}
