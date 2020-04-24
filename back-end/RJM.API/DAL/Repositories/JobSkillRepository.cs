using RJM.API.Framework;
using RJM.API.Models;
using System;
using System.Linq;

namespace RJM.API.DAL.Repositories
{
    /// <summary>
    /// The repository for JobSkills in the data access layer.
    /// </summary>
    public class JobSkillRepository : Repository<JobSkill>
    {
        private new readonly RJMContext context;

        /// <summary>
        /// The constructor of the JobSkill repository.
        /// </summary>
        public JobSkillRepository(RJMContext context) : base(context)
        {
            this.context = context;
        }

        // Additional functionality and overrides

        public JobSkill GetByJobAndSkillId(Guid jobId, Guid skillId)
        {
            return this.context.JobSkill
                .Where(x => x.JobId == jobId && x.SkillId == skillId)
                .SingleOrDefault();
        }

        public JobSkill GetBySkillAndJobId(Guid skillId, Guid jobId)
        {
            return this.context.JobSkill
                .Where(x => x.SkillId == skillId && x.JobId == jobId)
                .SingleOrDefault();
        }
    }
}
