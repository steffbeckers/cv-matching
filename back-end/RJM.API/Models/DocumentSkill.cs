using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RJM.API.Models
{
    public class DocumentSkill
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        public Guid SkillId { get; set; }
        public Skill Skill { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public Guid ModifiedByUserId { get; set; }
        public User ModifiedByUser { get; set; }

        // TODO: Multi-tenancy
        //public Guid TenantId { get; set; }
    }
}
