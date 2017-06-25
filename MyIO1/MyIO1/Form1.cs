using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MyIO1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				//DirectoryInfo myFolder
			}
			catch (Exception)
			{

				throw;
			}
		}

		
		private void Form1_Load(object sender, EventArgs e)
		{
			TreeNode DriveNodes=new TreeNode("Файлова система");
			
			foreach (string dr in Directory.GetLogicalDrives())
			{
				TreeNode folder=new TreeNode(dr.Remove(dr.Length-1));
				DirectoryInfo di =new DirectoryInfo(dr);
				foreach (DirectoryInfo fl in di.GetDirectories())
					folder.Nodes.Add(fl.Name);
				DriveNodes.Nodes.Add(folder);

			}
			treeView1.Nodes.Add(DriveNodes);
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Parent == null)
				return;

			foreach (TreeNode el in e.Node.Nodes)
			{
				try
				{
					//TreeNode innerEl = new TreeNode();
					DirectoryInfo di =new DirectoryInfo(el.FullPath.Remove(0,el.FullPath.IndexOf('\\')+1));
					DirectoryInfo[] fl = di.GetDirectories();
					for (int i = 0; i < fl.Length; i++)
					{
						el.Nodes.Add(fl[i].Name);
					}
					//el.Nodes.Add(innerEl);
				}
				catch (Exception)
				{
					el.ForeColor = Color.LightGray;
				}
			}
			e.Node.EnsureVisible();
		}

		private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
		{

		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Parent == null)
				return;
			DirectoryInfo di =new DirectoryInfo(e.Node.FullPath.Remove(0,e.Node.FullPath.IndexOf('\\')+1));
			label2.Text = di.GetType().ToString();
			label4.Text = e.Node.Text;

			label6.Text = di.FullName;
			toolTip1.SetToolTip(label6, label6.Text);

			label8.Text = di.CreationTime.ToString();
			label10.Text = di.LastAccessTime.ToString();
			label12.Text = di.LastWriteTime.ToString();

			listBox2.Items.Clear();
			foreach (FileInfo fi in di.GetFiles())
			{
				listBox2.Items.Add(fi);
			}

			e.Node.Collapse();
			e.Node.Parent.EnsureVisible();
			e.Node.EnsureVisible();
			
		}

		private void listBox2_SelectedValueChanged(object sender, EventArgs e)
		{
			FileInfo di = listBox2.SelectedItem as FileInfo;
			if (di == null) return;

			label2.Text = di.GetType().ToString();
			label4.Text = di.Name;

			label6.Text = di.FullName;
			toolTip1.SetToolTip(label6, label6.Text);

			label8.Text = di.CreationTime.ToString();
			label10.Text = di.LastAccessTime.ToString();
			label12.Text = di.LastWriteTime.ToString();
		}
	}
}
