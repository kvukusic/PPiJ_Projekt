#region

using System;
using System.Collections.Generic;
using System.Device.Location;
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
            var totalTime = model.EndTime - model.StartTime;
            var newTotalTime = new TimeSpan(totalTime.Hours, totalTime.Minutes, totalTime.Seconds);
            this.TotalTime = newTotalTime.ToString();
            this.Distance = model.RouteLength.Length();

            this.Waypoints = model.Checkpoints;
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

        public List<GeoCoordinate> Waypoints { get; set; }

        public Model.HistoryItem HistoryItem { get { return _model; } }
    }
}