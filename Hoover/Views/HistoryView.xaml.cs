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
using System.Windows.Media.Animation;
using Hoover.Annotations;
using Hoover.Database;
using Hoover.Helpers;
using Hoover.Services;
using Hoover.Views.HistoryItems;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;

#endregion

namespace Hoover.Views
{
    public partial class HistoryView : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Storyboard for showing the header.
        /// </summary>
        private Storyboard _headerVisibleStoryboard;

        public HistoryView()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += OnLoaded;

            _headerVisibleStoryboard = this.Resources["HeaderVisibleStoryboard"] as Storyboard;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Get the history items from the database
            var historyItems = App.DataAccess.GetAllHistoryItems().OrderBy(item => item.StartTime);
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
                    if (!item.IsHeader)
                    {
                        Services.NavigationService.Instance.Navigate(PageNames.HistoryDetailsPageName, item);
                    }
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

        private HistoryViewItem _HeaderItem;
        public HistoryViewItem HeaderItem
        {
            get { return _HeaderItem; }
            set
            {
                if (value != _HeaderItem)
                {
                    _HeaderItem = value;
                    OnPropertyChanged("HeaderItem");
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
