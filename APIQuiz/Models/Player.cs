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
    }
}
