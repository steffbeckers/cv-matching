using RJM.API.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RJM.API.ViewModels
{
    /// <summary>
    /// Resume view model
    /// </summary>
    public class ResumeVM
    {
        public ResumeVM()
        {
            // Relations

            //// Many-to-many
            this.Documents = new List<DocumentVM>();
            this.Skills = new List<SkillVM>();
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
        [Required]
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
        public UserVM User { get; set; }

        /// <summary>
        /// The related foreign key ResumeStateId for ResumeState of Resume.
        /// </summary>
        public Guid ResumeStateId { get; set; }

        /// <summary>
        /// The related ResumeState of Resume.
        /// </summary>
        public ResumeStateVM ResumeState { get; set; }

        //// Many-to-many

        /// <summary>
        /// The related Documents of Resume.
        /// </summary>
        public IList<DocumentVM> Documents { get; set; }

        ////// To create a link with Document directly on create of Resume.
        public Guid? DocumentId { get; set; }

        /// <summary>
        /// The related Skills of Resume.
        /// </summary>
        public IList<SkillVM> Skills { get; set; }

        ////// To create a link with Skill directly on create of Resume.
        public Guid? SkillId { get; set; }

        public int? SkillLevel { get; set; }
        public string SkillDescription { get; set; }

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
        /// The Id of the user who created the record
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// The user who created the record
        /// </summary>
        public UserVM CreatedByUser { get; set; }

        /// <summary>
        /// The Id of the user who last modified the record
        /// </summary>
        public Guid ModifiedByUserId { get; set; }

        /// <summary>
        /// The user who last modified the record
        /// </summary>
        public UserVM ModifiedByUser { get; set; }

    }
}
