using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FeatureEngine;
using Microsoft.VisualStudio.Shell;

namespace VSFeatureEngine
{
    public abstract class ListDynamicMenu<T> : DynamicMenu<T>
    {
        public ListDynamicMenu(Package package, Guid menuGroup, int startId) : base(package, menuGroup, startId) { }

        protected override T GetItem(int idx)
        {
            return Items[idx];
        }

        protected override bool IsInRange(int idx)
        {
            return ((idx > -1) && (idx < Items.Count));
        }

        protected abstract IList<T> Items { get; }
    }
}