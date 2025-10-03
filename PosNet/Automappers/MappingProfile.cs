

using AutoMapper;
using PosNet.DTOs;
using PosNet.Models;

namespace PosNet.Automappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // User's mappers
            CreateMap<AuthDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}
