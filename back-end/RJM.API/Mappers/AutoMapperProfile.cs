using AutoMapper;
using RJM.API.Models;
using RJM.API.ViewModels;
using RJM.API.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RJM.API.Mappers
{
    /// <summary>
    /// Profile for mapping models to/from view models with AutoMapper.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// The constructor of AutoMapperProfile.
        /// </summary>
        public AutoMapperProfile()
        {
            // Documents
            CreateMap<Document, DocumentVM>()
                .ForMember(
                    x => x.Resumes,
                    x => x.MapFrom(
                        y => y.DocumentResume.Select(z => z.Resume)
                    )
                )
                .ForMember(
                    x => x.Contents,
                    x => x.MapFrom(y => y.DocumentContents)
                );
            CreateMap<DocumentVM, Document>()
                .ForMember(
                    x => x.DocumentResume,
                    x =>
                    {
                        x.PreCondition(z => z.ResumeId != null);
                        x.MapFrom(
                            y => new List<DocumentResume>() {
                                new DocumentResume()
                                {
                                    ResumeId = (Guid)y.ResumeId
                                }
                            }
                        );
                    }
                );

            // DocumentTypes
            CreateMap<DocumentType, DocumentTypeVM>();
            CreateMap<DocumentTypeVM, DocumentType>();

            // DocumentContent
            CreateMap<DocumentContent, DocumentContentVM>();
            CreateMap<DocumentContentVM, DocumentContent>();

            // Resumes
            CreateMap<Resume, ResumeVM>()
                .ForMember(
                    x => x.Documents,
                    x => x.MapFrom(
                        y => y.DocumentResume.Select(z => z.Document)
                    )
                )
                .ForMember(
                    x => x.Skills,
                    x => x.MapFrom(
                        y => y.ResumeSkill.Select(z => z.Skill)
                    )
                );
            CreateMap<ResumeVM, Resume>()
                .ForMember(
                    x => x.DocumentResume,
                    x =>
                    {
                        x.PreCondition(z => z.DocumentId != null);
                        x.MapFrom(
                            y => new List<DocumentResume>() {
                                new DocumentResume()
                                {
                                    DocumentId = (Guid)y.DocumentId
                                }
                            }
                        );
                    }
                )
                .ForMember(
                    x => x.ResumeSkill,
                    x =>
                    {
                        x.PreCondition(z => z.SkillId != null);
                        x.MapFrom(
                            y => new List<ResumeSkill>() {
                                new ResumeSkill()
                                {
                                    SkillId = (Guid)y.SkillId,
                                    Level = y.SkillLevel,
                                    Description = y.SkillDescription
                                }
                            }
                        );
                    }
                );

            // ResumeStates
            CreateMap<ResumeState, ResumeStateVM>();
            CreateMap<ResumeStateVM, ResumeState>();

            // Skills
            CreateMap<Skill, SkillVM>()
                .ForMember(
                    x => x.Resumes,
                    x => x.MapFrom(
                        y => y.ResumeSkill.Select(z => z.Resume)
                    )
                )
                .ForMember(
                    x => x.Jobs,
                    x => x.MapFrom(
                        y => y.JobSkill.Select(z => z.Job)
                    )
                );
            CreateMap<SkillVM, Skill>()
                .ForMember(
                    x => x.ResumeSkill,
                    x =>
                    {
                        x.PreCondition(z => z.ResumeId != null);
                        x.MapFrom(
                            y => new List<ResumeSkill>() {
                                new ResumeSkill()
                                {
                                    ResumeId = (Guid)y.ResumeId,
                                    Level = y.ResumeLevel,
                                    Description = y.ResumeDescription
                                }
                            }
                        );
                    }
                )
                .ForMember(
                    x => x.JobSkill,
                    x =>
                    {
                        x.PreCondition(z => z.JobId != null);
                        x.MapFrom(
                            y => new List<JobSkill>() {
                                new JobSkill()
                                {
                                    JobId = (Guid)y.JobId,
                                    Level = y.JobLevel,
                                    Description = y.JobDescription
                                }
                            }
                        );
                    }
                );

            // SkillAliases
            CreateMap<SkillAlias, SkillAliasVM>();
            CreateMap<SkillAliasVM, SkillAlias>();

            // Jobs
            CreateMap<Job, JobVM>()
                .ForMember(
                    x => x.Skills,
                    x => x.MapFrom(
                        y => y.JobSkill.Select(z => z.Skill)
                    )
                );
            CreateMap<JobVM, Job>()
                .ForMember(
                    x => x.JobSkill,
                    x =>
                    {
                        x.PreCondition(z => z.SkillId != null);
                        x.MapFrom(
                            y => new List<JobSkill>() {
                                new JobSkill()
                                {
                                    SkillId = (Guid)y.SkillId,
                                    Level = y.SkillLevel,
                                    Description = y.SkillDescription
                                }
                            }
                        );
                    }
                );

            // JobStates
            CreateMap<JobState, JobStateVM>();
            CreateMap<JobStateVM, JobState>();

            // Users
            CreateMap<User, UserVM>();
            CreateMap<UserVM, User>();
        }
    }
}
