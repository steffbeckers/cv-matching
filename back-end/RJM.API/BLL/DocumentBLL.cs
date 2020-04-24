using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RJM.API.DAL.Repositories;
using RJM.API.Framework.Exceptions;
using RJM.API.Framework.Extensions;
using RJM.API.Models;
using RJM.API.Services.Files;
using RJM.API.Services.RabbitMQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        private readonly FileService fileService;
        private readonly RabbitMQService rabbitMQService;

        private readonly DocumentRepository documentRepository;
        private readonly DocumentTypeRepository documentTypeRepository;

        private readonly ResumeBLL resumeBLL;
        private readonly ResumeRepository resumeRepository;
        private readonly DocumentResumeRepository documentResumeRepository;

        /// <summary>
        /// The constructor of the Document business logic layer.
        /// </summary>
        public DocumentBLL(
            IConfiguration configuration,
            ILogger<DocumentBLL> logger,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            FileService fileService,
            RabbitMQService rabbitMQService,
            DocumentRepository documentRepository,
            DocumentTypeRepository documentTypeRepository,
            ResumeBLL resumeBLL,
            ResumeRepository resumeRepository,
            DocumentResumeRepository documentResumeRepository
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;

            this.userManager = userManager;
            this.fileService = fileService;
            this.rabbitMQService = rabbitMQService;

            this.documentRepository = documentRepository;
            this.documentTypeRepository = documentTypeRepository;

            this.resumeBLL = resumeBLL;
            this.resumeRepository = resumeRepository;
            this.documentResumeRepository = documentResumeRepository;
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
        /// Uploads a new document.
        /// </summary>
        public async Task<Document> UploadDocumentAsync(IFormFile file, DateTime? fileLastModified, string typeName)
        {
            // Validation
            if (file == null)
            {
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

            // Document type
            if (!string.IsNullOrEmpty(typeName))
            {
                DocumentType documentType = await this.documentTypeRepository.GetByNameAsync(typeName);
                if (documentType != null)
                {
                    document.DocumentTypeId = documentType.Id;
                    document.DocumentType = documentType;
                }
            }

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

            // Resume
            if (document.DocumentType != null && document.DocumentType.Name == "uploaded-resume")
            {
                Resume resume = new Resume()
                {
                    Description = "Resume created during upload of document: " + document.DisplayName,
                    DocumentResume = new List<DocumentResume>() {
                        new DocumentResume()
                        {
                            DocumentId = document.Id,
                            Document = document,
                            Primary = true
                        }
                    }
                };

                await this.resumeBLL.CreateResumeAsync(resume);

                // Add document to parsing queue
                // TODO: Setting?
                Document documentToQueue = new Document()
                {
                    Id = document.Id,
                    Path = document.Path,
                    MimeType = document.MimeType,
                    SizeInBytes = document.SizeInBytes,
                    DocumentType = document.DocumentType
                };

                if (document.DocumentType != null)
                {
                    documentToQueue.DocumentType = new DocumentType()
                    {
                        Id = document.DocumentType.Id,
                        Name = document.DocumentType.Name,
                        DisplayName = document.DocumentType.DisplayName
                    };
                }

                this.rabbitMQService.Publish(documentToQueue, "rjm.background.tasks", "topic", "document.parser");
            }

            return document;
        }

        public async Task<Document> CreateDocumentContentAsync(Guid id, DocumentContent documentContent)
        {
            // Validation
            if (documentContent == null) { return null; }

            // Retrieve existing
            Document document = await this.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return null;
            }

            // Add the content to the document
            document.DocumentContents.Add(documentContent);

            document = await this.documentRepository.UpdateAsync(document);

            return document;
        }

        public async Task<Document> CreateDocumentContentsAsync(Guid id, List<DocumentContent> documentContents)
        {
            Document document = null;

            foreach (DocumentContent documentContent in documentContents)
            {
                document = await this.CreateDocumentContentAsync(id, documentContent);
            }

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
