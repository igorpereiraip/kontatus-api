﻿using AutoMapper;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Domain.ViewModels;
using ConsigIntegra.Helper.Utilitarios;

namespace ConsigIntegra.API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Base, BaseViewModel>()
                .ForMember(b => b.DataCadastro, mapper => mapper.MapFrom(b => TimezoneConfig.ConvertDateTime(b.DataCadastro)))
                .ForMember(b => b.DataAlteracao, mapper => mapper.MapFrom(b => TimezoneConfig.ConvertDateTime(b.DataAlteracao)))
                .IncludeAllDerived()
                .ReverseMap()
                .ForMember(b => b.DataCadastro, mapper => mapper.MapFrom(b => b.DataCadastro.ToUniversalTime()))
                .ForMember(b => b.DataAlteracao, mapper => mapper.MapFrom(b => TimezoneConfig.ConvertDateTimeToUTC(b.DataAlteracao)))
                .IncludeAllDerived();

            CreateMap<Login, LoginViewModel>().ReverseMap();

            CreateMap<LogUsuario, LogUsuarioViewModel>().ForMember(b => b.RegistroAfetadoNome, mapper => mapper.MapFrom(b => b.RegistroAfetado.Nome))
                .IncludeAllDerived();
            CreateMap<LogUsuarioDTO, LogUsuario>().ReverseMap();
            CreateMap<TokenConfig, TokenConfigViewModel>().ReverseMap();

            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();

            //CreateMap<Perfil, PerfilViewModel>().ReverseMap();
            //CreateMap<PerfilDTO, Perfil>().ReverseMap();
        }
    }
}
