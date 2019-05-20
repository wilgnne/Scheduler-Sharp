using System;
using Gtk;

namespace SchedulerSharp.GUI
{
    public static class GTKUtils
    {
        public static void ShowDilog(string Title, MessageType type, string messege)
        {
            MessageDialog dialog = new MessageDialog
            (MainClass.win,
                DialogFlags.Modal,
                type,
                ButtonsType.Ok,
                messege)
            {
                Title = Title
            };

            ResponseType response = (ResponseType)dialog.Run();

            if (response == ResponseType.Ok || response == ResponseType.DeleteEvent)
            {
                dialog.Destroy();
            }
        }

        public static bool ShowFileChooser(out string path, string Title, string confirm, string cancel = "Cancelar")
        {
            FileFilter filter = new FileFilter
            {
                Name = "JSON"
            };
            filter.AddPattern("*.json");

            path = null;
            FileChooserDialog saveDialog = new FileChooserDialog(Title, null,
                FileChooserAction.Save, cancel, ResponseType.Cancel, confirm,
                ResponseType.Accept)
            {
                Filter = filter
            };
            if (saveDialog.Run() == (int)Gtk.ResponseType.Accept)
            {
                path = saveDialog.Filename;

                if (saveDialog.Filter.Name == "JSON" && saveDialog.Filename.ToLower().
                    Trim().EndsWith(".json", StringComparison.CurrentCulture) == false)
                {
                    path = saveDialog.Filename + ".json";
                }
                return true;
            }

            saveDialog.Destroy();
            Console.WriteLine("Path: " + path);
            return false;
        }
    }
}
