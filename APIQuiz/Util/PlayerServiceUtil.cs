namespace APIQuiz.Util
{
    public static class PlayerServiceUtil
    {
        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int MAX_PLAYERS_PER_PAGE = 5;
        public const int MAX_PLAYER_NUMBER = 1000;
        public const int POINTS_PER_CORRECT_ANSWER = 10;
        public const int POINTS_PER_INCORRECT_ANSWER = 0;
        public const int FIRST_STREAK_THRESHOLD = 3;
        public const int FIRST_STREAK_POINTS = 15;
        public const int SECOND_STREAK_THRESHOLD = 5;
        public const int SECOND_STREAK_POINTS = 20;
        public const int MINIMUM_PASSWORD_LENGTH = 3;

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

        /// <summary>
        /// Checks if the specified player has a valid name
        /// </summary>
        /// <param name="player"></param>
        /// <returns> True if the player has a valid name; Otherwise, false </returns>
        public static bool IsValidName(string playerName)
        {
            return !string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName);
        }

        /// <summary>
        /// Checks is a password is secure enough
        /// </summary>
        /// <param name="password"></param>
        /// <returns> True if the password is considered safe; Otherwise, false </returns>
        public static bool IsValidPassword(string password)
        {

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (password.Length < MINIMUM_PASSWORD_LENGTH)
            {
                return false;
            }

            if (password.Contains(" "))
            {
                return false;
            }

            if (CheckThreeConsecutiveDigitsSubstring(password))
            {
                return false;
            }

            string lowerCasePassword = password.ToLower();

            if (lowerCasePassword.Contains("password"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a string has three consecutives digits as a substring
        /// </summary>
        /// <param name="str"></param>
        /// <returns> True is the input string has three consecutive digits as a substring; Otherwise false </returns>
        public static bool CheckThreeConsecutiveDigitsSubstring(string str)
        {
            if (str.Length < 3)
            {
                return false;
            }

            for (int i = 0; i <= str.Length - 3; i++)
            {
                if (char.IsDigit(str[i]) && char.IsDigit(str[i + 1]) && char.IsDigit(str[i + 2]))
                {
                    if ((int)char.GetNumericValue(str[i]) + 1 == (int)char.GetNumericValue(str[i + 1]) &&
                        (int)char.GetNumericValue(str[i + 1]) + 1 == (int)char.GetNumericValue(str[i + 2]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
