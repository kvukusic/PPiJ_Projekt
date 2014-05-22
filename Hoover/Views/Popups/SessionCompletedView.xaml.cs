#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Helpers;
using Hoover.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views.Popups
{
    public partial class SessionCompletedView : UserControl, INotifyPropertyChanged
    {
        private HistoryItem _item;

        public SessionCompletedView(HistoryItem item)
        {
            InitializeComponent();

            this._item = item;

            this.DataContext = this;

            // Initialize properties
            this.AverageSpeed = item.AverageSpeed.Speed();
            this.Distance = item.RouteLength.Length();
            this.Time = (item.EndTime - item.StartTime).TimeSpanFormatString();
        }

        #region Properties

        private string _AverageSpeed;
        public string AverageSpeed
        {
            get { return _AverageSpeed; }
            set
            {
                if (value != _AverageSpeed)
                {
                    _AverageSpeed = value;
                    OnPropertyChanged("AverageSpeed");
                }
            }
        }

        private string _Distance;
        public string Distance
        {
            get { return _Distance; }
            set
            {
                if (value != _Distance)
                {
                    _Distance = value;
                    OnPropertyChanged("Distance");
                }
            }
        }

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set
            {
                if (value != _Time)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        #endregion

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
