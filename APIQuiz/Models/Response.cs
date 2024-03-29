﻿using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace APIQuiz.Models
{
    public class Response
    {
        public string Token { get; set; }
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }
        [JsonPropertyName("response_message")]
        public string ResponseMessage { get; set; }
        [JsonPropertyName("results")]
        public IEnumerable<Question> Result { get; set; }
    }
}
