using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Models
{
    public class Epic
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<Story> Stories { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
