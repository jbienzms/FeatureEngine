using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.PlatformUI;

namespace VSFeatureEngine
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ManageFeaturesDialog : DialogWindow
    {
        #region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageFeaturesDialog"/> class.
        /// </summary>
        public ManageFeaturesDialog()
        {
            this.HasMaximizeButton = true;
            this.HasMinimizeButton = true;

            InitializeComponent();
        }
        #endregion
    }
}
