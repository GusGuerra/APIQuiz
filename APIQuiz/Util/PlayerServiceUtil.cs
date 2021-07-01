namespace APIQuiz.Util
{
    public static class PlayerServiceUtil
    {
        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int MAX_PLAYERS_PER_PAGE = 5;
        public const int MAX_PLAYER_NUMBER = 1000;

        /// <summary>
        /// Checks for page numbers out of bounds.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="playerCount"></param>
        /// <returns>In-bounds page number (The first page or the last page)</returns>
        public static int PageNumberAdjustment(int page, int playerCount)
        {
            // lowest page number = 1
            if (page <= 0)
                return 1;

            int highestPageNumber = HighestPageNumberCalculation(playerCount);

            if (page > highestPageNumber)
                return highestPageNumber;

            return page;
        }

        /// <summary>
        /// Calculates the highest page number given the player count
        /// </summary>
        /// <param name="playerCount"></param>
        /// <returns>The highest possible page number</returns>
        public static int HighestPageNumberCalculation(int playerCount)
        {
            // highest page number = min(1, amount of players/max players)
            if (playerCount <= MAX_PLAYERS_PER_PAGE)
            {
                return 1;
            }
            
            int highestPageNumber = playerCount / MAX_PLAYERS_PER_PAGE;
            if (playerCount % MAX_PLAYERS_PER_PAGE != 0)
                highestPageNumber += 1;

            return highestPageNumber;
        }

        /// <summary>
        /// Calculates the amount of elements available
        /// in the given range (limited by MAX_PLAYERS_PER_PAGE).
        /// </summary>
        /// <param name="firstIndexInPage"></param>
        /// <param name="playerCount"></param>
        /// <returns>The minimum between amount of elements in the given range and MAX_PLAYER_PER_PAGE </returns>
        public static int PlayerAmountCalculation(int currentIndex, int listSize)
        {
            int highestPossibleAmount = (listSize - 1) - currentIndex + 1;

            if (MAX_PLAYERS_PER_PAGE > highestPossibleAmount)
            {
                return highestPossibleAmount;
            }
            
            return MAX_PLAYERS_PER_PAGE;
        }
    }
}
