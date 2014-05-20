#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hoover.Helpers;
using Hoover.Views.WeatherItems;

#endregion

namespace Hoover.TemplateSelectors
{
    /// <summary>
    /// Selects between forecast item and forecast item header.
    /// </summary>
    public class WeatherItemTemplateSelector : TemplateSelector
    {
        /// <summary>
        /// The header template.
        /// </summary>
        public DataTemplate ForeastHeaderTemplate { get; set; }

        /// <summary>
        /// The forecast item template.
        /// </summary>
        public DataTemplate ForecastItemTemplate { get; set; }

        /// <summary>
        /// The method whose logic determines which DataTemplate to select for a list control.
        /// </summary>
        /// <param name="item">The item for which the template is selecting.</param>
        /// <param name="container">The container of the item for which the template is being selected.</param>
        /// <returns>A <see cref="DataTemplate"/> which will represent the <see cref="item"/> in the list control.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is WeatherItem)) return null;

            var weatherItem = item as WeatherItem;
            if (weatherItem.IsHeader) return ForeastHeaderTemplate;
            else return ForecastItemTemplate;
        }
    }
}