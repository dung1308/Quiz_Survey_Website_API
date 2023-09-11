using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AutoMapper;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.QuestionMapperProfile;

public class AutoMapperProfile : Profile
{
    protected AutoMapperProfile()
    {
        // CreateMap<User, UserDTO>()
        // // .ForMember(dest => dest.Id,
        // // opt => opt.MapFrom(src => Guid.NewGuid()))
        // .ForMember(dest => dest.UserName,
        // opt => opt.MapFrom(src => src.UserName))
        // .ForMember(dest => dest.Password,
        // opt => opt.MapFrom(src => src.Password))
        // .ForMember(dest => dest.Email,
        // opt => opt.MapFrom(src => src.Email));

        CreateMap<UserDTO, User>()
        .ForMember(dest => dest.UserName,
        opt => opt.MapFrom(src => src.UserName))
        .ForMember(dest => dest.Password,
        opt => opt.MapFrom(src => src.Password))
        .ForMember(dest => dest.Email,
        opt => opt.MapFrom(src => src.Email));

        // CreateMap<CategoryList, CategoryListDTO>()
        // .ForMember(dest => dest.CategoryName,
        // opt => opt.MapFrom(src => src.CategoryName));

        CreateMap<CategoryListDTO, CategoryList>()
        .ForMember(dest => dest.CategoryName,
        opt => opt.MapFrom(src => src.CategoryName));

        // CreateMap<Question, QuestionDTO>()
        // .ForMember(dest => dest.QuestionName,
        // opt => opt.MapFrom(src => src.QuestionName))
        // .ForMember(dest => dest.ChoicesString,
        // opt => opt.MapFrom(src => src.ChoicesString))
        // .ForMember(dest => dest.Type,
        // opt => opt.MapFrom(src => src.Type))
        // .ForMember(dest => dest.AnswersString,
        // opt => opt.MapFrom(src => src.AnswersString))
        // .ForMember(dest => dest.OnAnswersString,
        // opt => opt.MapFrom(src => src.OnAnswersString))
        // .ForMember(dest => dest.Score,
        // opt => opt.MapFrom(src => src.Score));

        CreateMap<QuestionDTO, Question>()
        .ForMember(dest => dest.QuestionName,
        opt => opt.MapFrom(src => src.QuestionName))
        .ForMember(dest => dest.ChoicesString,
        opt => opt.MapFrom(src => src.ChoicesString))
        .ForMember(dest => dest.Type,
        opt => opt.MapFrom(src => src.Type))
        .ForMember(dest => dest.AnswersString,
        opt => opt.MapFrom(src => src.AnswersString))
        .ForMember(dest => dest.OnAnswersString,
        opt => opt.MapFrom(src => src.OnAnswersString))
        .ForMember(dest => dest.Score,
        opt => opt.MapFrom(src => src.Score));

        // CreateMap<QuestionBank, QuestionBankDTO>()
        // .ForMember(dest => dest.SurveyCode,
        // opt => opt.MapFrom(src => src.SurveyCode))
        // .ForMember(dest => dest.SurveyName,
        // opt => opt.MapFrom(src => src.SurveyName))
        // .ForMember(dest => dest.Owner,
        // opt => opt.MapFrom(src => src.Owner))
        // .ForMember(dest => dest.Category,
        // opt => opt.MapFrom(src => src.Category))
        // .ForMember(dest => dest.Timer,
        // opt => opt.MapFrom(src => src.Timer))
        // .ForMember(dest => dest.StartDate,
        // opt => opt.MapFrom(src => src.StartDate))
        // .ForMember(dest => dest.EndDate,
        // opt => opt.MapFrom(src => src.EndDate))
        // .ForMember(dest => dest.Status,
        // opt => opt.MapFrom(src => src.Status))
        // .ForMember(dest => dest.EnableStatus,
        // opt => opt.MapFrom(src => src.EnableStatus));

        CreateMap<QuestionBankDTO, QuestionBank>()
        .ForMember(dest => dest.SurveyCode,
        opt => opt.MapFrom(src => src.SurveyCode))
        .ForMember(dest => dest.SurveyName,
        opt => opt.MapFrom(src => src.SurveyName))
        .ForMember(dest => dest.Owner,
        opt => opt.MapFrom(src => src.Owner))
        .ForMember(dest => dest.Category,
        opt => opt.MapFrom(src => src.Category))
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

        // CreateMap<ResultShow, ResultShowDTO>()
        // .ForMember(dest => dest.OnAnswer,
        // opt => opt.MapFrom(src => src.OnAnswer));

        CreateMap<ResultShowDTO, ResultShow>()
        .ForMember(dest => dest.OnAnswer,
        opt => opt.MapFrom(src => src.OnAnswer));

        // CreateMap<Role, RoleDTO>()
        // .ForMember(dest => dest.RoleName,
        // opt => opt.MapFrom(src => src.RoleName))
        // .ForMember(dest => dest.Permission,
        // opt => opt.MapFrom(src => src.Permission));

        CreateMap<RoleDTO, Role>()
        .ForMember(dest => dest.RoleName,
        opt => opt.MapFrom(src => src.RoleName))
        .ForMember(dest => dest.Permission,
        opt => opt.MapFrom(src => src.Permission));

        // CreateMap<QuestionBankInteract, QuestionBankInteractDTO>()
        // .ForMember(dest => dest.ResultScores,
        // opt => opt.MapFrom(src => src.ResultScores));

        CreateMap<QuestionBankInteractDTO, QuestionBankInteract>()
        .ForMember(dest => dest.ResultScores,
        opt => opt.MapFrom(src => src.ResultScores));
    }
}