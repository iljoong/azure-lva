using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LVADeskApp
{
    public class TextBoxWriter : TextWriter
    {
        // The control where we will write text.
        private Control MyControl;
        public TextBoxWriter(Control control)
        {
            MyControl = control;
        }

        public override void Write(char value)
        {
            // avoid cross thread issue: https://stackoverflow.com/questions/142003/cross-thread-operation-not-valid-control-accessed-from-a-thread-other-than-the
            MyControl.Invoke(new MethodInvoker(delegate {
                MyControl.Text += value;
            }));

            //MyControl.Text += value;
        }

        public override void Write(string value)
        {
            MyControl.Invoke(new MethodInvoker(delegate {
                MyControl.Text += value;
            }));

            //MyControl.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}
