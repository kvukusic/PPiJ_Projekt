#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace Hoover.Helpers
{
    /// <summary>
    /// This is an abstract class which has a SelectTemplate method that needs to bee implemented.
    /// </summary>
    public abstract class TemplateSelector : ContentControl
    {
        /// <summary>
        /// The method whose logic determines which DataTemplate to select for a list control.
        /// </summary>
        /// <param name="item">The item for which the template is selecting.</param>
        /// <param name="container">The container of the item for which the template is being selected.</param>
        /// <returns>A <see cref="DataTemplate"/> which will represent the <see cref="item"/> in the list control.</returns>
        public abstract DataTemplate SelectTemplate(object item, DependencyObject container);

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}