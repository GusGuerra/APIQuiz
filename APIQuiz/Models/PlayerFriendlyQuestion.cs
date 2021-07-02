using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIQuiz.Models
{
    public class PlayerFriendlyQuestion
    {
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Body { get; set; }
        public List<string> Alternatives { get; set; }
    }
}
