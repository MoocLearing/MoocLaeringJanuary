using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooctestv01.Models;



namespace Mooctestv01.Repository
{
    public interface ISubjectRepository
    {
        Task<Subject> GetByIdAsync(int id);

        Task<List<Subject>> ListAsync();

        Task AddAsync(Subject subject);

        Task<bool> UpdateAsync(Subject subject);

        
    }
}
