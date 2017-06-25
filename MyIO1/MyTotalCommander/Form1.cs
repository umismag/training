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

namespace MyTotalCommander
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TotCommPanel leftPanel = new TotCommPanel();
            //TotCommPanel rightPanel = new TotCommPanel();
            //leftPanel.Parent = this.splitContainer1.Panel1;
            //rightPanel.Parent = this.splitContainer1.Panel2;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            int buttonWidth;
            buttonWidth = (panel1.Width) / 6;
            foreach (Button bt in panel1.Controls)
            {
                bt.Width = buttonWidth;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //F3
            bool isFileInfoType = totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0] is FileInfo;
            if (!isFileInfoType)
                return;
            ViewForm vf = new ViewForm();
            vf.MyTextBox = File.ReadAllText(totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0].FullName);
            vf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //F4
            bool isFileInfoType = totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0] is FileInfo;
            if (!isFileInfoType)
                return;
            ViewForm vf = new ViewForm();

            vf.MyTextBox = File.ReadAllText(totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0].FullName);
            //vf.MyTextBox = ".fm.mf.mf.mf";
            vf.Show();

            //StreamWriter sw = new StreamWriter(totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0].FullName);

            vf.FormClosing += (x,y) =>
              {
                  if (MessageBox.Show("Записати зміни?","Збереження змін",MessageBoxButtons.YesNo)==DialogResult.Yes)

                  File.WriteAllText(totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0].FullName, vf.MyTextBox);
                  //sw.WriteLine(vf.MyTextBox);
              };
            //FileStream fs = new FileStream(totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0].FullName,FileMode.Open);
            //FileStream fs = ((FileInfo)totCommPanel1.foldersFilesPanel.selectedFoldersFiles[0]).Open(FileMode.Open);
            //fs.Read()

            //fs.Close();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            //F5
            void CopyFolder(DirectoryInfo SourceDir, DirectoryInfo DestDir)
            {
                DirectoryInfo di = new DirectoryInfo(DestDir.FullName + "\\" + SourceDir.Name);
                if (di.Exists == false)
                    di.Create();

                foreach (FileSystemInfo fsi in SourceDir.GetFileSystemInfos())
                {
                    if (fsi is DirectoryInfo)
                    {
                        CopyFolder(((DirectoryInfo)fsi),di);
                    }
                    else if (fsi is FileInfo)
                        ((FileInfo)fsi).CopyTo(di.FullName + "\\" + fsi.Name);
                }
            }

            MooveForm mf = new MooveForm();
            mf.SourceFolder = totCommPanel1.CurrentDirectory.FullName;
            mf.DestFolder = totCommPanel2.CurrentDirectory.FullName;

            if (mf.ShowDialog() != DialogResult.OK)
                return;

            if (totCommPanel1.CurrentDirectory.FullName == mf.DestFolder)
            {
                MessageBox.Show("Не можна копіювати директорію саму у себе!");
                return;
            }

            foreach (FileSystemInfo fs in totCommPanel1.foldersFilesPanel.selectedFoldersFiles)
            {
                if (fs is DirectoryInfo)
                    CopyFolder( (DirectoryInfo)fs, new DirectoryInfo(mf.DestFolder) );
                else if (fs is FileInfo)
                    ((FileInfo)fs).CopyTo(mf.DestFolder + "\\" + fs.Name);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //F6
            MooveForm mf = new MooveForm();
            mf.SourceFolder = totCommPanel1.CurrentDirectory.FullName;
            mf.DestFolder = totCommPanel2.CurrentDirectory.FullName;

            if (mf.ShowDialog() != DialogResult.OK)
                return;
            
            if (totCommPanel1.CurrentDirectory.FullName == mf.DestFolder)
            {
                MessageBox.Show("Не можна копіювати директорію саму у себе!");
                return;
            }

            foreach (FileSystemInfo fs in totCommPanel1.foldersFilesPanel.selectedFoldersFiles)
            {
                if (fs is DirectoryInfo)
                    ((DirectoryInfo)fs).MoveTo(mf.DestFolder+"\\"+fs.Name);
                else if (fs is FileInfo)
                    ((FileInfo)fs).MoveTo(mf.DestFolder + "\\" + fs.Name);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //F7
            MooveForm mf = new MooveForm();
            mf.SourceFolder = totCommPanel1.CurrentDirectory.FullName;
            mf.DestFolder = "";
            if (mf.ShowDialog() != DialogResult.OK)
                return;

            DirectoryInfo di = new DirectoryInfo(totCommPanel1.CurrentDirectory.FullName + "\\" + mf.DestFolder);
            if (!di.Exists)
                di.Create();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //F8
            MooveForm mf = new MooveForm();
            mf.SourceFolder = totCommPanel1.CurrentDirectory.FullName;
            mf.DestFolder = "Видалити вибрані елементи?";
            if (mf.ShowDialog() != DialogResult.OK)
                return;

            

            foreach (FileSystemInfo fs in totCommPanel1.foldersFilesPanel.selectedFoldersFiles)
            {

                if (fs is DirectoryInfo)
                    ((DirectoryInfo)fs).Delete(true);
                else if (fs is FileInfo)
                    ((FileInfo)fs).Delete();
            }
        }
    }

    class LogicalDrivesPanel : Panel //Панель вибору логічного диску та відображення інформації про нього
    {
        //Список логічних дисків
        ComboBox comboAllLogicalDrives = new ComboBox();

        //відображення інформації про вибраний логічний диск
        Label driveInfoLabel = new Label();
        public event Action<DriveInfo> currentLogicalDriveChanged;

        //Поточний логічний диск
        DriveInfo currentLogicalDrive; // Поле
        public DriveInfo CurrentLogicalDrive  // Властивість
        {
            get { return currentLogicalDrive; }
            set
            {
                if (value is DriveInfo)
                {
                    currentLogicalDrive = value;
                    driveInfoLabel.Text = "[" + currentLogicalDrive.VolumeLabel + "]  \\  " +
                        currentLogicalDrive.AvailableFreeSpace + " із " +
                        currentLogicalDrive.TotalSize + " вільно";
                    if (currentLogicalDriveChanged != null)
                        currentLogicalDriveChanged(value);
                }
            }
        }

        public LogicalDrivesPanel() //Конструктор без параметрів
        {
            this.Dock = DockStyle.Top;
            this.Height = comboAllLogicalDrives.Height + 2;
            this.BackColor = Color.LightBlue;
            comboAllLogicalDrives.Parent = this;
            comboAllLogicalDrives.Left = 1;
            comboAllLogicalDrives.Width = 50;
            comboAllLogicalDrives.Top = 1;
            comboAllLogicalDrives.SelectedIndexChanged += (x, y) => CurrentLogicalDrive = comboAllLogicalDrives.SelectedItem as DriveInfo;
            //comboAllLogicalDrives_SelectedIndexChanged;

            driveInfoLabel.Height = 18;

            //Формування списку логічних дисків
            foreach (DriveInfo drv in DriveInfo.GetDrives())
            {
                //AllLogicalDrives.Add(drv);
                comboAllLogicalDrives.Items.Add(drv);
            }
            comboAllLogicalDrives.Text = comboAllLogicalDrives.Items[0].ToString();
            CurrentLogicalDrive = comboAllLogicalDrives.Items[0] as DriveInfo;

            driveInfoLabel.AutoSize = true;
            driveInfoLabel.Parent = this;
            driveInfoLabel.Left = comboAllLogicalDrives.Width + 20;

        }
    }

    class FoldersFilesPanel : Panel
    {
        DirectoryInfo currentDirectory;
        public DirectoryInfo CurrentDirectory
        {
            get => currentDirectory;
            set
            {
                if (value.Exists & value != null)
                {
                    currentDirectory = value;
                    AddAllToListBox();
                    currentDirectoryLabel.Text = currentDirectory.FullName;
                }
            }
        }

        public List<FileSystemInfo> selectedFoldersFiles = new List<FileSystemInfo>();

        Label currentDirectoryLabel = new Label();

        ListBox FoldersFilesListBox = new ListBox();

        public FoldersFilesPanel() //Конструктор безпараметрів
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.LightGreen;

            //FoldersFilesListBox.Anchor = AnchorStyles.Bottom & AnchorStyles.Top;
            FoldersFilesListBox.Dock = DockStyle.Fill;
            FoldersFilesListBox.Parent = this;
            FoldersFilesListBox.DrawMode = DrawMode.OwnerDrawVariable;
            FoldersFilesListBox.ItemHeight = 19;

            currentDirectoryLabel.Dock = DockStyle.Top;
            currentDirectoryLabel.Height = 19;
            currentDirectoryLabel.Parent = this;

            FoldersFilesListBox.DoubleClick += (x, y) =>
            {
                DirectoryInfo di = FoldersFilesListBox.SelectedItem as DirectoryInfo;
                if (di == null || !di.Exists)
                    return;
                CurrentDirectory = di;
                selectedFoldersFiles.Clear();
            };

            FoldersFilesListBox.DrawItem += (sender, e) =>
              {
                  e.DrawBackground();

                  string itemText = "Error!!!";
                  if (e.Index < 0) return;

                  if (FoldersFilesListBox.Items[e.Index] is DirectoryInfo)
                  {
                      bool isParentDirectory = e.Index == 0 && CurrentDirectory.Parent != null;
                      if (isParentDirectory)
                          itemText = "[..]";
                      else
                          itemText = "[" + ((DirectoryInfo)FoldersFilesListBox.Items[e.Index]).Name + "]";
                  }
                  else
                  {
                      itemText = (FoldersFilesListBox.Items[e.Index] as FileInfo).Name ?? "Error!";
                  }

                  e.Graphics.DrawString(itemText, e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                  e.DrawFocusRectangle();
              };

            FoldersFilesListBox.SelectedIndexChanged += (sender, e) =>
             {
                 selectedFoldersFiles.Clear();//***************************
                 if (FoldersFilesListBox.SelectedItems.Count > 0)
                     foreach (FileSystemInfo fsi in FoldersFilesListBox.SelectedItems)
                         selectedFoldersFiles.Add(fsi);
             };

            FoldersFilesListBox.SelectionMode = SelectionMode.MultiExtended;
        }

        public FoldersFilesPanel(DirectoryInfo CurrentDirectory) : this() //Конструктор
        {
            if (CurrentDirectory.Exists)
            {
                this.CurrentDirectory = CurrentDirectory;
            }
            else
                this.CurrentDirectory = null;
        }

        void AddFolders()
        {
            try
            {
                DirectoryInfo[] fl = CurrentDirectory.GetDirectories();
                for (int i = 0; i < fl.Length; i++)
                {
                    FoldersFilesListBox.Items.Add(fl[i]);
                }
            }
            catch (Exception)
            {
                //el.ForeColor = Color.LightGray;
            }
        }

        void AddFiles()
        {
            try
            {
                foreach (FileInfo fi in CurrentDirectory.GetFiles())
                {
                    FoldersFilesListBox.Items.Add(fi);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        void AddAllToListBox()
        {
            FoldersFilesListBox.Items.Clear();
            if (CurrentDirectory.Parent != null & CurrentDirectory.Exists)
            {
                FoldersFilesListBox.Items.Add(CurrentDirectory.Parent);
            }

            AddFolders();
            AddFiles();
        }

    }

    class TotCommPanel : Panel
    {

        public LogicalDrivesPanel driveInfoPanel = new LogicalDrivesPanel();
        public FoldersFilesPanel foldersFilesPanel = new FoldersFilesPanel();

        //Поточна директорія
        //DirectoryInfo currentDirectory;
        public DirectoryInfo CurrentDirectory
        {
            get => foldersFilesPanel.CurrentDirectory;// currentDirectory;
            set
            {
                if (value.Exists & value != null)
                {
                    //currentDirectory = value;
                    foldersFilesPanel.CurrentDirectory = value;

                }
            }
        }


        public TotCommPanel()
        {
            this.Dock = DockStyle.Fill;

            CurrentDirectory = new DirectoryInfo(driveInfoPanel.CurrentLogicalDrive.Name);

            foldersFilesPanel.Top = driveInfoPanel.Height + 2;
            driveInfoPanel.currentLogicalDriveChanged += (drv) => foldersFilesPanel.CurrentDirectory = drv.RootDirectory;

            foldersFilesPanel.Parent = this;
            driveInfoPanel.Parent = this;

            //

        }

    }


}
