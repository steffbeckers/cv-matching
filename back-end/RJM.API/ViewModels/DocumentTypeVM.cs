using System;
using System.ComponentModel.DataAnnotations;

namespace RJM.API.ViewModels
{
    /// <summary>
    /// DocumentType view model
    /// </summary>
    public class DocumentTypeVM
    {
        /// <summary>
        /// The identifier of DocumentType.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name property of DocumentType.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The DisplayName property of DocumentType.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
    }
}
