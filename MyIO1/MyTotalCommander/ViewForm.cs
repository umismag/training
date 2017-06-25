using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTotalCommander
{
    public partial class ViewForm : Form
    {
        public ViewForm()
        {
            InitializeComponent();
        }

        public string MyTextBox
        {
            set
            {
                textBox1.Text = value;
            }
            get
            {
                return textBox1.Text;
            }
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (textBox1.)
            //totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0].FullName
        }
    }
}
