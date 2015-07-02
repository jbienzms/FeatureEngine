using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class ItemTemplate : Metadata, IItemTemplate
    {
        public ItemTemplate()
        {
            InstallPath = string.Empty;
        }

        public string InstallPath { get; set; }
    }
}
