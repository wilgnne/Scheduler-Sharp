using System;
using Gtk;

namespace SchedulerSharp
{
    static class MainClass
    {
        public static MainWindow win;
        public static void Main(string[] args)
        {
            //Executar GUI apenas se nao houver argumentos
            if (args.Length == 0)
            {
                Application.Init();
                win = new MainWindow();
                win.Show();
                Application.Run();
            }
        }
    }
}
