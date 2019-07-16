using ApiTests.ModelObjects;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace Tests
{
    public class ApiTests
    {
        [SetUp]
        public void Setup()
        {
        }
       

        [TestCase("http://jsonplaceholder.typicode.com/", "posts", Method.GET, @"application/json; charset=utf-8", TestName = "Check Content Type is application/json")] 
        public void ContentTypeAndStatusCodeTest(string baseUrl, string requestUrl, Method requestMethod, string expectedContentType)
        {
            // arrange
            RestClient client = new RestClient(baseUrl);
            RestRequest request = new RestRequest(requestUrl, requestMethod);

            // act
            IRestResponse response = client.Execute(request);

            // assert            
            Assert.That(response.ContentType, Is.EqualTo(expectedContentType));
        }

        [TestCase("http://jsonplaceholder.typicode.com/", "posts", Method.GET, HttpStatusCode.OK, TestName = "Get Post Status Code OK")]
        [TestCase("http://jsonplaceholder.typicode.com/", "posts", Method.POST, HttpStatusCode.Created, TestName = "Create Post Status Code Created")]
        public void StatusCodeTest(string baseUrl, string requestUrl, Method requestMethod, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestClient client = new RestClient(baseUrl);
            RestRequest request = new RestRequest(requestUrl, requestMethod);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));            
        }

        [TestCase("http://jsonplaceholder.typicode.com/", "posts", Method.POST, HttpStatusCode.Created, @"application/json; charset=utf-8", TestName = "Create Post")]
        public void CreatePost(string baseUrl, string requestUrl, Method requestMethod, HttpStatusCode expectedHttpStatusCode, string expectedContentType)
        {
            // arrange
            RestClient client = new RestClient(baseUrl);
            RestRequest createRequest = new RestRequest(requestUrl, requestMethod);            
            var post = Post.NewPost(1, 1, "test title", "test body");
            createRequest.AddJsonBody(post);

            // act
            IRestResponse response = client.Execute(createRequest);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
            Assert.That(response.ContentType, Is.EqualTo(expectedContentType));
        }

        [TestCase("http://jsonplaceholder.typicode.com/", "posts/1", Method.DELETE, HttpStatusCode.OK, TestName = "Delete Post")]
        public void DeletePost(string baseUrl, string requestUrl, Method requestMethod, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestClient client = new RestClient(baseUrl);
            RestRequest deleteRequest = new RestRequest(requestUrl, requestMethod);
            RestRequest getRequest = new RestRequest(requestUrl, Method.GET);

            // act
            // check resource exists
            IRestResponse getResponse = client.Execute(getRequest);
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Delete
            IRestResponse deleteResponse = client.Execute(deleteRequest);
            // check delete 
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(expectedHttpStatusCode));

            IRestResponse getResponseAfterDelete = client.Execute(getRequest);

            // assert
            Assert.That(getResponseAfterDelete.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));            
        }
    }
}