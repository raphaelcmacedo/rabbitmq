using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue
{
    public static class Util
    {
        public static string HandleQueueName(string queue, bool durable)
        {
            if (string.IsNullOrEmpty(queue))
            {
                if (durable)
                {
                    queue = "ha.prionDurable";
                }
                else
                {
                    queue = "ha.prion";
                }       
            }

            if (!queue.StartsWith("ha."))
            {
                queue = "ha." + queue;
            }

            return queue;
        }
    }
}
