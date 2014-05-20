#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hoover.Model;
using Wintellect.Sterling.Database;

#endregion

namespace Hoover.Database
{
    /// <summary>
    /// This is the My Games database instance for Sterling DB.
    /// </summary>
    public class HistoryDatabaseInstance : BaseDatabaseInstance
    {
        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>
                       {
                           CreateTableDefinition<HistoryItem, long>(item => item.ID),
                       };
        }
    }
}