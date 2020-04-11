using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RJM.API.DAL.Repositories;
using RJM.API.Framework.Exceptions;
using RJM.API.Framework.Extensions;
using RJM.API.Models;
using RJM.API.Services.Files;

namespace RJM.API.BLL
{
	/// <summary>
	/// The business logic layer for Documents.
	/// </summary>
    public class DocumentBLL
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<DocumentBLL> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly DocumentRepository documentRepository;
        private readonly ResumeRepository resumeRepository;
        private readonly DocumentResumeRepository documentResumeRepository;
        private readonly FileService fileService;

        /// <summary>
        /// The constructor of the Document business logic layer.
        /// </summary>
        public DocumentBLL(
            IConfiguration configuration,
            ILogger<DocumentBLL> logger,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            DocumentRepository documentRepository,
            ResumeRepository resumeRepository,
			DocumentResumeRepository documentResumeRepository,
            FileService fileService
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.documentRepository = documentRepository;
            this.resumeRepository = resumeRepository;
			this.documentResumeRepository = documentResumeRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Retrieves all documents.
        /// </summary>
        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
			// Before retrieval

            return await this.documentRepository.GetWithLinkedEntitiesAsync();
        }

		/// <summary>
		/// Retrieves one document by Id.
		/// </summary>
		public async Task<Document> GetDocumentByIdAsync(Guid id)
        {
			// Before retrieval

            return await this.documentRepository.GetWithLinkedEntitiesByIdAsync(id);
        }

		/// <summary>
		/// Creates a new document record.
		/// </summary>
        public async Task<Document> CreateDocumentAsync(IFormFile file, DateTime? fileLastModified)
        {
            // Validation
            if (file == null) {
                // TODO: Check for better exception/error handling implementation?
                throw new DocumentException("File should be present");
            }

            Document document = new Document()
            {
                Id = Guid.NewGuid(),
                Name = file.FileName.ToSlug(),
                DisplayName = file.FileName,
                MimeType = file.ContentType,
                SizeInBytes = file.Length,
                FileLastModifiedOn = fileLastModified
            };

            // Before creation

            // User
            User currentUser = await this.userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
            document.UserId = currentUser.Id;
            document.User = currentUser;

            // File upload
            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                document = await this.fileService.UploadDocument(document, stream);
            }

            document = await this.documentRepository.InsertAsync(document);

			// After creation

            return document;
        }

        /// <summary>
        /// Updates an existing document record by Id.
        /// </summary>
        public async Task<Document> UpdateDocumentAsync(Document documentUpdate)
        {
            // Validation
            if (documentUpdate == null) { return null; }

            // Retrieve existing
            Document document = await this.documentRepository.GetByIdAsync(documentUpdate.Id);
            if (document == null)
            {
                return null;
            }

			// Trimming strings
            if (!string.IsNullOrEmpty(documentUpdate.Name))
                documentUpdate.Name = documentUpdate.Name.Trim();
            if (!string.IsNullOrEmpty(documentUpdate.DisplayName))
                documentUpdate.DisplayName = documentUpdate.DisplayName.Trim();
            if (!string.IsNullOrEmpty(documentUpdate.Description))
                documentUpdate.Description = documentUpdate.Description.Trim();
            if (!string.IsNullOrEmpty(documentUpdate.Path))
                documentUpdate.Path = documentUpdate.Path.Trim();
            if (!string.IsNullOrEmpty(documentUpdate.URL))
                documentUpdate.URL = documentUpdate.URL.Trim();
            if (!string.IsNullOrEmpty(documentUpdate.MimeType))
                documentUpdate.MimeType = documentUpdate.MimeType.Trim();

            // Mapping
            document.Name = documentUpdate.Name;
            document.DisplayName = documentUpdate.DisplayName;
            document.Description = documentUpdate.Description;
            document.Path = documentUpdate.Path;
            document.URL = documentUpdate.URL;
            document.SizeInBytes = documentUpdate.SizeInBytes;
            document.FileLastModifiedOn = documentUpdate.FileLastModifiedOn;
            document.MimeType = documentUpdate.MimeType;
            document.DocumentTypeId = documentUpdate.DocumentTypeId;

			// #-#-# {B5914243-E57E-41AE-A7C8-553F2F93267B}
			// Before update
			// #-#-#

			document = await this.documentRepository.UpdateAsync(document);

			// #-#-# {983B1B6C-14A7-4925-8571-D77447DF0ADA}
			// After update
			// #-#-#

            return document;
        }

        public async Task<Document> LinkResumeToDocumentAsync(DocumentResume documentResume)
        {
            // Validation
            if (documentResume == null) { return null; }

            // Check if document exists
            Document document = await this.documentRepository.GetByIdAsync(documentResume.DocumentId);
            if (document == null)
            {
                return null;
            }

            // Check if resume exists
            Resume resume = await this.resumeRepository.GetByIdAsync(documentResume.ResumeId);
            if (resume == null)
            {
                return null;
            }

            // Retrieve existing link
            DocumentResume documentResumeLink = this.documentResumeRepository.GetByDocumentAndResumeId(documentResume.DocumentId, documentResume.ResumeId);

            if (documentResumeLink == null)
            {
                await this.documentResumeRepository.InsertAsync(documentResume);
            }
            else
            {
                // Mapping of fields on many-to-many

                await this.documentResumeRepository.UpdateAsync(documentResumeLink);
            }

            return await this.GetDocumentByIdAsync(documentResume.DocumentId);
        }

        public async Task<Document> UnlinkResumeFromDocumentAsync(DocumentResume documentResume)
        {
            // Validation
            if (documentResume == null) { return null; }

            // Retrieve existing link
            DocumentResume documentResumeLink = this.documentResumeRepository.GetByDocumentAndResumeId(documentResume.DocumentId, documentResume.ResumeId);
		
            if (documentResumeLink != null)
            {
                await this.documentResumeRepository.DeleteAsync(documentResumeLink);
            }

            return await this.GetDocumentByIdAsync(documentResume.DocumentId);
        }

		/// <summary>
		/// Deletes an existing document record by Id.
		/// </summary>
        public async Task<Document> DeleteDocumentByIdAsync(Guid documentId)
        {
            Document document = await this.documentRepository.GetByIdAsync(documentId);

            return await this.DeleteDocumentAsync(document);
        }

		/// <summary>
		/// Deletes an existing document record.
		/// </summary>
        public async Task<Document> DeleteDocumentAsync(Document document)
        {
            // Validation
            if (document == null) { return null; }

			// #-#-# {FE1A99E0-482D-455B-A8C1-3C2C11FACA58}
			// Before deletion
			// #-#-#

            await this.documentRepository.DeleteAsync(document);

			// #-#-# {F09857C0-44E7-4E6C-B3E6-883C0D28E1A6}
			// After deletion
			// #-#-#

            return document;
        }
    }
}
