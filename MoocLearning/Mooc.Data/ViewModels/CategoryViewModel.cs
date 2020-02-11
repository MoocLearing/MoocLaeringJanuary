using Mooc.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
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
