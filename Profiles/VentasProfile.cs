using AutoMapper;
using Ventas.DTOs;
using Ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ventas.Profiles
{
    public class VentasProfile: Profile
    {
      
        //En esta clase declaro el mapeo de un modelo tarea a un DTO Tarea o viceversa
        public VentasProfile( IMapper mapper)
        {
            CreateMap<Articulo,VentasReadDTO>().ReverseMap();
        }

        public VentasProfile()
        {

        }
    }
}
