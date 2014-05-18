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
using Hoover.Model.Weather;
using Hoover.Services;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion

namespace Hoover.Views
{
	public partial class StatisticsView : UserControl, INotifyPropertyChanged
	{
		public StatisticsView()
		{
			InitializeComponent();

		    this.DataContext = this;
            this.Loaded += OnLoaded;
		}

	    private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	    {
            
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
