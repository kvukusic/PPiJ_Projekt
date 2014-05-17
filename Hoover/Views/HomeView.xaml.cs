using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hoover.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

		private void startButton_Click(object sender, RoutedEventArgs e)
		{
			Services.NavigationService.Instance.Navigate(Services.PageNames.TestPageViewName);
		}
    }
}
