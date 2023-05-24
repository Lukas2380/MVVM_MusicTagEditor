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
        /// <summary>
        /// The ChangeDirectory method creates a dialog for the user to choose a directory.
        /// </summary>
        /// <param name="directory">The correct directory.</param>
        /// <returns>True, if the user chose a correct directory, False if not.</returns>
        public static bool ChangeDirectory(out string directory)
        {
            // Code for me cuz faster, make sure to remove in final project
            string dir = "C:\\Users\\lukas\\Music\\";
            if (System.Environment.MachineName == "LUKAS-DESKTOP")
            {
                dir = "D:\\Music\\";
            }

            directory = null;
            // Let user pick folder 
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = dir;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directory = dialog.FileName;
                return true;
            }
            return false;
        }
    }
}
