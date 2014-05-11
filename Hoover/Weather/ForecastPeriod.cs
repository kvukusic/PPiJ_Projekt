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
    /// <summary>
    /// Class for holding the forecast for a particular time period
    /// </summary>
    public class ForecastPeriod : INotifyPropertyChanged
    {
        #region member variables
        private string timeName;
        private int temperature;
        private int chancePrecipitation;
        private string weatherType;
        private string textForecast;
        private string conditionIcon;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public ForecastPeriod()
        {
        }


        public string TimeName
        {
            get
            {
                return timeName;
            }
            set
            {
                if (value != timeName)
                {
                    this.timeName = value;
                    NotifyPropertyChanged("TimeName");
                }
            }
        }

        public int Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if (value != temperature)
                {
                    this.temperature = value;
                    NotifyPropertyChanged("Temperature");
                }
            }
        }


        public int ChancePrecipitation
        {
            get
            {
                return chancePrecipitation;
            }
            set
            {
                if (value != chancePrecipitation)
                {
                    this.chancePrecipitation = value;
                    NotifyPropertyChanged("ChancePrecipitation");
                }
            }
        }

        public string WeatherType
        {
            get
            {
                return weatherType;
            }
            set
            {
                if (value != weatherType)
                {
                    this.weatherType = value;
                    NotifyPropertyChanged("WeatherType");
                }
            }
        }

        public string TextForecast
        {
            get
            {
                return textForecast;
            }
            set
            {
                if (value != textForecast)
                {
                    this.textForecast = value;
                    NotifyPropertyChanged("TextForecast");
                }
            }
        }

        public string ConditionIcon
        {
            get
            {
                return conditionIcon;
            }
            set
            {
                if (value != conditionIcon)
                {
                    this.conditionIcon = value;
                    NotifyPropertyChanged("ConditionIcon");
                }
            }
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}