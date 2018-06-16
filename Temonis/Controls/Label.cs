using System;
using System.Windows.Forms;

namespace Temonis.Controls
{
    internal class Label : System.Windows.Forms.Label
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 515)
            {
                OnDoubleClick(new EventArgs());
                return;
            }

            base.WndProc(ref m);
        }
    }
}
