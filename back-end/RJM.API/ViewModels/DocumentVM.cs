using RJM.API.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RJM.API.ViewModels
{
    /// <summary>
    /// Document view model
    /// </summary>
    public class DocumentVM
    {
        public DocumentVM()
        {
            // Relations

            //// One-to-many
            this.Contents = new List<DocumentContentVM>();

            //// Many-to-many
            this.Resumes = new List<ResumeVM>();
        }

        // Properties

        /// <summary>
        /// The identifier of Document.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name property of Document.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The DisplayName property of Document.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// The Description property of Document.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Path property of Document.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The URL property of Document.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// The SizeInBytes property of Document.
        /// </summary>
        public long? SizeInBytes { get; set; }

        /// <summary>
        /// The FileLastModifiedOn property of Document.
        /// </summary>
        public DateTime? FileLastModifiedOn { get; set; }

        /// <summary>
        /// The MimeType property of Document.
        /// </summary>
        public string MimeType { get; set; }

        // Relations

        //// Many-to-one

        /// <summary>
        /// The related foreign key UserId for User of Document.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The related User of Document.
        /// </summary>
        public UserVM User { get; set; }

        /// <summary>
        /// The related foreign key DocumentTypeId for DocumentType of Document.
        /// </summary>
        public Guid? DocumentTypeId { get; set; }

        /// <summary>
        /// The related DocumentType of Document.
        /// </summary>
        public DocumentTypeVM DocumentType { get; set; }

        //// One-to-many

        /// <summary>
        /// The related Contents of Document.
        /// </summary>
        public IList<DocumentContentVM> Contents { get; set; }

        //// Many-to-many

        /// <summary>
        /// The related Resumes of Document.
        /// </summary>
        public IList<ResumeVM> Resumes { get; set; }

        ////// To create a link with Resume directly on create of Document.
        public Guid? ResumeId { get; set; }

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
