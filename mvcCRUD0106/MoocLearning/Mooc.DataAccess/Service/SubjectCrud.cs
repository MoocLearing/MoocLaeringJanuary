using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Mooc.DataAccess.Entities;
using Mooc.DataAccess.Context;

namespace Mooc.DataAccess.Service
{
    public class SubjectCrud : ISubjectCrud
    {
        //想直接注入DbContext
        DataContext _dataContext = new DataContext();


        public async Task<bool> DeleteAsync(int id)
        {

                Task<Subject> subject = _dataContext.Subjects.FirstAsync(r => r.SubId == id);
                _dataContext.Subjects.Remove(await subject);
                return await _dataContext.SaveChangesAsync()>0;

            
        }

        public Task<Subject> GetByIdAsync(int id)
        {

                return _dataContext.Subjects.FirstOrDefaultAsync(r =>r.SubId==id);
            
        }

        //List
        public Task<List<Subject>> ListAsync()
        {


                return _dataContext.Subjects.ToListAsync();
            
        }

        //public List<Subject> GetList()
        //{
        //    using (DataContext db = new DataContext())
        //    {
        //        return db.Subjects.ToList();
        //    }
        //}



        public async Task<bool> UpdateAsync(Subject subject)
        {

            _dataContext.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                return await _dataContext.SaveChangesAsync() > 0;
            
        }

        //Add
        public Task AddAsync(Subject subject)
        {
            _dataContext.Subjects.Add(subject);
            return _dataContext.SaveChangesAsync();
        }
    }
}