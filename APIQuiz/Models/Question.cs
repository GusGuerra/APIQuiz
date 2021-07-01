using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace APIQuiz.Models
{
    public class Question
    {
        public List<int> SeenBy { get; set; }
        public int Id { get; set; }
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
        
        public PlayerFriendlyQuestion GenerateUserFriendlyQuestion()
        {
            PlayerFriendlyQuestion playerFriendlyQuestion = new();
            playerFriendlyQuestion.Body = Body;
            playerFriendlyQuestion.Category = Category;
            playerFriendlyQuestion.Difficulty = Difficulty;
            playerFriendlyQuestion.Alternatives = (List<string>)IncorrectAnswers;
            playerFriendlyQuestion.Alternatives.Add(CorrectAnswer);
            playerFriendlyQuestion.Alternatives.Sort();

            return playerFriendlyQuestion;
        }
    }
}
