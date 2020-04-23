using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RJM.API.ViewModels
{
    public class DocumentContentVM
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public double? Confidence { get; set; }

        public Guid DocumentId { get; set; }
        public DocumentVM Document { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
