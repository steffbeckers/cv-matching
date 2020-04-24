using Microsoft.EntityFrameworkCore;
using RJM.API.Framework;
using RJM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RJM.API.DAL.Repositories
{
    /// <summary>
    /// The repository for Documents in the data access layer.
    /// </summary>
    public class DocumentRepository : Repository<Document>
    {
        private new readonly RJMContext context;

        /// <summary>
        /// The constructor of the Document repository.
        /// </summary>
        public DocumentRepository(RJMContext context) : base(context)
        {
            this.context = context;
        }

        // Additional functionality and overrides

        public async Task<IEnumerable<Document>> GetWithLinkedEntitiesAsync()
        {
            return await this.context.Documents
                .Include(x => x.DocumentType)
                .Include(x => x.DocumentResume)
                    .ThenInclude(x => x.Resume)
                .Include(x => x.CreatedByUser)
                .Include(x => x.ModifiedByUser)
                .OrderByDescending(x => x.ModifiedOn)
                .ToListAsync();
        }

        public async Task<Document> GetWithLinkedEntitiesByIdAsync(Guid id)
        {
            return await this.context.Documents
                .Include(x => x.DocumentType)
                .Include(x => x.DocumentResume)
                    .ThenInclude(x => x.Resume)
                .Include(x => x.CreatedByUser)
                .Include(x => x.ModifiedByUser)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<Document> GetByDocumentTypeId(Guid documentTypeId)
        {
            return this.context.Documents
                .Where(t => t.DocumentTypeId == documentTypeId)
                .ToList();
        }

        public IEnumerable<Document> GetByResumeId(Guid resumeId)
        {
            return this.context.DocumentResume
                .Include(x => x.Document)
                .Where(x => x.ResumeId == resumeId)
                .Select(x => x.Document)
                .ToList();
        }
    }
}
