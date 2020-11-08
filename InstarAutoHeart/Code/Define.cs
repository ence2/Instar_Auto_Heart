using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstarAutoHeart
{
    public class TargetData
    {
        public string searchKeyword = "";
        public Queue<string> tags = new Queue<string>();
    }
    public enum WorkerState
    {
        None = 0,
        SettingKeyword = 1,
        CollectingTag = 2,
        WorkHeartChecking = 3,
        Exception = 4,
    }
}
