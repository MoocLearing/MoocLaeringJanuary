using AutoMapper;
using Mooc.Data.AutoMapper;

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