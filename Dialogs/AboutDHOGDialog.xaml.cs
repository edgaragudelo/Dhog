using System.Diagnostics;
using System.Windows.Navigation;
using Telerik.Windows.Controls;

namespace DHOG_WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutDHOGDialog.xaml
    /// </summary>
    public partial class AboutDHOGDialog : RadWindow
    {
        public AboutDHOGDialog()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
