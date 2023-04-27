using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dialog
{
    public class ChangeDirectoryService
    {
        public static bool ChangeDirectory(out string filename)
        {
            // Code for me cuz faster make sure to remove in end project
            string dir = "C:\\Users\\lukas\\Music\\";
            if (System.Environment.MachineName == "LUKAS-DESKTOP")
            {
                dir = "D:\\Music\\";
            }

            filename = null;
            // Let user pick folder 
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = dir;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                filename = dialog.FileName;
                return true;
            }
            return false;
        }
    }
}
