#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Charting;
using Telerik.Windows.Controls;

#endregion

namespace Hoover.TemplateSelectors
{
    public class LinePointTemplateSelector : DataTemplateSelector
    {
        private DataTemplate filledTemplate;
        private DataTemplate emptyTemplate;

        public DataTemplate FilledTemplate
        {
            get
            {
                return this.filledTemplate;
            }
            set
            {
                this.filledTemplate = value;
            }
        }

        public DataTemplate EmptyTemplate
        {
            get
            {
                return this.emptyTemplate;
            }
            set
            {
                this.emptyTemplate = value;
            }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            int pointIndex = (item as CategoricalDataPoint).CollectionIndex;
            CategoricalSeries series = container as CategoricalSeries;
            if (pointIndex == 0 || pointIndex == series.DataPoints.Count - 1)
            {
                return this.filledTemplate;
            }

            return this.emptyTemplate;
        }
    }
}