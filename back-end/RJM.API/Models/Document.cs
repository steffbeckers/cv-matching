using System;
using System.Collections.Generic;

namespace RJM.API.Models
{
    /// <summary>
    /// Document model
    /// </summary>
    public class Document
    {
        public Document()
        {
            // Relations

            //// One-to-many
            this.DocumentContents = new List<DocumentContent>();

            //// Many-to-many
            this.DocumentResume = new List<DocumentResume>();
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
        public User User { get; set; }

        /// <summary>
        /// The related foreign key DocumentTypeId for DocumentType of Document.
        /// </summary>
        public Guid? DocumentTypeId { get; set; }

        /// <summary>
        /// The related DocumentType of Document.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        //// One-to-many
        public IList<DocumentContent> DocumentContents { get; set; }

        //// Many-to-many

        /// <summary>
        /// The related Resumes of Document.
        /// </summary>
        public IList<DocumentResume> DocumentResume { get; set; }

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
