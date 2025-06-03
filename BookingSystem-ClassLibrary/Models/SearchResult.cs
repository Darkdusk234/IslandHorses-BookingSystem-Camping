using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models
{
    public class SearchResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int AvaiableSpotsCount { get; set; }
        public IEnumerable<T> AvailableSpots { get; set; } = new List<T>();
        public string ErrorMessage { get; set; }
    }
}
