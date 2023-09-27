using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AutoMapper;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.DTO.Outgoing;
using survey_quiz_app.Models;

namespace survey_quiz_app.QuestionMapperProfile;

public class DomainToResponse : Profile
{
    public DomainToResponse()
    {
        CreateMap<User, UserDTOResponse>()
        // .ForMember(dest => dest.Id,
        // opt => opt.MapFrom(src => Guid.NewGuid()))
        .ForMember(dest => dest.UserName,
        opt => opt.MapFrom(src => src.UserName))
        .ForMember(dest => dest.Password,
        opt => opt.MapFrom(src => src.Password))
        .ForMember(dest => dest.Email,
        opt => opt.MapFrom(src => src.Email));

        CreateMap<CategoryList, CategoryListDTOResponse>()
        .ForMember(dest => dest.CategoryName,
        opt => opt.MapFrom(src => src.CategoryName));

        // CreateMap<List<QuestionBank>, List<QuestionBankDTOResponse>>()
        // .ForMember(dest => dest,
        // opt => opt.MapFrom(src => src));

        CreateMap<QuestionBank, QuestionBankDTOResponse>()
        .ForMember(dest => dest.SurveyCode,
        opt => opt.MapFrom(src => src.SurveyCode))
        .ForMember(dest => dest.SurveyName,
        opt => opt.MapFrom(src => src.SurveyName))
        .ForMember(dest => dest.Owner,
        opt => opt.MapFrom(src => src.Owner))
        .ForMember(dest => dest.Timer,
        opt => opt.MapFrom(src => src.Timer))
        .ForMember(dest => dest.StartDate,
        opt => opt.MapFrom(src => src.StartDate))
        .ForMember(dest => dest.EndDate,
        opt => opt.MapFrom(src => src.EndDate))
        .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status))
        .ForMember(dest => dest.EnableStatus,
        opt => opt.MapFrom(src => src.EnableStatus));

        CreateMap<ResultShow, ResultShowDTOResponse>()
        .ForMember(dest => dest.ResultScore,
        opt => opt.MapFrom(src => src.ResultScore));

        CreateMap<Role, RoleDTOResponse>()
        .ForMember(dest => dest.RoleName,
        opt => opt.MapFrom(src => src.RoleName))
        .ForMember(dest => dest.Permission,
        opt => opt.MapFrom(src => src.Permission));

        CreateMap<QuestionBankInteract, QuestionBankInteractDTOResponse>();
    }
}