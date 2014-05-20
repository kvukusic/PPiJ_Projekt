#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Views.HistoryItems;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
    public partial class HistoryDetailsPage : PhoneApplicationPage
    {
        /// <summary>
        /// The item for this details view.
        /// </summary>
        private HistoryViewItem _historyItem;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HistoryDetailsPage()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                var parameter = Services.NavigationService.Instance.GetLastNavigationParameter<HistoryViewItem>();

                if (parameter != null)
                {
                    _historyItem = parameter;
                }

                if (_historyItem != null)
                {

                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}