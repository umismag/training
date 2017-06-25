using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;

namespace Komax_Convert
{
	public partial class Form1 : Form
	{
		Microsoft.Office.Interop.Excel.Application XL;

		public Form1()
		{
			InitializeComponent();

		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private async void button1_Click(object sender, EventArgs e)
		{
			try
			{
				Workbook WB;
				OpenFileDialog xlsFile = new OpenFileDialog();
				if (xlsFile.ShowDialog() == DialogResult.OK)
				{
					WB = await OpenXLSAsync(xlsFile.FileName);
					label2.Text = xlsFile.FileName;
					toolTip1.SetToolTip(label2, xlsFile.FileName);
					this.Activate();
				}
				else
					return;
								
				IProgress<int> onChangeProgress = new Progress<int>((i) =>
				{
					label4.Text = i.ToString();
					if (progressBar1.Maximum == 0)
						progressBar1.Maximum = i;
					else
						progressBar1.Value = i;
				});
				label4.Text = (await ProcessAsync(WB, onChangeProgress)).ToString() + " рядків.";
				//ViewTree(NewArticles);
				textBox1.Text = GenerateText(NewArticles);

				XL.ActiveWorkbook.Close();
				XL.Application.Quit();
				XL.Quit();
			}
			catch (Exception err)
			{
				MessageBox.Show(err.Message);
			}
			finally
			{
				if (XL != null)
					XL.Quit();
				XL = null;
			}
		}

		Task<Microsoft.Office.Interop.Excel.Workbook> OpenXLSAsync(string xlsFileName)
		{
			return Task.Run( () =>
			{
				XL = new Microsoft.Office.Interop.Excel.Application();				
				XL.Visible = true;
				return XL.Workbooks.Open(xlsFileName);
			}
				);
		}

		//void ViewTree(Dictionary<string, NewArticle> NewArticles)
		//{
		//	TreeNode NewArticlesNode = new TreeNode("New Articles");

		//	foreach (string NewArticleKey in NewArticles.Keys)
		//	{
		//		TreeNode NewArticleNode = new TreeNode("New Article:" + NewArticleKey);
		//		for (int i = 0; i < NewArticles[NewArticleKey].NumberOfLeadSets; i++)
		//		{
		//			TreeNode NewLeadSetNode = new TreeNode("NewLeadSet" + (i + 1));

		//			foreach (string prop in NewArticles[NewArticleKey][i].PrintNewLeadSet())
		//			{
		//				NewLeadSetNode.Nodes.Add(prop);
		//			}
		//			NewArticleNode.Nodes.Add(NewLeadSetNode);
		//		}
		//		NewArticlesNode.Nodes.Add(NewArticleNode);
		//	}
		//	treeView1.Nodes.Add(NewArticlesNode);
		//}

		string GenerateText(Dictionary<string, NewArticle> Newarticles)
		{
			StringBuilder res = new StringBuilder();
			
			SortedSet<NewArticle> sortedNewarticles = new SortedSet<NewArticle>();
						
			foreach (var na in Newarticles)
				sortedNewarticles.Add(na.Value);
			
			foreach (NewArticle na in sortedNewarticles)
			{
				res.AppendFormat("[DeleteArticle]");
				res.AppendLine();
				res.AppendFormat("  ArticleKey = {0}", na.ArticleKey);
				res.AppendLine();
				res.AppendLine();
				res.AppendFormat("[NewArticle]");
				res.AppendLine();
				res.AppendFormat("      ArticleKey = {0}", na.ArticleKey);
				res.AppendLine();
				res.AppendFormat("      NumberOfLeadSets = {0}", na.NumberOfLeadSets);
				res.AppendLine();
				res.AppendLine();
				int i = 0;
				//for (int i = 0; i < na.NumberOfLeadSets; i++)
				foreach (NewLeadSet nls in na.Items)
				{
					res.AppendFormat("     [NewLeadSet{0}]", i + 1);

					foreach (string prop in nls.PrintNewLeadSet())
					{
						res.AppendLine();
						if (prop.Contains("[NewMarkingTextWire1-2]"))
						{
							res.AppendLine();
							res.AppendFormat("     {0}", prop);
						}
						else
							res.AppendFormat("         {0}", prop);
					}
					res.AppendLine();
					res.AppendLine();
					i++;
				}
				res.AppendLine();
				res.AppendLine();
			}
			return res.ToString();

			//DeleteArticle]
			//	ArticleKey = A2815

			//[NewArticle]
			// ArticleKey = A2815
			// NumberOfLeadSets = 5

			//[NewLeadSet1]
			//		WireKey = 2mm2blue
			//		WireLength = 600
			//		FontKey = 5x5
			//		StrippingLength = 20, 10
			//		PullOffLength = 10, 7

			//  [NewMarkingTextWire1-2]
			//		MarkingTextBegin = 20, "Begin 1", 1
			//		MarkingTextBegin = 60, "Begin 2", 1
			//		MarkingTextBegin = 100, "Begin 3", 1
			//		MarkingTextEndless = 40, "Endless", 1
			//		MarkingTextEnd = 100, "End 1", 0
			//		MarkingTextEnd = 60, "End 2", 0
			//		MarkingTextEnd = 100, "End 3", 0

		}

		enum ColumnNumbers
		{
			NomerPoChertegu1 = 4,
			NomerPoChertegu2 = 5,
			WireSm2 = 6,
			WireColor = 7,
			WireColorNew = 57,                      //!!!!!
			WireLength = 8,
			StrippingLength1 = 16,
			PullOffLength1 = 17,
			StrippingLength2 = 29,
			PullOffLength2 = 30,
			WireMarka = 36,
			WireMarkaNew = 59,                      //!!!!!
			Pieces = 40,
			NameOfNewLeadSet = 56,
			ArticleKey = 58,
			WireKey = 59,
			FontKey = 64,

			MarkingTextBegin1_distance = 66,
			MarkingTextBegin1_MarkingText = 65,
			MarkingTextBegin1_turnText = 67,

			MarkingTextBegin2_distance = 70,
			MarkingTextBegin2_MarkingText = 69,
			MarkingTextBegin2_turnText = 71,

			MarkingTextBegin3_distance = 74,
			MarkingTextBegin3_MarkingText = 73,
			MarkingTextBegin3_turnText = 75,

			MarkingTextEndless_distance = 80,
			MarkingTextEndless_MarkingText = 79,
			MarkingTextEndless_turnText = 81,

			MarkingTextEnd1_distance = 85,
			MarkingTextEnd1_MarkingText = 84,
			MarkingTextEnd1_turnText = 86,

			MarkingTextEnd2_distance = 89,
			MarkingTextEnd2_MarkingText = 88,
			MarkingTextEnd2_turnText = 90,

			MarkingTextEnd3_distance = 93,
			MarkingTextEnd3_MarkingText = 92,
			MarkingTextEnd3_turnText = 94
		};

		Dictionary<string, NewArticle> NewArticles = new Dictionary<string, NewArticle>();

		double? GetDoubleValue(double v)
		{
			return v;
		}

		double? GetDoubleValue(string v)
		{
			if (double.TryParse(v, out double res))
				return res;
			else
				return null;
		}

		int? GetIntValue(int v)
		{
			return v;
		}

		int? GetIntValue(string v)
		{
			if (int.TryParse(v, out int res))
				return res;
			else
				return null;
		}

		int? GetIntValue(double v)
		{
			return (int)Math.Floor(v);

		}

		Task<int> ProcessAsync(Workbook WB, IProgress<int> ChangeProgressBar)
		{
			return Task.Run(() =>
		  {
			  Worksheet T1 = WB.Sheets["А407-088Т1"];

			  int NumberOfFirstRow = 8;
			  int maxRow = NumberOfFirstRow;

			  while (T1.Cells[maxRow, ColumnNumbers.WireSm2].Value != null)
			  {
				  maxRow++;

			  }

			  ChangeProgressBar.Report(maxRow - NumberOfFirstRow);

			  
			  int CurrentRow = NumberOfFirstRow;
			  while (T1.Cells[CurrentRow, ColumnNumbers.WireSm2].Value != null)
			  {
				  Wire wr = new Wire(T1.Cells[CurrentRow, ColumnNumbers.WireSm2].Value,
					  T1.Cells[CurrentRow, ColumnNumbers.WireColorNew].Value.ToString(), T1.Cells[CurrentRow, ColumnNumbers.WireKey].Value.ToString().Replace(',', '.'));
				  
					  //T1.Cells[8,7]
					   //Color = WireColor.GetEngColor(T1.Cells[CurrentRow, ColumnNumbers.WireColor].Value.ToString()),
					  //wr.WireKey = wr.ElectricalSizeMM2.ToString().Replace(',', '.') + "_" + WireColor.toText(wr.Color) + "__PVA_" + T1.Cells[CurrentRow, ColumnNumbers.WireMarka].Value + "_U_660";
					
				  
				  //string ArticleKey = wr.ElectricalSizeMM2.ToString().Replace(',', '.') + WireColor.toText(wr.Color);
				  string ArticleKey = T1.Cells[CurrentRow, ColumnNumbers.ArticleKey].Value.ToString().Replace(',', '.');

				  //string name = "\"" + (T1.Cells[CurrentRow, ColumnNumbers.NomerPoChertegu1].Value ?? "").ToString() + "   " + (T1.Cells[CurrentRow, ColumnNumbers.NomerPoChertegu2].Value ?? "").ToString() + "\"";
				  string name = "\"" + T1.Cells[CurrentRow, ColumnNumbers.NameOfNewLeadSet].Value + "\"";

				  double wireLength = T1.Cells[CurrentRow, ColumnNumbers.WireLength].Value;

				  double?[] strippingLength = new double?[2]
				  {GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.StrippingLength1].Value),
				GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.StrippingLength2].Value)};

				  double?[] pullOffLength = new double?[2]
					  {GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.PullOffLength1].Value),
					GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.PullOffLength2].Value)};

				  int pieces = (int)T1.Cells[CurrentRow, ColumnNumbers.Pieces].Value;

				  string fontKey = T1.Cells[CurrentRow, ColumnNumbers.FontKey].Value;

				  MarkingText MTBegin1 = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextBegin
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin1_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin1_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin1_turnText].Value)
					  );

				  MarkingText MTBegin2 = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextBegin
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin2_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin2_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin2_turnText].Value)
					  );

				  MarkingText MTBegin3 = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextBegin
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin3_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin3_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextBegin3_turnText].Value)
					  );

				  MarkingText MTEndless = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextEndless
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEndless_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEndless_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEndless_turnText].Value)
					  );

				  MarkingText MTEnd1 = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextEnd
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd1_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd1_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd1_turnText].Value)
					  );

				  MarkingText MTEnd2 = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextEnd
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd2_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd2_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd2_turnText].Value)
					  );

				  MarkingText MTEnd3 = new MarkingText(
					  MarkingText.MarkingTextTypes.MarkingTextEnd
					  , GetDoubleValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd3_distance].Value)
					  , T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd3_MarkingText].Value
					  , GetIntValue(T1.Cells[CurrentRow, ColumnNumbers.MarkingTextEnd3_turnText].Value)
					  );

				  NewMarkingTextWire nmtw = new NewMarkingTextWire(new List<MarkingText>
				  {
					  MTBegin1,
					  MTBegin2,
					  MTBegin3,
					  MTEndless,
					  MTEnd1,
					  MTEnd2,
					  MTEnd3
				  });

				  NewLeadSet nls = new NewLeadSet(name, new string[] { wr.WireKey }, new double[] { wireLength }, strippingLength, pullOffLength, pieces, fontKey, nmtw);

				  if (NewArticles.ContainsKey(ArticleKey))
				  {
					  ((NewArticle)NewArticles[ArticleKey]).AddNewLeadSet(nls);
				  }
				  else
				  {
					  NewArticles.Add(ArticleKey, new NewArticle(wr, ArticleKey, new List<NewLeadSet> { nls }));
				  }

				  ChangeProgressBar.Report(CurrentRow - NumberOfFirstRow+1);
				  CurrentRow++;
			  }
			  return CurrentRow - NumberOfFirstRow;
		  });
		}// Process!!!!!!!!

		private void progressBar1_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Записати зміни?", "Збереження змін", MessageBoxButtons.YesNo) == DialogResult.Yes)

				File.WriteAllText(@"Article.dds", textBox1.Text);
		}
	}

	class MarkingText
	{
		double distance;
		double Distance
		{
			get { return distance; }
			set { distance = value; }
		}

		string text;
		string Text
		{
			get { return text; }
			set
			{
				if (value.Length > 39)
					text = "\"" + value.Substring(0, 39) + "\"";
				else
					text = "\"" + value + "\"";
			}
		}

		int turnText;
		int TurnText
		{
			get { return turnText; }
			set
			{
				if (value != 1)
					turnText = 0;
				else turnText = value;
			}
		}

		public enum MarkingTextTypes { MarkingTextBegin, MarkingTextEndless, MarkingTextEnd };

		MarkingTextTypes MarkingTextType;

		public MarkingText(MarkingTextTypes markingTextType, double distance, string markingText, int turnText)
		{
			MarkingTextType = markingTextType;
			Distance = distance;
			Text = markingText;
			TurnText = turnText;
		}

		public override string ToString()
		{
			return MarkingTextType.ToString() + " = " + Distance.ToString().Replace(',', '.') + "," + Text + "," + TurnText;
		}

		//  [NewMarkingTextWire1-2]
		//		MarkingTextBegin = 20, "Begin 1", 1
		//		MarkingTextBegin = 60, "Begin 2", 1
		//		MarkingTextBegin = 100, "Begin 3", 1
		//		MarkingTextEndless = 40, "Endless", 1
		//		MarkingTextEnd = 100, "End 1", 0
		//		MarkingTextEnd = 60, "End 2", 0
		//		MarkingTextEnd = 100, "End 3", 0
	}

	class NewMarkingTextWire
	{
		List<MarkingText> Items = new List<MarkingText>();

		public MarkingText this[int i]
		{
			get { return Items[i]; }
			set
			{
				Items[i] = value;
			}
		}

		string Name = "[NewMarkingTextWire1-2]";

		public NewMarkingTextWire(List<MarkingText> markingTextWire)
		{
			Items.AddRange(markingTextWire);
		}

		public NewMarkingTextWire(MarkingText markingTextWire)
		{
			Items.Add(markingTextWire);
		}

		public void AddNewMarkingText(MarkingText markingTextWire)
		{
			Items.Add(markingTextWire);
		}

		public override string ToString()
		{
			StringBuilder res = new StringBuilder(Name);

			foreach (MarkingText mt in Items)
			{
				res.AppendLine();
				res.AppendFormat("         {0}", mt.ToString());
			}
			return res.ToString();
		}
	}

	//[DeleteArticle]
	//ArticleKey = 0.75_BI

	// [NewArticle]
	//	ArticleKey = 0.75_BI
	//	NumberOfLeadSets = 16

	//   [NewLeadSet1]

	//	 Name = "0   20"
	//        WireKey = 0.75_BI_PVA_Proof_U_660
	//	 WireLength = 10000

	//	 StrippingLength = 6, 6
	//        PullOffLength = 2, 2
	//        Pieces = 100
	//        FontKey = 5x5
	//	 NewMarkingText = NewMarkingTextWire1 - 2

	//MarkingTextBegin = 30,"0   20",1    MarkingTextBegin = 80,"0   20",1    MarkingTextBegin = 130,"0   20",1    MarkingTextEndless = 100,"0   20",1    MarkingTextEnd = 30,"0   20",1    MarkingTextEnd = 80,"0   20",1    MarkingTextEnd = 130,"0   20",1


	class NewLeadSet:IComparable<NewLeadSet>
	{
		enum leadSetTypes { singleLead, doubleLead, twisterLead };

		leadSetTypes leadSetType = leadSetTypes.singleLead;
		leadSetTypes LeadSetType
		{
			get { return leadSetType; }
			set
			{
				leadSetType = value;
			}
		}

		string name;
		//Текст без переводу рядка
		//\ "= Подвійна лапки
		//Добровільне додаткове ім'я для елемента
		//Там немає обмежень на тип характеру, який може бути використаний
		//Ім'я повинно бути між подвійними лапками
		//Максимальна кількість символів = 50
		public string Name
		{
			get { return name; }
			set
			{
				if (value.Length > 50)
					name = value.Substring(0, 50);
				else
					name = value;
			}
		}

		string[] wireKey = new string[2];
		//The key must not be between quotation marks
		//Maximum number of characters = 25
		//If the wire does not exist, a new wire with the key "WireKey" will be created
		public string[] WireKey
		{
			get { return wireKey; }
			set
			{
				for (int i = 0; i < 2; i++)
					if (value[i].Length > 25)
						wireKey[i] = value[i].Substring(0, 25);
					else
						wireKey[i] = value[i];
			}
		}

		double[] wireLength = new double[2];
		//Length Example: 23.4
		//Unit: [mm]
		public double[] WireLength
		{
			get { return wireLength; }
			set
			{
				for (int i = 0; i < 2; i++)
					if (value[i] != 0)
						wireLength[i] = value[i];
					else break;
			}
		}

		double?[] strippingLength = new double?[3];
		public double?[] StrippingLength
		{
			get { return strippingLength; }
			set
			{
				for (int i = 0; i < 3; i++)
					strippingLength[i] = value[i];
			}
		}

		double?[] pullOffLength = new double?[3];
		public double?[] PullOffLength
		{
			get { return pullOffLength; }
			set
			{
				for (int i = 0; i < 3; i++)
					pullOffLength[i] = value[i];
			}
		}

		int pieces = 1;
		//For multi leadset production; if greater than 1 the key "TotalPieces" from block[NewJob] will be ignored
		//		Number of pieces.Example: 1500
		//Only whole numbers
		//Unit: [pieces]
		public int Pieces
		{
			get { return pieces; }
			set
			{
				pieces = value;
			}
		}

		string fontKey;
		public string FontKey
		{
			get { return fontKey; }
			set
			{
				fontKey = value;
			}
		}

		NewMarkingTextWire newMarkingText;
		public NewMarkingTextWire NewMarkingText
		{
			get { return newMarkingText; }
			set
			{
				newMarkingText = value;
			}
		}

		public NewLeadSet(string name, string[] wireKey, double[] wireLength)
		{
			Name = name;

			for (int i = 0; i < wireKey.Length; i++)
				WireKey[i] = wireKey[i];

			for (int i = 0; i < wireLength.Length; i++)
				WireLength[i] = wireLength[i];
		}

		public NewLeadSet(string name, string[] wireKey, double[] wireLength, double?[] strippingLength) : this(name, wireKey, wireLength)
		{
			for (int i = 0; i < strippingLength.Length; i++)
				StrippingLength[i] = strippingLength[i];
		}

		public NewLeadSet(string name, string[] wireKey, double[] wireLength, double?[] strippingLength, double?[] pullOffLength) : this(name, wireKey, wireLength, strippingLength)
		{
			for (int i = 0; i < pullOffLength.Length; i++)
				PullOffLength[i] = pullOffLength[i];
		}

		public NewLeadSet(string name, string[] wireKey, double[] wireLength, double?[] strippingLength, double?[] pullOffLength, int pieces) : this(name, wireKey, wireLength, strippingLength, pullOffLength)
		{
			Pieces = pieces;
		}

		public NewLeadSet(string name, string[] wireKey, double[] wireLength, double?[] strippingLength, double?[] pullOffLength, int pieces, string fontKey) : this(name, wireKey, wireLength, strippingLength, pullOffLength, pieces)
		{
			FontKey = fontKey;
		}

		public NewLeadSet(string name, string[] wireKey, double[] wireLength, double?[] strippingLength, double?[] pullOffLength, int pieces, string fontKey, NewMarkingTextWire newMarkingTextWire) : this(name, wireKey, wireLength, strippingLength, pullOffLength, pieces, fontKey)
		{
			NewMarkingText = newMarkingTextWire;
		}

		public List<string> PrintNewLeadSet()
		{
			List<string> res = new List<string>();

			Type t = this.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propinfo in pi)
			{
				if (propinfo.PropertyType.Name.IndexOf("[]") > 0)
				{
					string s = string.Empty;
					foreach (var element in propinfo.GetValue(this, null) as Array)
					{
						if (element != null)
						{
							if (element is double)
								if (element.ToString() != "0")
									s += element.ToString() + ", ";
								else
									continue;
							else
								s += element.ToString() + ", ";
						}
						else
							break;
					}
					if (s.LastIndexOf(", ") == s.Length - 2)
						s = s.Remove(s.Length - 2, 2);
					res.Add(propinfo.Name + " = " + s);
				}
				else if (propinfo.Name == "NewMarkingText")
				{
					res.Add(propinfo.GetValue(this, null).ToString());
				}
				else
					res.Add(propinfo.Name + " = " + propinfo.GetValue(this, null));
			}

			return res;
		}

		public int CompareTo(NewLeadSet other)
		{
			return Compare(this, other);
		}
		public int Compare(NewLeadSet x, NewLeadSet y)
		{
			if (x.WireLength[0] > y.WireLength[0])
				return 1;
			if (x.WireLength[0] < y.WireLength[0])
				return -1;
			else
			{
				return x.Name.CompareTo(y.Name);
			}
		}

		
		//DeleteArticle]
		//	ArticleKey = A2815

		//[NewArticle]
		// ArticleKey = A2815
		// NumberOfLeadSets = 5

		//[NewLeadSet1]
		//		WireKey = 2mm2blue
		//		WireLength = 600
		//		FontKey = 5x5
		//		StrippingLength = 20, 10
		//		PullOffLength = 10, 7

		//  [NewMarkingTextWire1-2]
		//		MarkingTextBegin = 20, "Begin 1", 1
		//		MarkingTextBegin = 60, "Begin 2", 1
		//		MarkingTextBegin = 100, "Begin 3", 1
		//		MarkingTextEndless = 40, "Endless", 1
		//		MarkingTextEnd = 100, "End 1", 0
		//		MarkingTextEnd = 60, "End 2", 0
		//		MarkingTextEnd = 100, "End 3", 0

	}

	//class WireColor
	//{
	//	enum UkrBaseColor { Б, Г, Ж, З, К, О, П, Р, С, Ф, Ч };
	//	enum EngBaseColor { WT, BI, YW, GR,BR,OR,RD, PR, GY, VT, BK };
	//	enum UkrColor { Б, БГ, БЖ, БЗ, БК, БО, БП, БР, БС, БФ, БЧ, Г, ГБ, ГЖ, ГЗ, ГК, ГО, ГП, ГР, ГС, ГФ, ГЧ, Ж, ЖБ, ЖГ, ЖЗ, ЖК, ЖО, ЖП, ЖР, ЖС, ЖФ, ЖЧ, З, ЗБ, ЗГ, ЗЖ, ЗК, ЗО, ЗП, ЗР, ЗС, ЗФ, ЗЧ, К, КБ, КГ, КЖ, КЗ, КО, КП, КР, КС, КФ, КЧ, О, ОБ, ОГ, ОЖ, ОЗ, ОК, ОП, ОР, ОС, ОФ, ОЧ, П, ПБ, ПГ, ПЖ, ПЗ, ПК, ПО, ПР, ПС, ПФ, ПЧ, Р, РБ, РГ, РЖ, РЗ, РК, РО, РП, РС, РФ, РЧ, С, СБ, СГ, СЖ, СЗ, СК, СО, СП, СР, СФ, СЧ, Ф, ФБ, ФГ, ФЖ, ФЗ, ФК, ФО, ФП, ФР, ФС, ФЧ, Ч, ЧБ, ЧГ, ЧЖ, ЧЗ, ЧК, ЧО, ЧП, ЧР, ЧС, ЧФ };
	//	public enum EngColor { WT, WT_BI, WT_YW, WT_GR, WT_BR, WT_OR, WT_RD, WT_PR, WT_GY, WT_VT, WT_BK, BI, BI_WT, BI_YW, BI_GR, BI_BR, BI_OR, BI_RD, BI_PR, BI_GY, BI_VT, BI_BK, YW, YW_WT, YW_BI, YW_GR, YW_BR, YW_OR, YW_RD, YW_PR, YW_GY, YW_VT, YW_BK, GR, GR_WT, GR_BI, GR_YW, GR_BR, GR_OR, GR_RD, GR_PR, GR_GY, GR_VT, GR_BK, BR, BR_WT, BR_BI, BR_YW, BR_GR, BR_OR, BR_RD, BR_PR, BR_GY, BR_VT, BR_BK, OR, OR_WT, OR_BI, OR_YW, OR_GR, OR_BR, OR_RD, OR_PR, OR_GY, OR_VT, OR_BK, RD, RD_WT, RD_BI, RD_YW, RD_GR, RD_BR, RD_OR, RD_PR, RD_GY, RD_VT, RD_BK, PR, PR_WT, PR_BI, PR_YW, PR_GR, PR_BR, PR_OR, PR_RD, PR_GY, PR_VT, PR_BK, GY, GY_WT, GY_BI, GY_YW, GY_GR, GY_BR, GY_OR, GY_RD, GY_PR, GY_VT, GY_BK, VT, VT_WT, VT_BI, VT_YW, VT_GR, VT_BR, VT_OR, VT_RD, VT_PR, VT_GY, VT_BK, BK, BK_WT, BK_BI, BK_YW, BK_GR, BK_BR, BK_OR, BK_RD, BK_PR, BK_GY, BK_VT };

	//	//public readonly Dictionary<string, string> wireColors = new Dictionary<string, string>();

	//	//public WireColor()
	//	//{
	//	//	for (UkrColor i=UkrColor.Б;i<=UkrColor.ЧФ;i++)
	//	//	{
	//	//		wireColors.Add(i.ToString(), ((EngColor)i).ToString().Replace('_','-'));
	//	//	}
	//	//}

	//	public static string toText(EngColor ec)
	//	{
	//		return ec.ToString().Replace('_', '-');
	//	}

	//	//public static string GetTxtEngColor(string ukrColor)
	//	//{
	//	//	UkrColor tmpUkrColor;
	//	//	if (Enum.TryParse<UkrColor>(ukrColor, out tmpUkrColor))
	//	//		return toText((EngColor)tmpUkrColor);
	//	//	else
	//	//		return null;
	//	//}

	//	public static EngColor GetEngColor(string ukrColor)
	//	{
	//		if (Enum.TryParse<UkrColor>(ukrColor, out UkrColor tmpUkrColor))
	//			return (EngColor)tmpUkrColor;
	//		else
	//		{
	//			Exception WireColorException = new Exception("Не вдається розпізнати колір \"" + ukrColor + "\"!");
	//			throw WireColorException;
	//		}
	//	}
	//}

	class Wire : IComparer<Wire>
	{
		string wireKey;
		//The key must not be between quotation marks
		//Maximum number of characters = 25
		public string WireKey
		{
			get { return wireKey; }
			private set
			{
				if (value.Length > 25)
					wireKey = value.Substring(0, 25);
				else
					wireKey = value;
			}
		}

		double electricalSizeMM2 = 0;
		//Cross-section.Example: 0.75
		//Unit: [mm2]
		//0 .. 10 mm2, For Kappa machines, the range of the electrical size in mm2 is between 0 and 120 mm2 (respect SW version)
		public double ElectricalSizeMM2
		{
			get { return electricalSizeMM2; }
			private set
			{
				electricalSizeMM2 = value;
			}
		}

		//WireColor.EngColor color = WireColor.EngColor.RD;
		//public WireColor.EngColor Color
		//{
		//	get { return color; }
		//	set
		//	{
		//		color = value;
		//	}
		//}
		string color;
		public string Color
		{
			get { return color; }
			private set
			{
				color = value;
			}
		}

		public Wire(double electricalSizeInMM2, string color)
		{
			ElectricalSizeMM2 = electricalSizeInMM2;
			Color = color;
			WireKey = ElectricalSizeMM2 + Color;
		}

		public Wire(double electricalSizeInMM2, string color, string wireKey):this(electricalSizeInMM2,color)
		{
			WireKey = wireKey;
		}

		public int Compare(Wire x, Wire y)
		{
			if (x.ElectricalSizeMM2>y.ElectricalSizeMM2)
				return 1;
			if (x.ElectricalSizeMM2<y.ElectricalSizeMM2)
				return -1;
			else
				return 0;
		}
	}

	class WireComparer : IComparer<Wire>
	{
		public int Compare(Wire x, Wire y)
		{
			if (x.ElectricalSizeMM2 > y.ElectricalSizeMM2)
				return 1;
			if (x.ElectricalSizeMM2 < y.ElectricalSizeMM2)
				return -1;
			else
				return 0;
		}
	}

	class NewArticle: IComparable<NewArticle>
	{
		//List<NewLeadSet> items = new List<NewLeadSet>();
		SortedSet<NewLeadSet> items = new SortedSet<NewLeadSet>();

		public SortedSet<NewLeadSet> Items
		{
			get { return items; }
		}

		//public NewLeadSet this[int i]//індексатор
		//{
		//	get { return items; }
		//	set
		//	{
		//		items[i] = value;
		//	}
		//}

		string articleKey;
		//The key must not be between quotation marks
		//Maximum number of characters = 25
		//The optional second article key defines the article where this new article will be copied from.
		public string ArticleKey
		{
			get { return articleKey; }
			set
			{
				if (value.Length > 25)
					articleKey = value.Substring(0, 25);
				else
					articleKey = value;
			}
		}

		public int NumberOfLeadSets
		{
			get { return items.Count; }
		}

		Wire wireOfNewArticle;
		public Wire WireOfNewArticle
		{
			get { return wireOfNewArticle; }
			set
			{
				wireOfNewArticle = value;
			}
		}

		//private NewArticle() { }

		public NewArticle(Wire wire)
		{
			ArticleKey = wire.ElectricalSizeMM2.ToString() + wire.Color.ToString();
			WireOfNewArticle = wire;
		}

		public NewArticle(Wire wire, string articleKey):this(wire)
		{
			ArticleKey = articleKey;
			WireOfNewArticle = wire;
		}

		public NewArticle(Wire wire, string articleKey, List<NewLeadSet> NewLeadSets) : this(wire, articleKey)
		{
			foreach(NewLeadSet item in NewLeadSets)
			items.Add(item);
		}

		public void AddNewLeadSet(NewLeadSet leadSet)
		{
			items.Add(leadSet);
		}

		public int Compare(NewArticle x, NewArticle y)
		{
			if (x.WireOfNewArticle.ElectricalSizeMM2 > y.WireOfNewArticle.ElectricalSizeMM2)
				return 1;
			if (x.WireOfNewArticle.ElectricalSizeMM2 < y.WireOfNewArticle.ElectricalSizeMM2)
				return -1;
			else
			{
				return x.WireOfNewArticle.Color.CompareTo(y.WireOfNewArticle.Color);				
			}
		}

		public int CompareTo(NewArticle other)
		{
			return Compare(this, other);
		}
	}

	class NewArticleCompare : IComparer<NewArticle>//, IComparable<NewArticle>
	{
		public int Compare(NewArticle x, NewArticle y)
		{
			if (x.WireOfNewArticle.ElectricalSizeMM2 > y.WireOfNewArticle.ElectricalSizeMM2)
				return 1;
			if (x.WireOfNewArticle.ElectricalSizeMM2 < y.WireOfNewArticle.ElectricalSizeMM2)
				return -1;
			else
				return 0;
		}

		//public int CompareTo(NewArticle other)
		//{
		//	return Compare(this, other);
		//}
	}
}
