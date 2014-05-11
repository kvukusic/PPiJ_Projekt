#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hoover__n_Recreate.Services;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace Hoover__n_Recreate.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void TestGrid_OnTap(object sender, GestureEventArgs e)
        {
            Services.NavigationService.Instance.Navigate(PageNames.TestPageViewName);
        }
    }
}