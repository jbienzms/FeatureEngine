using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public class Metadata : IMetadata
    {
        public Metadata()
        {
            Description = string.Empty;
            Id = string.Empty;
            Title = string.Empty;
        }

        public string Description { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

    }
}
