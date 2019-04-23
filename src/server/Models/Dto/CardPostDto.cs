using System.ComponentModel.DataAnnotations;
using Server.Infrastructure;

namespace Server.Models.Dto
{
    /// <summary>
    /// Card model Dto to post to controller
    /// </summary>
    public class CardPostDto
    {
        /// <summary>
        /// Short name of the cards
        /// </summary>
        /// <returns><see langword="string"/> short card name representation</returns>
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Card <see cref="Currency"/>
        /// </summary>
        [Required]
        [Range(0, 2)]
        public int Currency { get; set; }

        /// <summary>
        /// Card <see cref="CardType"/>
        /// </summary>
        [Required]
        [Range(1, 4)]
        public int Type { get; set; }
    }
}