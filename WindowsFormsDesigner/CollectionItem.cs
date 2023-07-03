using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsDesigner
{
    public class CollectionItem
    {
        private Control control;
        public Control Item { get { return control; }  set { control = value; } }
        public string Name = "";
        public CollectionItem()
        {

        }
        public CollectionItem(Control control)
        {
            this.control = control;
            if(control != null)
            {
                Name = control.Name;
            }
        }

        public override string ToString()
        {
            if (control != null)
            {
                return control.Name;
            }
            return Name;
        }
    }
}
