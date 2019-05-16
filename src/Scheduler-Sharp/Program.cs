using System;
using Gtk;

namespace SchedulerSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
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
