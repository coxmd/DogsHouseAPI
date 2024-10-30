using AutoMapper;
using DogsHouse.Core.DTOs;
using DogsHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from Dog to DogDto
            CreateMap<Dog, DogDto>();

            // Map from CreateDogDto to Dog
            CreateMap<DogDto, Dog>();
        }
    }
}
