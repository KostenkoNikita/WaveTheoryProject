using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTheoryProject
{
    static class Extensions
    {
        public static bool IsAllThreadsCompleted(this List<IAsyncResult> l)
        {
            foreach (IAsyncResult i in l)
            {
                if (!i.IsCompleted) { return false; }
            }
            return true;
        }
    }
}
