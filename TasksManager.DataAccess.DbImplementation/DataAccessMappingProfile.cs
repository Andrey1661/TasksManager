using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using TasksManager.DataAccess.DbImplementation.Utilities;
using TasksManager.Entities;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.DbImplementation
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<CreateProjectRequest, Project>();
            CreateMap<Project, ProjectResponse>()
                .ForMember(dst => dst.OpenTasks, opt => opt.MapFrom(src => src.Tasks == null ? 0 : src.Tasks.Count));

            CreateMap<CreateTaskRequest, Task>()
                .ForMember(dst => dst.CreateDate, opt => opt.UseValue(DateTime.Now))
                .ForMember(dst => dst.Status, opt => opt.UseValue(TaskStatus.New));
            CreateMap<Task, TaskResponse>()
                .ForMember(dst => dst.Tags, opt => opt.MapFrom(src => src.TaskTags.Select(t => t.Tag.Name)));

            CreateMap<UpdateProjectRequest, Project>();

            CreateMap<UpdateTaskRequest, Task>()
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status.Convert()));

            CreateMap<Tag, TagResponce>()
                .ForMember(dst => dst.TaskCount, opt => opt.MapFrom(src => src.TaskTags.Count))
                .ForMember(dst => dst.OpenTaskCount,
                    opt =>
                        opt.MapFrom(src => src.TaskTags.Select(t => t.Task).Count(t => t.Status != TaskStatus.Completed)));
        }
    }
}
