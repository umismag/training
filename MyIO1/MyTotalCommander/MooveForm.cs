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
    public partial class MooveForm : Form
    {
        public MooveForm()
        {
            InitializeComponent();
            
        }

        public string DestFolder
        {
            get { return DestTextBox.Text; }
            set { DestTextBox.Text = value; }
        }

        public string SourceFolder
        {
            set { SourceFolderLabel.Text = value; }
        }
    }
}
