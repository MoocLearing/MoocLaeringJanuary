using Mooc.Data.Entities;
using System.Collections.Generic;

namespace Mooc.Data.ViewModels
{
    public class CategoryViewModel
    {
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public SubjectCategory SubjectCategory { get; set; }
    }
}
