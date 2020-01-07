using Mooc.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooc.Web.Models
{
    public class TestModel
    {
        private readonly DataContext _dataContext;
        public TestModel(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public void GetList()
        {
            using (DataContext db = new DataContext())
            {

            }
        }

    }
}