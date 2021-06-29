using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace APIQuiz.Models
{
    public class Question
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }
        [JsonPropertyName("question")]
        public string Body { get; set; }
        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonPropertyName("incorrect_answers")]
        public IEnumerable<string> IncorrectAnswers { get; set; }
        public List<int> SeenBy { get; set; }
        public int Id { get; set; }
    }
}
