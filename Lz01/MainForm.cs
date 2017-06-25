/*
 * Created by SharpDevelop.
 * User: Alpha
 * Date: 30.12.2015
 * Time: 22:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace Lz01
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{ 
		class price
		{
			private decimal cina;
			public decimal Cina
			{
				get {return cina;}
				private set {cina=value;}
			}
			
		}

		class price_rozdr :price
		{
			private decimal nacenka=0.2M;
			public decimal Cina_rozdr
			{
				get {return Cina*nacenka;}
			}
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			MyListOfConverters.Add(new UnitConverter(1000,"Кілометри","Метри"),
			                       new MyRadioButtonClass());
            MyListOfConverters.MyRadioButton[0].Click += Button1Click;
            this.groupBox2.Controls.Add(MyListOfConverters.MyRadioButton[0]);
			
		}
		
		public class MyRadioButtonClass :System.Windows.Forms.RadioButton
		{
			public MyRadioButtonClass()
			{
				this.AutoSize=true;
				this.CheckedChanged+=ChkChnd;
			} //Конструктор
			
			public void ChkChnd(object sender, EventArgs e)
			{
                if (((System.Windows.Forms.RadioButton)sender).Checked) 
                {
                    this.Parent.Tag = ((System.Windows.Forms.RadioButton)sender).Tag;
                }
			}//Метод
		}
		
		public ListOfConverters MyListOfConverters = new ListOfConverters();
		
		public class ListOfConverters
		{
			public List<UnitConverter> MyConverters = new List<UnitConverter>();
			
			public List<MyRadioButtonClass> MyRadioButton = new List<MyRadioButtonClass>();
				
			private static int i;
			
			public int ItemCount {get {return(i);}}
			
			public ListOfConverters() {i=0;}
			
			public void Add(UnitConverter U, MyRadioButtonClass R)
			{
				MyConverters.Add(U);
				R.Text=U.NameFrom+" в "+U.NameTo;
				R.Location = new System.Drawing.Point(6, 25*(i+1));
                R.Tag = ++i;
				MyRadioButton.Add(R);
			}
			
			public double ItemConvert(int i,double x)
			{
				return MyConverters[i].Convert(x);
			}
		}
				
		public class UnitConverter
  			{
			double Koef; 	//Поле
			public string NameFrom;
			public string NameTo;
			
			public UnitConverter (double UnitKoef, string NaFr, string NaTo)
    			{
   				Koef=UnitKoef;
   				NameFrom=NaFr;
   				NameTo=NaTo;
   			    }	//Конструктор
   			public double Convert (double Unit)
    			{return Unit*Koef;}//Метод
			}	
				
		public void Button1Click(object sender, EventArgs e)
		{
			try
			{
				int n=Convert.ToInt16(this.groupBox2.Tag)-1;
				label2.Text=(MyListOfConverters.ItemConvert(n,Convert.ToDouble(textBox1.Text))).ToString();
				label1.Text=MyListOfConverters.MyConverters[n].NameFrom;
				groupBox1.Text=MyListOfConverters.MyConverters[n].NameTo;
			}
			catch
			{
				label2.Text="Помилка!";
				textBox1.SelectAll();
				textBox1.Focus();
			}
		}
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			label2.Text="";
		}
		
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Form1 F= new Form1();
			if (F.ShowDialog(this)==DialogResult.OK)
			{
				MyListOfConverters.Add(new UnitConverter(F.Koef,F.InpV,F.OutpV),
				                       new MyRadioButtonClass());			                       
			
			this.groupBox2.Controls.Add(
				MyListOfConverters.MyRadioButton[MyListOfConverters.ItemCount-1]);
			MyListOfConverters.MyRadioButton[MyListOfConverters.ItemCount-1].CheckedChanged+=
				this.Button1Click;
			}
            
			F.Dispose();
		}
		
	}
}
