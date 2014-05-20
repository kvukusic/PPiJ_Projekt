#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Hoover.Views.HistoryItems
{
    public class HistoryItem
    {
        private Model.HistoryItem _model;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model"></param>
        public HistoryItem(Model.HistoryItem model)
        {
            this._model = model;
        }

        /// <summary>
        /// Constructor for header.
        /// </summary>
        /// <param name="title"></param>
        public HistoryItem(string title)
        {
            this.IsHeader = true;
            this.Title = title;
        }

        public string AverageSpeed { get; set; }
        public string TotalTime { get; set; }
        public string Distance { get; set; }

        public bool IsHeader { get; set; }
        public string Title { get; set; }
    }
}