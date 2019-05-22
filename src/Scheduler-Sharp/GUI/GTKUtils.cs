using System;
using Gtk;

namespace SchedulerSharp.GUI
{
    /// <summary>
    /// Implementações genericas de utilitarios GTK
    /// </summary>
    public static class GTKUtils
    {
        /// <summary>
        /// Mostra uma caixa de dialogo personalizada
        /// </summary>
        /// <param name="Title">Titulo da janela.</param>
        /// <param name="type">Tipo de mensagem.</param>
        /// <param name="messege">Mensagem.</param>
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

        /// <summary>
        /// Exibir um selecionador de arquivo
        /// </summary>
        /// <returns><c>true</c>, se o usuario selecionou um arquivo valido, <c>false</c> se não.</returns>
        /// <param name="path">Retorno do arquivo selecionado.</param>
        /// <param name="extension">Extenção desejada para o arquivo.</param>
        /// <param name="Title">Titulo para a janela.</param>
        /// <param name="confirm">Texto de confirmação.</param>
        /// <param name="cancel">Texto de cancelar.</param>
        public static bool ShowFileChooser(out string path, string extension, string Title, string confirm, string cancel = "Cancelar")
        {
            FileFilter filter = new FileFilter
            {
                Name = "FILTER"
            };
            filter.AddPattern("*"+extension);

            path = null;
            FileChooserDialog saveDialog = new FileChooserDialog(Title, null,
                FileChooserAction.Save, cancel, ResponseType.Cancel, confirm,
                ResponseType.Accept)
            {
                Filter = filter
            };
            if (saveDialog.Run() == (int)ResponseType.Accept)
            {
                path = saveDialog.Filename;

                if (saveDialog.Filter.Name == "FILTER" && saveDialog.Filename.ToLower().
                    Trim().EndsWith(extension, StringComparison.CurrentCulture) == false)
                {
                    path = saveDialog.Filename + extension;
                }
                saveDialog.Destroy();
                Console.WriteLine("Path: " + path);
                return true;
            }

            saveDialog.Destroy();
            Console.WriteLine("Path: " + path);
            return false;
        }
    }
}
