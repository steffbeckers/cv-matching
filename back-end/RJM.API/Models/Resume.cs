using System;
using System.Collections.Generic;

namespace RJM.API.Models
{
    /// <summary>
    /// Resume model
    /// </summary>
    public class Resume
    {
        public Resume()
        {
            // Relations

            //// Many-to-many
            this.DocumentResume = new List<DocumentResume>();
            this.ResumeSkill = new List<ResumeSkill>();
        }

        // Properties

        /// <summary>
        /// The identifier of Resume.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name property of Resume.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The DisplayName property of Resume.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The JobTitle property of Resume.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// The Description property of Resume.
        /// </summary>
        public string Description { get; set; }

        // Relations

        //// Many-to-one

        /// <summary>
        /// The related foreign key UserId for User of Resume.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The related User of Resume.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The related foreign key ResumeStateId for ResumeState of Resume.
        /// </summary>
        public Guid ResumeStateId { get; set; }

        /// <summary>
        /// The related ResumeState of Resume.
        /// </summary>
        public ResumeState ResumeState { get; set; }

        //// Many-to-many

        /// <summary>
        /// The related Documents of Resume.
        /// </summary>
        public IList<DocumentResume> DocumentResume { get; set; }
        /// <summary>
        /// The related Skills of Resume.
        /// </summary>
        public IList<ResumeSkill> ResumeSkill { get; set; }

        // Generic properties

        /// <summary>
        /// The date and time of when the record is created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The date and time of when the record is modified
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// The date and time of when the record is (soft) deleted
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// The Id of the user who created the record
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// The user who created the record
        /// </summary>
        public User CreatedByUser { get; set; }

        /// <summary>
        /// The Id of the user who last modified the record
        /// </summary>
        public Guid ModifiedByUserId { get; set; }

        /// <summary>
        /// The user who last modified the record
        /// </summary>
        public User ModifiedByUser { get; set; }

        // TODO: Multi-tenancy
        //public Guid TenantId { get; set; }
    }
}
