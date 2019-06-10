using System;
namespace SchedulerSharp
{
    public partial class AboutWindows : Gtk.Window
    {
        public AboutWindows() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
        }
    }
}
