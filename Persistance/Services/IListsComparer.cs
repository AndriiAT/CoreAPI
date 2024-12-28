using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services
{
    public interface IListsComparer
    {
        ListsComparResult<T> Compare<T, TKey>(List<T> list1, List<T> list2, Func<T, TKey> keySelector);
    }
}
