using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class ChapterViewDto
    {
        public long ID { get; set; }
        public string ChapterName { get; set; }

        public string VideoGuid { get; set; }

        public long CourseId { get; set; }
    }
}
