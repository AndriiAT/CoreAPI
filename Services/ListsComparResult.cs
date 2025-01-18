using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services
{
    public class ListsComparResult<T>
    {
        public List<T> Added { get; internal set; }
        public List<T> Removed { get; internal set; }
        public List<T> Updated { get; internal set; }
    }
}
