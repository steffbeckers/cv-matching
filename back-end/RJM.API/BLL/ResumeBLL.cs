using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RJM.API.DAL.Repositories;
using RJM.API.Framework.Extensions;
using RJM.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RJM.API.BLL
{
    /// <summary>
    /// The business logic layer for Resumes.
    /// </summary>
    public class ResumeBLL
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<DocumentBLL> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        private readonly ResumeRepository resumeRepository;
        private readonly ResumeStateRepository resumeStateRepository;
        private readonly ResumeSkillRepository resumeSkillRepository;

        private readonly DocumentResumeRepository documentResumeRepository;
        private readonly DocumentRepository documentRepository;

        private readonly SkillRepository skillRepository;

        /// <summary>
        /// The constructor of the Resume business logic layer.
        /// </summary>
        public ResumeBLL(
            IConfiguration configuration,
            ILogger<DocumentBLL> logger,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            ResumeRepository resumeRepository,
            ResumeStateRepository resumeStateRepository,
            ResumeSkillRepository resumeSkillRepository,
            DocumentResumeRepository documentResumeRepository,
            DocumentRepository documentRepository,
            SkillRepository skillRepository
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;

            this.resumeRepository = resumeRepository;
            this.resumeStateRepository = resumeStateRepository;
            this.resumeSkillRepository = resumeSkillRepository;

            this.documentResumeRepository = documentResumeRepository;
            this.documentRepository = documentRepository;

            this.skillRepository = skillRepository;
        }

        /// <summary>
        /// Retrieves all resumes.
        /// </summary>
        public async Task<IEnumerable<Resume>> GetAllResumesAsync()
        {
            return await this.resumeRepository.GetWithLinkedEntitiesAsync();
        }

        /// <summary>
        /// Retrieves one resume by Id.
        /// </summary>
        public async Task<Resume> GetResumeByIdAsync(Guid id)
        {
            return await this.resumeRepository.GetWithLinkedEntitiesByIdAsync(id);
        }

        /// <summary>
        /// Creates a new resume record.
        /// </summary>
        public async Task<Resume> CreateResumeAsync(Resume resume)
        {
            // Validation
            if (resume == null) { return null; }

            // Before creation

            // Trimming strings
            if (!string.IsNullOrEmpty(resume.DisplayName))
                resume.DisplayName = resume.DisplayName.Trim();
            if (!string.IsNullOrEmpty(resume.JobTitle))
                resume.JobTitle = resume.JobTitle.Trim();
            if (!string.IsNullOrEmpty(resume.Description))
                resume.Description = resume.Description.Trim();

            // Name field
            resume.Name = resume.DisplayName.ToSlug();

            // Default resume state
            if (resume.ResumeStateId == Guid.Empty)
            {
                ResumeState resumeStateActive = await this.resumeStateRepository.GetByNameAsync("active");
                resume.ResumeStateId = resumeStateActive.Id;
                resume.ResumeState = resumeStateActive;
            }

            // User
            if (resume.UserId == Guid.Empty)
            {
                User currentUser = await this.userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
                resume.UserId = currentUser.Id;
                resume.User = currentUser;
            }

            resume = await this.resumeRepository.InsertAsync(resume);

            // After creation

            return resume;
        }

        /// <summary>
        /// Updates an existing resume record by Id.
        /// </summary>
        public async Task<Resume> UpdateResumeAsync(Resume resumeUpdate)
        {
            // Validation
            if (resumeUpdate == null) { return null; }

            // Retrieve existing
            Resume resume = await this.resumeRepository.GetByIdAsync(resumeUpdate.Id);
            if (resume == null)
            {
                return null;
            }

            // Trimming strings
            if (!string.IsNullOrEmpty(resumeUpdate.JobTitle))
                resumeUpdate.JobTitle = resumeUpdate.JobTitle.Trim();
            if (!string.IsNullOrEmpty(resumeUpdate.Description))
                resumeUpdate.Description = resumeUpdate.Description.Trim();

            // Mapping
            resume.JobTitle = resumeUpdate.JobTitle;
            resume.Description = resumeUpdate.Description;
            if (resumeUpdate.ResumeStateId != Guid.Empty)
                resume.ResumeStateId = resumeUpdate.ResumeStateId;

            resume = await this.resumeRepository.UpdateAsync(resume);

            return resume;
        }

        public async Task<Resume> LinkDocumentToResumeAsync(DocumentResume documentResume)
        {
            // Validation
            if (documentResume == null) { return null; }

            // Check if resume exists
            Resume resume = await this.resumeRepository.GetByIdAsync(documentResume.ResumeId);
            if (resume == null)
            {
                return null;
            }

            // Check if document exists
            Document document = await this.documentRepository.GetByIdAsync(documentResume.DocumentId);
            if (document == null)
            {
                return null;
            }

            // Retrieve existing link
            DocumentResume documentResumeLink = this.documentResumeRepository.GetByResumeAndDocumentId(documentResume.ResumeId, documentResume.DocumentId);

            if (documentResumeLink == null)
            {
                await this.documentResumeRepository.InsertAsync(documentResume);
            }
            else
            {
                // Mapping of fields on many-to-many

                await this.documentResumeRepository.UpdateAsync(documentResumeLink);
            }

            return await this.GetResumeByIdAsync(documentResume.ResumeId);
        }

        public async Task<Resume> LinkSkillToResumeAsync(ResumeSkill resumeSkill)
        {
            // Validation
            if (resumeSkill == null) { return null; }

            // Check if resume exists
            Resume resume = await this.resumeRepository.GetByIdAsync(resumeSkill.ResumeId);
            if (resume == null)
            {
                return null;
            }

            // Check if skill exists
            Skill skill = await this.skillRepository.GetByIdAsync(resumeSkill.SkillId);
            if (skill == null)
            {
                return null;
            }

            // Retrieve existing link
            ResumeSkill resumeSkillLink = this.resumeSkillRepository.GetByResumeAndSkillId(resumeSkill.ResumeId, resumeSkill.SkillId);

            if (resumeSkillLink == null)
            {
                await this.resumeSkillRepository.InsertAsync(resumeSkill);
            }
            else
            {
                // Mapping of fields on many-to-many
                resumeSkillLink.Level = resumeSkill.Level;
                resumeSkillLink.Description = resumeSkill.Description;

                await this.resumeSkillRepository.UpdateAsync(resumeSkillLink);
            }

            return await this.GetResumeByIdAsync(resumeSkill.ResumeId);
        }

        public async Task<Resume> UnlinkDocumentFromResumeAsync(DocumentResume documentResume)
        {
            // Validation
            if (documentResume == null) { return null; }

            // Retrieve existing link
            DocumentResume documentResumeLink = this.documentResumeRepository.GetByResumeAndDocumentId(documentResume.ResumeId, documentResume.DocumentId);

            if (documentResumeLink != null)
            {
                await this.documentResumeRepository.DeleteAsync(documentResumeLink);
            }

            return await this.GetResumeByIdAsync(documentResume.ResumeId);
        }

        public async Task<Resume> UnlinkSkillFromResumeAsync(ResumeSkill resumeSkill)
        {
            // Validation
            if (resumeSkill == null) { return null; }

            // Retrieve existing link
            ResumeSkill resumeSkillLink = this.resumeSkillRepository.GetByResumeAndSkillId(resumeSkill.ResumeId, resumeSkill.SkillId);

            if (resumeSkillLink != null)
            {
                await this.resumeSkillRepository.DeleteAsync(resumeSkillLink);
            }

            return await this.GetResumeByIdAsync(resumeSkill.ResumeId);
        }

        /// <summary>
        /// Deletes an existing resume record by Id.
        /// </summary>
        public async Task<Resume> DeleteResumeByIdAsync(Guid resumeId)
        {
            Resume resume = await this.resumeRepository.GetByIdAsync(resumeId);

            return await this.DeleteResumeAsync(resume);
        }

        /// <summary>
        /// Deletes an existing resume record.
        /// </summary>
        public async Task<Resume> DeleteResumeAsync(Resume resume)
        {
            // Validation
            if (resume == null) { return null; }

            // #-#-# {FE1A99E0-482D-455B-A8C1-3C2C11FACA58}
            // Before deletion
            // #-#-#

            await this.resumeRepository.DeleteAsync(resume);

            // #-#-# {F09857C0-44E7-4E6C-B3E6-883C0D28E1A6}
            // After deletion
            // #-#-#

            return resume;
        }
    }
}
