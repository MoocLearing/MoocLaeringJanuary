using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.DataAccess.Entities;

namespace Mooc.DataAccess.ViewModels
{
    public class CategoryViewModel
    {
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public SubjectCategory SubjectCategory { get; set; }
    }
}
