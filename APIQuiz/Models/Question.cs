using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;

namespace APIQuiz.Models
{
    public class Question
    {
        [JsonIgnore]
        public List<int> SeenBy { get; set; }
        [JsonIgnore]
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
            playerFriendlyQuestion.Alternatives = new((List<string>)IncorrectAnswers);
            playerFriendlyQuestion.Alternatives.Add(CorrectAnswer);
            playerFriendlyQuestion.Alternatives.Sort();

            return playerFriendlyQuestion;
        }
    }
}
