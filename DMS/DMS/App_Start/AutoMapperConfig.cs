using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<Job, JobModels>();
            AutoMapper.Mapper.CreateMap<JobModels, Job>();

            AutoMapper.Mapper.CreateMap<History, HistoryModels>();
            AutoMapper.Mapper.CreateMap<HistoryModels, History>();

            AutoMapper.Mapper.CreateMap<Note, NoteModels>();
            AutoMapper.Mapper.CreateMap<NoteModels, Note>();

            AutoMapper.Mapper.CreateMap<Attachment, AttachmentModels>();
            AutoMapper.Mapper.CreateMap<AttachmentModels, Attachment>();
        }
    }
}