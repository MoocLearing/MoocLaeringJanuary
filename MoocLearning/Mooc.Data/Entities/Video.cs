using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Video:BaseEntity
    {
        public string VideoName { get; set; }

        public string VideoLocation { get; set; }
    }
}
