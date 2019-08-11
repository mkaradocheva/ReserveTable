namespace ReserveTable.Models.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class CreateReviewViewModel
    {
        private const int MinRate = 1;
        private const int MaxRate = 10;
        private const int MinCommentLength = 5;
        private const int MaxCommentLength = 100;

        [Required]
        [Range(minimum: MinRate, maximum: MaxRate, ErrorMessage = "Rate must be between {0} and {1}.")]
        public int Rate { get; set; }

        [Required]
        [StringLength(maximumLength: MaxCommentLength, MinimumLength = MinCommentLength, ErrorMessage = "Comment must be between {0} and {1} characters long.")]
        public string Comment { get; set; }
    }
}
