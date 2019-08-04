namespace ReserveTable.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        private const string CommentLengthErrorMessage = "Comment should be up to 100 symbols long.";
        private const string RateErrorMessage = "Rate must be a number between 1 and 10.";

        public string Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = CommentLengthErrorMessage)]
        public string Comment { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = RateErrorMessage)]
        public double Rate { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; }
        public ReserveTableUser User { get; set; }

        [Required]
        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
