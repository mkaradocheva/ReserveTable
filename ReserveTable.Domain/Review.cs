using System.ComponentModel.DataAnnotations;

namespace ReserveTable.Domain
{
    public class Review 
    {
        private const string CommentLengthErrorMessage = "Comment should be up to 300 symbols long.";

        public string Id { get; set; }

        [MaxLength(300, ErrorMessage = CommentLengthErrorMessage)]
        public string Comment { get; set; }

        [Required]
        public double Rate { get; set; }

        public string UserId { get; set; }
        public ReserveTableUser User { get; set; }

        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
