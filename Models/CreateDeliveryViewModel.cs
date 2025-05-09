// Models/ViewModels/CreateDeliveryViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace mycourier.Models.ViewModels
{
    public class CreateDeliveryViewModel
    {
        [Required]
        public string FromAddress { get; set; }

        [Required]
        public string ToAddress { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int WeightId { get; set; }

        [Required]
        public int LocationId { get; set; }
    }
}
