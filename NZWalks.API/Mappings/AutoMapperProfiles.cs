using AutoMapper;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

            CreateMap<ImageUploadRequestDto, Image>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FileDescription))
                //.ForMember(dest => dest.Extension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
                .ReverseMap();
        }
    }
}
