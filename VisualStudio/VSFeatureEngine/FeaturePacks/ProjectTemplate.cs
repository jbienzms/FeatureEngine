using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class ProjectTemplate : Metadata, IProjectTemplate
    {
        public ProjectTemplate()
        {
            InstallPath = string.Empty;
        }

        public string InstallPath { get; set; }
    }
}
