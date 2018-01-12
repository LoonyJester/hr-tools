using System.Collections.Generic;

namespace HRTools.Crosscutting.Common.Models
{
    public class GrigData<T> where T: class
    {
        public IEnumerable<T> Data { get; set; } 

        public int TotalCount { get; set; }
    }
}
