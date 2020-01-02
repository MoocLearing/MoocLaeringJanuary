using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Mooctestv01.Models;
using Mooctestv01.Repository;

namespace Mooctestv01.Models
{
    public class SubjectRepository : ISubjectRepository
    {
        SubjectContext _subjectcontext = new SubjectContext();

        //Add func
        public Task AddAsync(Subject subject)
        {
            _subjectcontext.Subjects.Add(subject);
            return _subjectcontext.SaveChangesAsync();
            
        }
        //SearchById
        public Task<Subject> GetByIdAsync(int id)
        {
            return _subjectcontext.Subjects.FirstOrDefaultAsync(r => r.Id == id);
        }

        //List ALL
        public Task<List<Subject>> ListAsync()
        {
            return _subjectcontext.Subjects.ToListAsync();
        }

        //Updata Subject
        public async Task<bool> UpdateAsync(Subject subject)
        {
            _subjectcontext.Subjects.AddOrUpdate(subject);
            return await _subjectcontext.SaveChangesAsync() > 0;
        }
    }
}