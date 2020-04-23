using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RJM.API.BLL;
using RJM.API.Models;
using RJM.API.ViewModels;
using Microsoft.AspNetCore.Http;

namespace RJM.API.Controllers
{
	/// <summary>
	/// The Documents controller.
	/// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
	[Produces("application/json")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> logger;
        private readonly IMapper mapper;
        private readonly DocumentBLL bll;

		/// <summary>
		/// The constructor of the Documents controller.
		/// </summary>
        public DocumentsController(
            ILogger<DocumentsController> logger,
			IMapper mapper,
            DocumentBLL bll
        )
        {
            this.logger = logger;
			this.mapper = mapper;
            this.bll = bll;
        }

        // GET: api/documents
		/// <summary>
		/// Retrieves all documents.
		/// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentVM>>> GetDocuments()
        {
            IEnumerable<Document> documents = await this.bll.GetAllDocumentsAsync();

			// Mapping
            return Ok(this.mapper.Map<IEnumerable<Document>, List<DocumentVM>>(documents));
        }

        // GET: api/documents/{id}
		/// <summary>
		/// Retrieves a specific document.
		/// </summary>
		/// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentVM>> GetDocument([FromRoute] Guid id)
        {
            Document document = await this.bll.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

			// Mapping
            return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }

        // POST: api/documents
        /// <summary>
        /// Uploads a new document.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileLastModified"></param>
        /// <param name="typeName"></param>
        [HttpPost]
        public async Task<ActionResult<DocumentVM>> UploadDocument(
            [FromForm] IFormFile file,
            [FromForm] DateTime fileLastModified,
            [FromForm] string typeName
        )
        {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Document document = await this.bll.UploadDocumentAsync(file, fileLastModified, typeName);

			// Mapping
            return CreatedAtAction(
				"GetDocument",
				new { id = document.Id },
				this.mapper.Map<Document, DocumentVM>(document)
			);
        }

        // PUT: api/documents/{id}
        /// <summary>
        /// Updates a specific document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentVM"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<DocumentVM>> UpdateDocument([FromRoute] Guid id, [FromBody] DocumentVM documentVM)
        {
			// Validation
            if (!ModelState.IsValid || id != documentVM.Id)
            {
                return BadRequest(ModelState);
            }

			// Mapping
            Document document = this.mapper.Map<DocumentVM, Document>(documentVM);

            document = await this.bll.UpdateDocumentAsync(document);

			// Mapping
			return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }

        // POST: api/documents/{id}/content
        /// <summary>
        /// Adds document content to a document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentContentVM"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<DocumentVM>> CreateDocumentContents([FromRoute] Guid id, [FromBody] DocumentContentVM documentContentVM)
        {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapping
            DocumentContent documentContent = this.mapper.Map<DocumentContentVM, DocumentContent>(documentContentVM);

            Document document = await this.bll.CreateDocumentContentAsync(id, documentContent);

            // Mapping
            return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }

        // POST: api/documents/{id}/contents
        /// <summary>
        /// Adds multiple document contents to a document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentContentsVM"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<DocumentVM>> CreateDocumentContents([FromRoute] Guid id, [FromBody] List<DocumentContentVM> documentContentsVM)
        {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapping
            List<DocumentContent> documentContents = this.mapper.Map<List<DocumentContentVM>, List<DocumentContent>>(documentContentsVM);

            Document document = await this.bll.CreateDocumentContentsAsync(id, documentContents);

            // Mapping
            return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }

        // PUT: api/documents/resumes/link
        /// <summary>
        /// Links a specific resume to document.
        /// </summary>
        /// <param name="documentResume"></param>
        [HttpPut("Resumes/Link")]
        public async Task<ActionResult<DocumentVM>> LinkResumeToDocument([FromBody] DocumentResume documentResume)
        {
			// Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Document document = await this.bll.LinkResumeToDocumentAsync(documentResume);

            // Mapping
            return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }

        // PUT: api/documents/resumes/unlink
		/// <summary>
		/// Unlinks a specific resume from document.
		/// </summary>
		/// <param name="documentResume"></param>
        [HttpPut("Resumes/Unlink")]
        public async Task<ActionResult<DocumentVM>> UnlinkResumeFromDocument([FromBody] DocumentResume documentResume)
        {
			// Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Document document = await this.bll.UnlinkResumeFromDocumentAsync(documentResume);

            // Mapping
            return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }

        // DELETE: api/documents/{id}
		/// <summary>
		/// Deletes a specific document.
		/// </summary>
		/// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DocumentVM>> DeleteDocument([FromRoute] Guid id)
        {
            // Retrieve existing document
            Document document = await this.bll.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            await this.bll.DeleteDocumentAsync(document);

            // Mapping
            return Ok(this.mapper.Map<Document, DocumentVM>(document));
        }
    }
}
