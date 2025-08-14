using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using static JavaAutomation.JabHelpers;
using JavaAutomation;

namespace TestClient
{
   public static class Utils
    {
       
        public static IEnumerable<T> SelectNestedChildren<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (T item in source)
            {
                yield return item;
                foreach (T subItem in SelectNestedChildren(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }

        public static List<AccessibleTreeItem> checkval(AccessibleTreeItem accessibleTreeItem,string name,string role) {

            List<AccessibleTreeItem> pointer = new AccessibleTreeItem[] { accessibleTreeItem }.SelectNestedChildren(x => x.children).Where(t => t.name == name).Where(b => b.role ==role).ToList();
            //List<AccessibleTreeItem> pointer = new AccessibleTreeItem[] { accessibleTreeItem }.SelectNestedChildren(x => x.children).Where(t => t.name == "Username").Where(b => b.role == "text").ToList();
            return pointer;

        }
    }
}
