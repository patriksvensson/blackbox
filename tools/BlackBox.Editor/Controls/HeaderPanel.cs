//
// Copyright 2011 Patrik Svensson
//
// This file is part of BlackBox.
//
// BlackBox is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BlackBox is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License
// along with BlackBox. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace BlackBox.Editor
{
	public class HeaderPanel : Panel
	{
		private string m_title;
		private Rectangle m_previousClientSize;
		private int m_innerPadding;
		private Color m_gradientStartColor = Color.White;
		private Color m_gradientEndColor = Color.CornflowerBlue;
		private LinearGradientMode m_gradientMode = LinearGradientMode.ForwardDiagonal;
		private Image m_image;
		private bool m_drawGradient = false;

        #region Properties

        [Browsable(true)]
		public Color GradientStartColor
		{
			get { return m_gradientStartColor; }
			set
			{
				m_gradientStartColor = value;
				this.Invalidate();
			}
		}

		[Browsable(true)]
		public bool DrawGradient
		{
			get { return m_drawGradient; }
			set
			{
				m_drawGradient = value;
				this.Invalidate();
			}
		}

		[Browsable(true)]
		public int InnerPadding
		{
			get
			{
				return m_innerPadding;
			}
			set
			{
				m_innerPadding = value;
				this.Invalidate();
			}
		}

		[Browsable(true)]
		public Image Icon
		{
			get { return m_image; }
			set
			{
				// Set the image.
				m_image = value;
				// Invalidate the control.
				this.Invalidate();
			}
		}


		[Browsable(true)]
		public Color GradientEndColor
		{
			get { return m_gradientEndColor; }
			set
			{
				m_gradientEndColor = value;
				this.Invalidate();
			}
		}

		[Browsable(true)]
		public LinearGradientMode GradientMode
		{
			get { return m_gradientMode; }
			set
			{
				m_gradientMode = value;
				this.Invalidate();
			}
		}

		[Browsable(true)]
		public string Title
		{
			get
			{
				if (m_title == null)
					m_title = this.Name;
				return m_title;
			}
			set
			{
				m_title = value;
				this.Invalidate();
			}
		}

        #endregion

        public HeaderPanel()
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// Get the graphics object.
			Graphics g = e.Graphics;

			// Get the colors.
			Color startColor = this.GradientStartColor;
			Color endColor = this.GradientEndColor;
			Color fontColor = this.ForeColor;

			// Is the control disabled?
			if (!this.Enabled)
			{
				startColor = this.MakeColorGrayScale(startColor);
				endColor = this.MakeColorGrayScale(endColor);
				fontColor = this.MakeColorGrayScale(fontColor);
			}

			// Create the brush.
			Brush brush = null;

			// Are we drawing the gradient or not?
			if (this.DrawGradient == true)
				brush = new LinearGradientBrush(this.ClientRectangle, startColor, endColor, this.GradientMode);
			else
				brush = new SolidBrush(this.BackColor);

			// Draw the rectangle.
			g.FillRectangle(brush, this.ClientRectangle);

			// Should we draw the image?
			bool drawImage = (this.Icon == null) ? false : true;
			if (drawImage)
			{
				// Is the control disabled?
				if (!this.Enabled)
				{
					// Draw an disabled icon.
					ControlPaint.DrawImageDisabled(g, this.Icon, 12,
						(this.Height - this.Icon.Height) / 2, Color.Transparent);
				}
				else
				{
					// Draw the regular icon.
					g.DrawImage(this.Icon, 12, (this.Height - this.Icon.Height) / 2);
				}
			}

			// Draw the text.
			SizeF size = g.MeasureString(this.Title, this.Font);
			g.DrawString(this.Title, this.Font, new SolidBrush(fontColor),
				(drawImage) ? this.Icon.Width + 12 + 5 : 12,
				((this.Height - size.Height) / 2));

			// Draw the border.			
			ControlPaint.DrawBorder3D(g, this.ClientRectangle, Border3DStyle.Etched, Border3DSide.Bottom);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			// If the previous client size is null, assign it.
			if (m_previousClientSize == null)
				m_previousClientSize = this.ClientRectangle;

			// If the current client size is smaller than or equal to 
			// the previous client size, don't do anything at all.
			if (this.ClientRectangle.Width <= m_previousClientSize.Width)
				return;

			// Invalidate.
			this.Invalidate();
		}

		public Image GetGrayScaleImage(Bitmap source)
		{
			Bitmap bm = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			for (int y = 0; y < bm.Height; y++)
			{
				for (int x = 0; x < bm.Width; x++)
				{
					Color c = source.GetPixel(x, y);
					if (c == Color.Transparent || c == Color.FromArgb(0, 0, 0, 0))
						bm.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
					else
					{

						bm.SetPixel(x, y, MakeColorGrayScale(c));
					}
				}
			}

			return bm;
		}

		private Color MakeColorGrayScale(Color color)
		{
			// Get the right grayscale value from the color.
			int value = (int)((color.R + color.G + color.B) / 3);
			// Get a new grayscale color.
			return Color.FromArgb(value, value, value);
		}
	}
}
