using System;
using System.Drawing;
using System.Windows.Forms;

namespace Temonis.Controls
{
    internal class DataGridView : System.Windows.Forms.DataGridView
    {
        private Color _borderColor;

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (BorderColor == value) return;
                _borderColor = value;
                Refresh();
            }
        }

        public DataGridView()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
            _borderColor = Resources.Util.Gray;

            foreach (var control in Controls)
            {
                if (!(control is VScrollBar vScrollBar)) continue;
                vScrollBar.Visible = true;
                vScrollBar.VisibleChanged += ScrollBar_VisibleChanged;
            }
        }

        private void ScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            var vScrollBar = (VScrollBar)sender;
            if (vScrollBar.Visible) return;
            vScrollBar.Location = new Point(ClientRectangle.Width - vScrollBar.Width, 0);
            vScrollBar.Size = new Size(vScrollBar.Width, ClientRectangle.Height);
            vScrollBar.Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, _borderColor, ButtonBorderStyle.Solid);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            e = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta / SystemInformation.MouseWheelScrollLines);
            base.OnMouseWheel(e);
        }
    }
}
