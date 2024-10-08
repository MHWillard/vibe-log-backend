using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using vibe-log-backend.vibe_backend.models;

namespace unit_tests
{

    //1. set up fixture equivalent
    //2. set up backend only post test
    //3. set up client based test to backend

    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TestCreatePost()
        {
            // Arrange
            var post = new Post { post_id = 1, userid = 2, content = "Test content" };
            var jsonContent = JsonContent.Create(post);

            // Act
            var response = await _client.PostAsync("/new-post", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await JsonSerializer.Deserialize<Post>(await response.Content.ReadFromJsonAsync<Post>());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(result.post_id);
            Assert.Equal(post.userid, result.userid);
            Assert.Equal(post.content, result.content);
        }

        [Fact]
        public async Task TestInvalidRequest()
        {
            // Arrange
            var invalidPost = new Post { post_id = 0, userid = 0, content = "" };
            var jsonContent = JsonContent.Create(invalidPost);

            // Act
            var response = await _client.PostAsync("/new-post", jsonContent);

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }

}