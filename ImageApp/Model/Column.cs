using ImageApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageApp.Helper;

namespace ImageApp.Model
{
    public class Column : IContainer,IObject 
    {
        public ObjectType Type
        {
            get
            {
                return ObjectType.Column;
            }
        }
    }
}
