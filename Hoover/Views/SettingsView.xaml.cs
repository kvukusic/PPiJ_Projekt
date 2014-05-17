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
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            this.DataContext = ApplicationSettings.Instance;
        }
    }
}
