﻿using AutoMapper;
using System.Linq;
using UpYourChannel.Data.Models;
using UpYourChannel.Web.ViewModels;
using UpYourChannel.Web.ViewModels.Comment;
using UpYourChannel.Web.ViewModels.Message;
using UpYourChannel.Web.ViewModels.Post;
using UpYourChannel.Web.ViewModels.Video;

namespace UpYourChannel.Web.MappingConfiguration
{
    public class UpYourChannelProfile : Profile
    {
        public UpYourChannelProfile()
        {
            CreateMap<VideoInputModel, Video>();
            CreateMap<AddVideoInputViewModel, RequestedVideo>();
            CreateMap<PostInputViewModel, Post>();
            CreateMap<Post, PostInputViewModel>();
            CreateMap<Post, PostViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<Message, MessageViewModel>();
        }
    }
}
