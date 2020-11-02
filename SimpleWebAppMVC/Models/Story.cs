using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Models
{
    public class Story
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<Task> Tasks { get; set; }
        public DateTime? Birthday { get; set; }

    }
}
