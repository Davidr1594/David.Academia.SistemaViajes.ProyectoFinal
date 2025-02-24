using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Dto;
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
            CreateMap<Pais, PaisDto>().ReverseMap();
            CreateMap<Transportista, TransportistaDto>().ReverseMap();
            CreateMap<Ciudad, CiudadDto>().ReverseMap();
            CreateMap<EstadoDepartamento, EstadoDepartamentoDto>().ReverseMap();
            CreateMap<MetodoPago, MetodoPagoDto>().ReverseMap();
            CreateMap<EstadoPago, EstadoPagoDto>().ReverseMap();
            CreateMap<Moneda, MonedaDto>().ReverseMap();
            CreateMap<EstadoViaje, EstadoViajeDto>().ReverseMap();
            CreateMap<Puesto, PuestoDto>().ReverseMap();
            CreateMap<Viaje, ViajeDto>().ReverseMap();







        }
    }
}
