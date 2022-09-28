using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace AspAPI.Mapper
{
    public static class OwnMapper
    {
        public static TOutput Map<TInput, TOutput>(TInput model)
        {
            var configuration = new MapperConfiguration(config => config.CreateMap<TInput, TOutput>());
            var mapper = new AutoMapper.Mapper(configuration);
            return mapper.Map<TOutput>(model);
        }
    }
}
