using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests.ModelObjects
{
    public class Post
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }

        public Post()
        {

        }

        public static Post NewPost(int userId, int postId, string title, string body) => new Post()
        {
            UserId = userId,
            Id = postId,
            Title = title,
            Body = body
        };

    }
}
