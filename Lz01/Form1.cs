/*
 * Created by SharpDevelop.
 * User: Alpha
 * Date: 31.12.2015
 * Time: 23:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lz01
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : Form
	{
		public Form1()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			button1.DialogResult=DialogResult.OK;
		}
		void Button2Click(object sender, EventArgs e)
		{
			button2.DialogResult= DialogResult.Cancel;
		}
		public string InpV 
		{
			get {return textBox1.Text;}
		}
		public string OutpV 
		{
			get {return textBox2.Text;}
		}
		public double Koef 
		{
			get {return Convert.ToDouble(textBox3.Text);}
		}
	}
}
