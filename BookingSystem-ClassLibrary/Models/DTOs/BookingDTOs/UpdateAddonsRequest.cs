using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs
{
    public class UpdateAddonsRequest
    {
        /// <summary>
        /// Add WiFi to the booking (Yes = true, No = false)
        /// </summary>
        public bool Wifi { get; set; }

        /// <summary>
        /// Add parking to the booking (Yes = true, No = false)
        /// </summary>
        public bool Parking { get; set; }
        
    }
}
