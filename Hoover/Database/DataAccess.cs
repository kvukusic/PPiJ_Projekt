#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hoover.Model;
using Wintellect.Sterling;
using Wintellect.Sterling.IsolatedStorage;

#endregion

namespace Hoover.Database
{
    public class DataAccess
    {
        /// <summary>
        /// The history database instance.
        /// </summary>
        private ISterlingDatabaseInstance _historyDatabase;

        /// <summary>
        /// The database engine.
        /// </summary>
        private SterlingEngine _engine;

        /// <summary>
        /// Activates the <c>Sterling</c> database engine if already not activated.
        /// </summary>
        public void ActivateEngine()
        {
            _engine = new SterlingEngine();
            _engine.Activate();
            _historyDatabase = _engine.SterlingDatabase.RegisterDatabase<HistoryDatabaseInstance>(new IsolatedStorageDriver());
        }

        /// <summary>
        /// Deactivates the <c>Sterling</c> database engine.
        /// </summary>
        public void DeactivateEngine()
        {
            _engine.Dispose();
            _historyDatabase = null;
            _engine = null;
        }

        /// <summary>
        /// Add the given <see cref="HistoryItem"/> to the database.
        /// </summary>
        /// <param name="item"></param>
        public void AddHistoryItem(HistoryItem item)
        {
            if (item == null)
            {
                return;
            }

            _historyDatabase.Save<HistoryItem>(item);
            _historyDatabase.Flush();
        }

        /// <summary>
        /// Remove the given <see cref="HistoryItem"/> from the database.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveHistoryItem(HistoryItem item)
        {
            if (item == null)
            {
                return;
            }

            _historyDatabase.Delete(item);
        }

        /// <summary>
        /// Remove a <see cref="HistoryItem"/> with the specified id from the database.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveHistoryItem(int id)
        {
            _historyDatabase.Delete(typeof(HistoryItem), id);
        }

        /// <summary>
        /// Retreives and returns all the stored history items from the database.
        /// </summary>
        /// <returns></returns>
        public List<HistoryItem> GetAllHistoryItems()
        {
            return (from item in _historyDatabase.Query<HistoryItem, long>() select item.LazyValue.Value).ToList();
        } 

    }
}