#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Database;
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

        }

        private ObservableCollection<HistoryItems.HistoryViewItem> _HistoryItems;
        public ObservableCollection<HistoryItems.HistoryViewItem> HistoryItems
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
