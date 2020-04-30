using System;
using System.ComponentModel.DataAnnotations;

namespace RJM.API.ViewModels
{
    /// <summary>
    /// ResumeState view model
    /// </summary>
    public class ResumeStateVM
    {
        /// <summary>
        /// The identifier of ResumeState.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name property of ResumeState.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The DisplayName property of ResumeState.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
    }
}
