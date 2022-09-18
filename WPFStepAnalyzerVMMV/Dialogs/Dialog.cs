using Microsoft.Win32;
using System.Windows;

namespace WPFStepAnalyzerVMMV.Dialogs
{
    public class Dialog
    {
        public Dialog()
        {
        }

        public string FilePath { get; set; }
        public string[] FilesPath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                FilesPath = openFileDialog.FileNames;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog(string ext,string filter,string defName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = defName;
            saveFileDialog.Filter = filter;
            saveFileDialog.DefaultExt = ext;
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
