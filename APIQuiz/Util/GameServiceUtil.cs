namespace APIQuiz.Util
{
    public static class GameServiceUtil
    {
        public const int DEFAULT_NEW_QUESTION_AMOUNT = 1;
        public const string EXTERNAL_API_SCHEME = "https";
        public const string EXTERNAL_API_HOST = "opentdb.com";
        public const string EXTERNAL_API_PATH_TOKEN = "api_token.php";
        public const string EXTERNAL_API_PATH_QUESTION = "api.php";
        public const string EXTERNAL_API_KEY_COMMAND = "command";
        public const string EXTERNAL_API_KEY_TOKEN = "token";
        public const string EXTERNAL_API_KEY_QUESTION_AMOUNT = "amount";
        public const string EXTERNAL_API_VALUE_TOKEN_REQUEST = "request";

        /// <summary>
        /// Appends the specfied query to an already existing uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddQuery(string uri, string key, string value)
        {
            string queryString = string.Format("{0}={1}", key, value);
            return AddQuery(uri, queryString);
        }

        public static string AddQuery(string uri, string queryString)
        {
            string newUri = uri;
            if (newUri.Contains('?')) // if this is not the first queryString in the uri
            {
                newUri += "&" + queryString;
            }
            else // if this is the first queryString in the uri
            {
                newUri += "?" + queryString;
            }
            return newUri;
        }
    }
}
