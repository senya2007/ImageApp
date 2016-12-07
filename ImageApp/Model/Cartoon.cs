using ImageApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageApp.Helper;
using System.Windows.Controls;

namespace ImageApp.Model
{
    public class Cartoon : IObject
    {
        public Image Link { get; set; }

        public ObjectType ParentType { get; set; }

        public ObjectType Type
        {
            get
            {
                return ObjectType.Image;
            }
        }
    }
}
