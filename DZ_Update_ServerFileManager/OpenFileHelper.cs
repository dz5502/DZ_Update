using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_Update_ServerFileManager
{
    public class OpenFileHelper
    {
        public static bool OpenFile(String title, String filter, ref String filePath)
        {
            // Show Open File dialog
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = filter;
            dlg.Multiselect = false;
            dlg.Title = title;

            dlg.RestoreDirectory = true;
            var result = dlg.ShowDialog();
            if ((result != System.Windows.Forms.DialogResult.OK) &&
                (result != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            filePath = dlg.FileName;
            return true;
        }
        public static bool MultiSelectFile(String title, String filter, List<String> fileLsit)
        {
            // Show Open File dialog
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = filter;
            dlg.Multiselect = true;
            dlg.Title = title;
            dlg.RestoreDirectory = true;
            var result = dlg.ShowDialog();
            if ((result != System.Windows.Forms.DialogResult.OK) &&
                (result != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            fileLsit.AddRange(dlg.FileNames.ToList());
            return true;
        }

        public static bool OpenFile(String title, String filter, String initialDirectory, ref String filePath)
        {
            // Show Open File dialog
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = filter;
            dlg.Multiselect = false;
            dlg.Title = title;
            dlg.InitialDirectory = initialDirectory;
            var result = dlg.ShowDialog();
            if ((result != System.Windows.Forms.DialogResult.OK) &&
                (result != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            filePath = dlg.FileName;
            return true;
        }

        public static bool OpenFileToSave(String title, String filter, ref String filePath)
        {
            // Show Open File dialog
            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.Filter = filter;
            dlg.Title = title;
            dlg.RestoreDirectory = true;
            var result = dlg.ShowDialog();
            if ((result != System.Windows.Forms.DialogResult.OK) &&
                (result != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            filePath = dlg.FileName;
            return true;
        }

        public static bool OpenDir(out String dir)
        {
            dir = String.Empty;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "请选择文件路径";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                dir = folderBrowserDialog.SelectedPath;
                return true;
            }
            return false;
        }
    }
}
