using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class PageResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public int PageCount => (int)Math.Ceiling(Convert.ToDouble(Count) / Convert.ToDouble(PageSize));
        public List<T> data { get; set; }
    }

}
