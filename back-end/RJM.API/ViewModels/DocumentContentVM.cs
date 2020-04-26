using RJM.API.Models;
using System;

namespace RJM.API.ViewModels
{
    public class DocumentContentVM
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DocumentContentTextType TextType { get; set; }
        public double? Confidence { get; set; }

        public Guid DocumentId { get; set; }
        public DocumentVM Document { get; set; }
    }
}
