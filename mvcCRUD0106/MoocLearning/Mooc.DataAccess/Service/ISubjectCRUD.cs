using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Threading.Tasks;
using Mooc.DataAccess.Entities;

namespace Mooc.DataAccess.Service
{
    public interface ISubjectCrud:IDependency
    {
        Task<Subject> GetByIdAsync(int id);

        Task<List<Subject>> ListAsync();
        //List<Subject> GetList();

        Task AddAsync(Subject subject);
        

        Task<bool> UpdateAsync(Subject subject);

        Task<bool> DeleteAsync(int id);
    }
}