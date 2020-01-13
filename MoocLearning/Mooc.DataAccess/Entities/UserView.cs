using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.DataAccess.Entities
{
    public class UserView
    {
       public IEnumerable<Country> Countries { get; set; }
       
       public User User { get; set; }

       public string PasswordCheck { get; set; }

    
    }
}
