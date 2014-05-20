#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Hoover.Annotations;
using Hoover.Database;
using Hoover.Helpers;
using Hoover.Services;
using Hoover.Views.HistoryItems;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
    public partial class HistoryView : UserControl, INotifyPropertyChanged
    {
        public HistoryView()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Get the history items from the database
            var historyItems = App.DataAccess.GetAllHistoryItems();
            var resultList = new ObservableCollection<HistoryViewItem>();
            DateTime? currentDate = null;
            foreach (var historyItem in historyItems)
            {
                if (!historyItem.StartTime.Date.Equals(currentDate))
                {
                    currentDate = historyItem.StartTime.Date;
                    resultList.Add(new HistoryViewItem(historyItem.StartTime.ToString("D")));
                }

                resultList.Add(new HistoryViewItem(historyItem));
            }

            this.HistoryItems = resultList;
        }
        
        /// <summary>
        /// This command is executed when a history item is selected from the list,
        /// and it navigates to the <see cref="HistoryDetailsPage"/> and passes the specified
        /// item as the <see cref="System.Windows.Navigation.NavigationService"/> parameter.
        /// </summary>
        public RelayCommand<HistoryViewItem> NavigateToHistoryDetailsCommand
        {
            get
            {
                return new RelayCommand<HistoryViewItem>(new Action<HistoryViewItem>(item =>
                {
                    Services.NavigationService.Instance.Navigate(PageNames.HistoryDetailsPageName, item);
                }));
            }
        }

        private ObservableCollection<HistoryViewItem> _HistoryItems;
        public ObservableCollection<HistoryViewItem> HistoryItems
        {
            get { return _HistoryItems; }
            set
            {
                if (value != _HistoryItems)
                {
                    _HistoryItems = value;
                    OnPropertyChanged("HistoryItems");
                }
            }
        }

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
