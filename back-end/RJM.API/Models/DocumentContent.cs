using System;

namespace RJM.API.Models
{
    public class DocumentContent
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public double? Confidence { get; set; }

        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
