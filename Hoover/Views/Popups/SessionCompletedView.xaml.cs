#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Hoover.Annotations;
using Hoover.Helpers;
using Hoover.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

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

        #region Commands

        public RelayCommand ShareSessionCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    RaiseCloseRequested();
                    await Task.Delay(TimeSpan.FromMilliseconds(200));

                    ShareStatusTask task = new ShareStatusTask()
                    {
                        Status = "Just ran " + this.Distance + " in " + this.Time + " with average speed of " + this.AverageSpeed + ". It was great!"
                    };
                    task.Show();
                });
            }
        }

        #endregion

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

        #region Events

        public event EventHandler<EventArgs> CloseRequested;
        private void RaiseCloseRequested()
        {
            if (CloseRequested != null)
            {
                CloseRequested(this, new EventArgs());
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
