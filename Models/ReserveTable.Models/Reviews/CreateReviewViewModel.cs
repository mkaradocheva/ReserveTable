using System.ComponentModel.DataAnnotations;

namespace ReserveTable.Models.Reviews
{
    public class CreateReviewViewModel
    {
        private const string RateRangeErrorMessage = "Rate must be between 1 and 10.";
        private const string CommentLengthErrorMessage = "Comment must be between 5 and 100 characters long.";

        [Required]
        [Range(1, 10, ErrorMessage = RateRangeErrorMessage)]
        public int Rate { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = CommentLengthErrorMessage)]
        public string Comment { get; set; }
    }
}
