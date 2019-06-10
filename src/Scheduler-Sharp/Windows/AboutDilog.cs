using System;
namespace SchedulerSharp.Windows
{
    public partial class AboutDilog : Gtk.Dialog
    {
        public AboutDilog(Gtk.Window parent):base("Title", parent, Gtk.DialogFlags.Modal)
        {
            this.Build();;
        }

        protected void CloseEvent(object sender, EventArgs e)
        {
            Destroy();
        }
    }
}
