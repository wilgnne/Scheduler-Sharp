using System;
using Gtk;

namespace SchedulerSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Executar GUI apenas se nao houver argumentos
            if (args.Length == 0)
            {
                Application.Init();
                MainWindow win = new MainWindow();
                win.Show();
                Application.Run();
            }
        }
    }
}
