using APIQuiz.Util;

namespace APIQuiz.Models
{
    public class Player
    {
        private int Streak { get; set; }
        public int Id { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }
        public void CopyDataFrom(Player updatedPlayer)
        {
            Name = updatedPlayer.Name;
        }

        /// <summary>
        /// Increases player score and updates the ranking
        /// </summary>
        public void IncreaseScore(int points)
        {
            if (points != 0)
            {
                Streak++;
            }
            else
            {
                Streak = 0;
                return;
            }

            if(Streak >= PlayerServiceUtil.FIRST_STREAK_THRESHOLD && Streak < PlayerServiceUtil.SECOND_STREAK_THRESHOLD)
            {
                points = PlayerServiceUtil.FIRST_STREAK_POINTS;
            }
            else if (Streak >= PlayerServiceUtil.SECOND_STREAK_THRESHOLD)
            {
                points = PlayerServiceUtil.SECOND_STREAK_POINTS;
            }

            Score += points;
        }
    }
}
