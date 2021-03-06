using Microsoft.EntityFrameworkCore;
using RJM.API.Framework;
using RJM.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RJM.API.DAL.Repositories
{
    /// <summary>
    /// The repository for ResumeStates in the data access layer.
    /// </summary>
    public class ResumeStateRepository : Repository<ResumeState>
    {
        private new readonly RJMContext context;

        /// <summary>
        /// The constructor of the ResumeState repository.
        /// </summary>
        public ResumeStateRepository(RJMContext context) : base(context)
        {
            this.context = context;
        }

        // Additional functionality and overrides

        public async Task<ResumeState> GetByNameAsync(string name)
        {
            return await this.context.ResumeStates.FirstOrDefaultAsync(rs => rs.Name == name);
        }

        public async Task<IEnumerable<ResumeState>> GetWithLinkedEntitiesAsync()
        {
            return await this.context.ResumeStates
                .Include(x => x.Resumes)
                .Include(x => x.CreatedByUser)
                .Include(x => x.ModifiedByUser)
                .ToListAsync();
        }

        public async Task<ResumeState> GetWithLinkedEntitiesByIdAsync(Guid id)
        {
            return await this.context.ResumeStates
                .Include(x => x.Resumes)
                .Include(x => x.CreatedByUser)
                .Include(x => x.ModifiedByUser)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
