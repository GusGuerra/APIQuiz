using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace APIQuiz.Models
{
    public class Question
    {
        public List<int> SeenBy { get; set; }
        public int Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        [JsonPropertyName("question")]
        public string Body { get; set; }
        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonPropertyName("incorrect_answers")]
        public IEnumerable<string> IncorrectAnswers { get; set; }
    }
}
