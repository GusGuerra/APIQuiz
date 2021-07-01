using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIQuiz.Models
{
    public class PlayerFriendlyQuestion : Question
    {
        public List<string> Alternatives { get; set; }
    }
}
