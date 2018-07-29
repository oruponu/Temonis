using System.Drawing;
using System.Windows.Forms;

namespace Temonis.Controls
{
    public class PictureBox : System.Windows.Forms.PictureBox
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

        public PictureBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            _borderColor = Resources.Util.Gray;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, _borderColor, ButtonBorderStyle.Solid);
        }
    }
}
