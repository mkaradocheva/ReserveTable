namespace ReserveTable.Models.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class CreateReviewBindingModel
    {
        private const string RateRangeErrorMessage = "Rate must be between 1 and 10.";
        private const string CommentLengthErrorMessage = "Comment must be between 5 and 100 characters long.";

        [Range(1, 10, ErrorMessage = RateRangeErrorMessage)]
        public int Rate { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = CommentLengthErrorMessage)]
        public string Comment { get; set; }
    }
}
