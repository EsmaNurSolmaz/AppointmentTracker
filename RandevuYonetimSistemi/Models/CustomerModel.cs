using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandevuYonetimSistemi.Models
{
    public class CustomerModel
    {
        [Key]
        public int CustomerId { get; set; }

        public string FullName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<AppointmentModel> Appointments { get; set; }
            = new List<AppointmentModel>();
    }
}
