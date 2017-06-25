using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Countour
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		double[,] kadr=new double[3010,2020];
		double[,] contur=new double[3010, 2020];
		public void pictureBox1_Click(object sender, EventArgs e)
		{
			Bitmap p=new Bitmap(@"C:\TMP\#\888.png");
			
			pictureBox1.BackgroundImage = p;
			//if (p.PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)


			for (int i = 1; i < p.Width - 1; i++)
				for (int j = 1; j < p.Height - 1; j++)
				{
					kadr[i, j] = ((
					p.GetPixel(i, j).B * 0.072 +
					p.GetPixel(i, j).G * 0.715 +
					p.GetPixel(i, j).R * 0.212
					));
				}
			for (int i = 1; i < p.Width - 1; i++)
				for (int j = 1; j < p.Height - 1; j++)
				{
					double rizn=kadr[i, j - 1] + kadr[i, j + 1] + kadr[i - 1, j] + kadr[i + 1, j] - 4 * kadr[i, j];
						if (Math.Abs(rizn)<20)
							contur[i, j] = Math.Abs(rizn);
						else contur[i, j] = 255;
					}

				for (int i = 1; i < p.Width - 1; i++)
					for (int j = 1; j < p.Height - 1; j++)
					{
						if (contur[i,j]==255)
						p.SetPixel(i, j,Color.FromArgb((int)contur[i,j],0,0));
						//else p.SetPixel(i, j, Color.FromArgb(0,(byte)contur[i, j], 0));
				}

				//p.SetPixel
				//p.Palette
			
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			ActiveForm.Text = "(x= " +e.X+ ", y= " +e.Y+ ") = " + kadr[e.Y, e.X]+"    Contur= "+contur[e.X,e.X];
		}
	}
}
