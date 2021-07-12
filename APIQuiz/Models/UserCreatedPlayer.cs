using APIQuiz.Models;

namespace APIQuiz.Models
{
    public class UserCreatedPlayer
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public Player GeneratePlayer()
        {
            Player player = new() { };
            player.Name = Name;
            
            return player;
        }
    }
}
