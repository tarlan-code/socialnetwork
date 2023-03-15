using AutoMapper;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, AppUser>();
            CreateMap<AppUser, SearchedUserVM>()
                .ForMember(x => x.Fullname, m => m.MapFrom(u => (u.Name) + "  " + (u.Surname == null ? "" : u.Surname)))
                .ForMember(x => x.ProfilImage, m => m.MapFrom(u => (u.ProfilImage == null ? "default.png" : u.ProfilImage)))
                .ForMember(x => x.Username, m => m.MapFrom(u => u.UserName));
            CreateMap<ProfileInformationVM, AppUser>().ReverseMap()
                .ForMember(x => x.City, m => m.MapFrom(u => u.City.Name))
                .ForMember(x => x.Country, m => m.MapFrom(u => u.Country.Name))
                .ForMember(x => x.Gender, m => m.MapFrom(u => u.Gender.Name))
                .ForMember(x => x.Relation, m => m.MapFrom(u => u.Relation.Name))
                .ForMember(x => x.PhoneNo, m => m.MapFrom(u => u.PhoneNumber));
            CreateMap<SettingInfoVM, AppUser>().ForMember(x => x.Email, opt => opt.Ignore());
            CreateMap<AppUser, SettingInfoVM>();
            CreateMap<PrivacySetting, PrivacyVM>().ReverseMap();
            CreateMap<AppUser, RequestVM>()
                .ForMember(u => u.Fullname, m => m.MapFrom(u => (u.Name) + "  " + (u.Surname == null ? "" : u.Surname)))
                .ForMember(u => u.Username, m => m.MapFrom(u => u.UserName));

            CreateMap<Post, ShowedPostVM>()
                .ForMember(p => p.LikeCount, m => m.MapFrom(p => p.PostReactions.Count()))
                .ForMember(p => p.PostTagsUsernames, m => m.MapFrom(p => p.PostTags.Select(p=>p.AppUser)))
                .ForMember(p => p.PostMediasName, m => m.MapFrom(p => p.PostMedias.Select(p => p.Media)))
                .ForMember(p => p.Text, m => m.MapFrom(p => p.Content))
                .ForMember(p => p.Date, m => m.MapFrom(p => p.Date.ToShortDateString()))
                .ForMember(p => p.PostId, m => m.MapFrom(p => p.Id))
                .ForMember(p => p.LikeCount, m => m.MapFrom(p => p.PostReactions.Count()))
                .ForMember(p => p.Comments, m => m.Ignore())
                .ForMember(p => p.CommentsCount, m => m.MapFrom(p => p.Comments.Count()))
                .ForMember(p=>p.Fullname,m=>m.MapFrom(u => (u.AppUser.Name) + "  " + (u.AppUser.Surname == null ? "" : u.AppUser.Surname)))
                .ForMember(p=>p.Username,m=>m.MapFrom(p=>p.AppUser.UserName))
                .ForMember(p=>p.ProfilImage,m=>m.MapFrom(p=>p.AppUser.ProfilImage)).ReverseMap();
            CreateMap<Comment, AddedCommentVM>()
                .ForMember(c => c.Fullname, m => m.MapFrom(u => (u.AppUser.Name) + "  " + (u.AppUser.Surname == null ? "" : u.AppUser.Surname)))
                .ForMember(c => c.Profilimage, m => m.MapFrom(u => u.AppUser.ProfilImage))
                .ForMember(c => c.Username, m => m.MapFrom(u => u.AppUser.UserName));
            CreateMap<AppUser, ContactVM>();
            CreateMap<AppUser, ChatVM>()
                .ForMember(p => p.Fullname, m => m.MapFrom(u => (u.Name) + "  " + (u.Surname == null ? "" : u.Surname)))
                .ForMember(c=>c.Messages,u=>u.Ignore());
            CreateMap<Message, MessageVM>()
                .ForMember(m => m.ProfilImage, m => m.MapFrom(p => p.Sender.ProfilImage))
                .ForMember(m => m.Username, m => m.MapFrom(p => p.Sender.UserName))
                .ForMember(m=>m.Date,m=>m.MapFrom(p=>p.Date));
            CreateMap<Notification, NotificationVM>()
                .ForMember(n => n.Fullname, m => m.MapFrom(u => (u.Sender.Name) + "  " + (u.Sender.Surname == null ? "" : u.Sender.Surname)))
                .ForMember(n => n.ProfilImage, m => m.MapFrom(u => u.Sender.ProfilImage))
                .ForMember(n=>n.Date,m=>m.Ignore())
                .ForMember(n=>n.Username,m=>m.MapFrom(u=>u.Sender.UserName));
            CreateMap<AppUser, BirthdayVM>()
                .ForMember(u => u.Fullname, m => m.MapFrom(u => (u.Name) + "  " + (u.Surname == null ? "" : u.Surname)));
            CreateMap<PostMedia, VideoVM>()
                .ForMember(p => p.CommentCount, m => m.MapFrom(u => u.Post.Comments.Count()))
                .ForMember(p => p.LikeCount, m => m.MapFrom(u => u.Post.PostReactions.Count()))
                .ForMember(p => p.Fullname, m => m.MapFrom(u => (u.Post.AppUser.Name) + "  " + (u.Post.AppUser.Surname == null ? "" : u.Post.AppUser.Surname)))
                .ForMember(p => p.Username, m => m.MapFrom(u => u.Post.AppUser.UserName))
                .ForMember(p=>p.ProfilImage,m=>m.MapFrom(u=>u.Post.AppUser.ProfilImage));

            CreateMap<AppUser, HeaderVM>()
                .ForMember(u => u.Notifications, m => m.Ignore())
                .ForMember(u => u.Requests, m => m.Ignore())
                .ForMember(u => u.ProfilImage, m => m.MapFrom(u => u.ProfilImage))
                .ForMember(u => u.Fullname, m => m.MapFrom(u => (u.Name) + "  " + (u.Surname == null ? "" : u.Surname)));
            CreateMap<EventVM, Event>();
            CreateMap<Event, EventCardVM>()
                .ForMember(e=>e.Username,e=>e.MapFrom(e=>e.AppUser.UserName));
            CreateMap<AppUser, PanelUserListVM>()
                  .ForMember(p => p.Fullname, m => m.MapFrom(u => (u.Name) + "  " + (u.Surname == null ? "" : u.Surname)));
            CreateMap<SupportContactVM, Contact>();
            CreateMap<Contact, PanelContactVM>()
                .ForMember(c => c.Username, m => m.MapFrom(c => c.AppUser.UserName))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.AppUser.Email))
                .ForMember(c => c.Fullname, m => m.MapFrom(u => (u.AppUser.Name) + "  " + (u.AppUser.Surname == null ? "" : u.AppUser.Surname)));



        }
    }
}