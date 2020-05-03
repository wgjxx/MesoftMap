using System;
using System.Collections.Generic;
using System.Text;

namespace Mesoft.Interface
{
    public class PageResult<T>
    {
        public List<T> DataList { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
