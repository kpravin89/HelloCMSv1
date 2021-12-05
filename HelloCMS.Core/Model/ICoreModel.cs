using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloCMS.Core.Model
{
    public interface ICoreModel
    {
        bool? deleted_fg { get; set; }
        bool? hold_fg { get; set; }
        DateTime? update_dt { get; set; }
        DateTime insert_dt { get; set; }
        string ss_id { get; set; }
        int Last_User_ID { get; set; }
    }
}
