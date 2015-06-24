using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;

namespace VSFeatureEngine.FeaturePacks
{
    public abstract class FeatureAction : IFeatureAction
    {
        public FeatureAction(string name)
        {
            // Validate
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            // Store
            this.Name = name;
        }

        public abstract void Execute(IFeatureActionContext context);

        public string Name { get; private set; }
    }
}
