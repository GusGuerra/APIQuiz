namespace APIQuiz.Models
{
    public class UserCreatedPlayer
    {
        public string Name { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Creates a Player object (no password field) with the same name.
        /// </summary>
        /// <returns> Player object with the same name </returns>
        public Player GeneratePlayer()
        {
            Player player = new() { };
            player.Name = Name;
            
            return player;
        }
    }
}
