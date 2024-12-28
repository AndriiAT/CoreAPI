using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services
{
    internal class ListsComparer : IListsComparer
    {
        public ListsComparResult<T> Compare<T, TKey>(List<T> list1, List<T> list2, Func<T, TKey> keySelector)
        {
            var result = new ListsComparResult<T>();
            var list1Keys = list1.Select(keySelector).ToList();
            var list2Keys = list2.Select(keySelector).ToList();
            result.Added = list2.Where(x => !list1Keys.Contains(keySelector(x))).ToList();
            result.Removed = list1.Where(x => !list2Keys.Contains(keySelector(x))).ToList();
            result.Updated = list1.Where(x => list2Keys.Contains(keySelector(x))).ToList();
            return result;
        }
    }
}
