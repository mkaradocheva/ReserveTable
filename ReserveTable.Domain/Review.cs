namespace ReserveTable.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        private const int MaxCommentLength = 100;
        private const int MinRate = 1;
        private const int MaxRate = 10;

        public string Id { get; set; }

        [Required]
        [MaxLength(MaxCommentLength, ErrorMessage = "Comment should be up to {0} symbols long.")]
        public string Comment { get; set; }

        [Required]
        [Range(minimum: MinRate, maximum: MaxRate, ErrorMessage = "Rate must be a number between {0} and {1}.")]
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
