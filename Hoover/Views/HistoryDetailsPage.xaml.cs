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
using Hoover.Helpers;

#endregion

namespace Hoover.Views
{
    public partial class HistoryDetailsPage : PhoneApplicationPage
	{

		#region Fields and Properties

		/// <summary>
        /// The item for this details view.
        /// </summary>
        private HistoryViewItem _historyItem;

		public string DateOfRoute
		{
			get
			{
				return _historyItem.StartDate;
			}
		}

		public string AverageSpeed
		{
			get
			{
				return _historyItem.AverageSpeed;
			}
		}

		public string RouteLength
		{
			get
			{
				return _historyItem.Distance;
			}
		}

		public string RuningTime
		{
			get
			{
				return _historyItem.TotalTime;
			}
		}

		#endregion


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