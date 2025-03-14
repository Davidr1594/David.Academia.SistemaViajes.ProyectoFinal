using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.SolicitudDeViaje.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using System.Text;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Usuario, UsuarioDto>().ForMember(x => x.UsuarioId, o => o.MapFrom(x => x.UsuarioId)).ReverseMap();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Colaborador, ColaboradorDto>().ReverseMap();
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Sucursal, SucursalDto>().ReverseMap();
            CreateMap<Sucursal, SucursalActualizaDto>().ReverseMap();
            CreateMap<Transportista, TransportistaDto>().ReverseMap();
            CreateMap<Viaje, ViajeDto>().ReverseMap();
            CreateMap<ParametroSistema, ParametroSistemaDto>().ReverseMap();
            CreateMap<SolicitudViaje, SolicitudViajeDto>().ReverseMap();








        }
    }
}
