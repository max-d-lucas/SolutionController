namespace SolutionMonitor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Common.Logging;
    using Models;
    using Newtonsoft.Json;

    public partial class SolutionManager : Form
    {
        private readonly ILog _Logger;

        public SolutionManager(ILogManager logManager)
        {
            this._Logger = logManager.GetLogger<SolutionManager>();
            InitializeComponent();
        }

        private void addToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var result = this.folderBrowserDialog1.ShowDialog();
            if(result != DialogResult.OK)
            {
                return;
            }
            this.MonitoredFolders.Items.Add(this.folderBrowserDialog1.SelectedPath);
        }

        private void deleteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.MonitoredFolders.Items.Remove(this.MonitoredFolders.SelectedItem);
        }

        private void openControlFileToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var result = this.openFileDialog1.ShowDialog();
            if(result != DialogResult.OK)
            {
                return;
            }
            this.ControlFile.Text = this.openFileDialog1.FileName;
            ControlFileRead(this.ControlFile.Text);
        }

        private void newControlFileToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var result = this.saveFileDialog1.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            this.ControlFile.Text = this.openFileDialog1.FileName;
            var controlFile = new ControlFile();
            this.ControlFileSave(this.ControlFile.Text, controlFile);
        }
        private void saveControlFileToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(this.ControlFile.Text))
            {
                return;
            }
            var controlFile = new ControlFile();
            this.ControlFileSave(this.ControlFile.Text, controlFile);
        }

        private ControlFile ControlFileRead(string fileName)
        {
            var fileContents = File.ReadAllText(fileName);
            if(string.IsNullOrWhiteSpace(fileContents))
            {
                return new ControlFile();
            }
            try
            {
                var controlFile = JsonConvert.DeserializeObject<ControlFile>(fileContents);
                ControlFileLoadToUi(controlFile);
                return controlFile;
            }
            catch (Exception ex)
            {
                this._Logger.ErrorFormat("Unable to load {0}. Check log file for more details", ex, fileName);
            }
            return null;
        }
        private void ControlFileLoadToUi(ControlFile controlFile)
        {
            this.MonitoredFolders.Items.Clear();
            this.MonitoredFolders.Items.AddRange(controlFile.MonitoredFolders.ToArray<object>());
        }
        private void ControlFileLoadFromUi(ControlFile controlFile)
        {
            controlFile.MonitoredFolders.Clear();
            foreach(var item in this.MonitoredFolders.Items)
            {
                controlFile.MonitoredFolders.Add(item.ToString());
            }
        }

        private void ControlFileSave(string fileName,  ControlFile controlFile)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(controlFile));
            }
            catch (Exception ex)
            {
                this._Logger.ErrorFormat("Unable to save {0}. Check log file for more details", ex, fileName);
            }
        }

    }
}
