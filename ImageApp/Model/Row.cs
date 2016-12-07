using ImageApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageApp.Helper;

namespace ImageApp.Model
{
    public class Row : IObject
    {
        public List<IObject> Children
        {
            get
            {
                return Children;
            }

            set
            {
                Children = new List<IObject>(value);
            }
        }
        public ObjectType Type
        {
            get
            {
                return ObjectType.Row;
            }
        }
    }
}
