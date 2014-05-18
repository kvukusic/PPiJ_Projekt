using GART.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoover.Common
{
	class CheckpointItem : ARItem
	{
		private string _description;

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
				NotifyPropertyChanged("_description");
			}
		}
	}
}
