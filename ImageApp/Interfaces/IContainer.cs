using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.Interfaces
{
    public abstract class IContainer
    {
        public List<IObject> Children = new List<IObject>();

        public void Add(IObject item)
        {
            if (item != null)
            {
                Children.Add(item);
            }
        }
    }
}
