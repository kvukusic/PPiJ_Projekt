#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Weather
{
    public class City : INotifyPropertyChanged
    {
        private string _CityName;
        private string _Latitude;
        private string _Longitude;

        public string CityName
        {
            get
            {
                return _CityName;
            }
            set
            {
                if (value != _CityName)
                {
                    _CityName = value;
                    NotifyPropertyChanged("CityName");
                }
            }
        }

        public string Latitude
        {
            get
            {
                return _Latitude;
            }
            set
            {
                if (value != _Latitude)
                {
                    _Latitude = value;
                    NotifyPropertyChanged("Latitude");
                }
            }
        }

        public string Longitude
        {
            get
            {
                return _Longitude;
            }
            set
            {
                if (value != _Longitude)
                {
                    _Longitude = value;
                    NotifyPropertyChanged("Longitude");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Constructor with full city information
        /// </summary>
        public City(string cityName, string latitude, string longitude)
        {
            CityName = cityName;
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Raise the PropertyChanged event and pass along the property that changed
        /// </summary>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}