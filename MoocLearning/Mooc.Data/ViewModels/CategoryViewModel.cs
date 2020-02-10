using Mooc.Data.Entities;
using System.Collections.Generic;

namespace Mooc.Data.ViewModels
{
    public class CategoryViewModel : Category
    {
        //public IEnumerable<CategoryType> CategoryTypes { get; set; }
        //public SubjectCategory SubjectCategory { get; set; }


        public CategoryViewModel()
        {
            CategryList = new List<Category>();
        }
        public IEnumerable<Category> CategryList { get; set; }


    }

}
