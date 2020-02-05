using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;

namespace Mooc.Data.ViewModels
{
    public class ListTeacherPage
    {
        public IEnumerable<Teacher> teachers { get; set; }
        
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
