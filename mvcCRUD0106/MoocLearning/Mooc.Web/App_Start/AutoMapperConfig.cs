using AutoMapper;
using Mooc.DataAccess.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooc.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });
        }
    }
}