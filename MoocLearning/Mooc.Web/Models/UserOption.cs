using Mooc.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Models
{
    public class UserOption
    {
        public IEnumerable<SelectListItem> GetRoleTypes()
        {
            IEnumerable<SelectListItem> enumsList = ((RoleTypeEnum[])Enum.GetValues(typeof(RoleTypeEnum))).Select(c => new SelectListItem() { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();
            return enumsList;
        }
    }
}