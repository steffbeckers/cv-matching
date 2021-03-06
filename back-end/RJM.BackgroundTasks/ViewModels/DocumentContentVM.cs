﻿using System;

namespace RJM.API.ViewModels
{
    public class DocumentContentVM
    {
        //public Guid Id { get; set; }
        public string Text { get; set; }
        public double? Confidence { get; set; }
        public DocumentContentTextType TextType { get; set; }

        public Guid DocumentId { get; set; }

        //public DateTime CreatedOn { get; set; }
        //public DateTime ModifiedOn { get; set; }
        //public DateTime? DeletedOn { get; set; }
    }

    public enum DocumentContentTextType
    {
        WORD,
        LINE
    }
}
