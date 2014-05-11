#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover__n_Recreate.Weather
{
    public class Cities : ObservableCollection<City>
    {
        public Cities() { }

        /// <summary>
        /// Create a default list of cities and their latitudes and longitudes
        /// </summary>
        public void LoadDefaultData()
        {
            //App.cityList.Add(new City("Zagreb", "45.8", "15.98"));
        }

    }
}