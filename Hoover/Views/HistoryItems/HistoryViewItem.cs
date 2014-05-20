#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hoover.Helpers;

#endregion

namespace Hoover.Views.HistoryItems
{
    public class HistoryViewItem
    {
        private Model.HistoryItem _model;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model"></param>
        public HistoryViewItem(Model.HistoryItem model)
        {
            this._model = model;

            this.StartDate = model.StartTime.ToString("d");
            this.StartTime = model.StartTime.ToString("t");
            this.AverageSpeed = model.AverageSpeed.Speed();
            this.TotalTime = (model.EndTime - model.StartTime).ToString();
            this.Distance = model.RouteLength.Length();
        }

        /// <summary>
        /// Constructor for header.
        /// </summary>
        /// <param name="title"></param>
        public HistoryViewItem(string title)
        {
            this.IsHeader = true;
            this.Title = title;
        }

        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string AverageSpeed { get; set; }
        public string TotalTime { get; set; }
        public string Distance { get; set; }

        public bool IsHeader { get; set; }
        public string Title { get; set; }
    }
}