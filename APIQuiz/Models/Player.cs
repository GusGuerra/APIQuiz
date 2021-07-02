namespace APIQuiz.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }
        public void CopyUpdatedDataFrom(Player updatedPlayer)
        {
            Name = updatedPlayer.Name;
        }

        /// <summary>
        /// Increases player score and updates the ranking
        /// </summary>
        public void IncreaseScore()
        {
            Score += 1;
        }
    }
}
