#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Settings;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            this.DataContext = ApplicationSettings.Instance;
        }

        private void FacebookToogleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            FacebookLogin.Visibility = System.Windows.Visibility.Visible;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FacebookToogleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            FacebookLogin.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}
