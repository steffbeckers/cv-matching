using System;

namespace RJM.BackgroundTasks.Models
{
    public class Document
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public string URL { get; set; }
		public long? SizeInBytes { get; set; }
		public DateTime? FileLastModifiedOn { get; set; }
		public string MimeType { get; set; }
		public Guid? DocumentTypeId { get; set; }
		public DocumentType DocumentType { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
		public DateTime? DeletedOn { get; set; }
    }
}
