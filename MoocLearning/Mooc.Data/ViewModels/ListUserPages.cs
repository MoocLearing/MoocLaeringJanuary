using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class ListUserPages
    {
        public IEnumerable<User> users { get; set; }

        [DefaultValue(1)]
        public int TotalPage { get; set; }

        [DefaultValue(1)]
        public int TotalCount { get; set; }

        [DefaultValue(1)]
        public int CurrentPage { get; set; }

        [DefaultValue(3)]
        public int PageSize { get; set; }
    }
}
