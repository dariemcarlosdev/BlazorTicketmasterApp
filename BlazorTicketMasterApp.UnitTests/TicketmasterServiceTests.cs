using BlazorTicketmasterApp.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace BlazorTicketMasterApp.UnitTests
{
    public class TicketmasterServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration; //Mocking the IConfiguration
        private readonly Mock<ILogger<TicketmasterService>> _mockLogger; //Mocking the ILogger
        private readonly CustomHttpMessageHandler _mockHttpMessageHandler; //Mocking the HttpMessageHandler
        private readonly HttpClient _httpClient; //Creating an instance of HttpClient
        private readonly TicketmasterService _ticketmasterService; //Creating an instance of TicketmasterService

        //Constructor for the TicketmasterServiceTests class
        public TicketmasterServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<TicketmasterService>>();
            //The SendAsyncFunc delegate is initialized in the constructor to satisfy the required member constraint.
            _mockHttpMessageHandler = new CustomHttpMessageHandler
            {
                SendAsyncFunc = (request, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))
            };
            _httpClient = new HttpClient(_mockHttpMessageHandler) { BaseAddress = new Uri("https://app.ticketmaster.com/") };

            _mockConfiguration.Setup(config => config["Ticketmaster:ApiKey"]).Returns("test_api_key");
            _ticketmasterService = new TicketmasterService(_httpClient, _mockConfiguration.Object, _mockLogger.Object);

            //get appsetting.json property value from BrazorTicketMasterApp project

        }


        /// <summary>
        /// This method is used to test the SearchAttractionsAsync method of the TicketmasterService, I am testing if the method returns the expected result when the API response is successful.
        /// The expected result is a list of attractions with the expected count, id, and name.It just tests a the success scenario.
        /// </summary>
        /// <param name="responseContext"></param>
        /// <param name="expectedCount"></param>
        /// <param name="expectedId"></param>
        /// <param name="expectedName"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("{\"_embedded\": {\"attractions\": [{\"id\": \"1\", \"name\": \"Attraction 1\"}]}}", 1, "1", "Attraction 1")] //Inlinedata attribute is used to pass the test data to the test method, and define the expected result
        public async Task SearchAttractionsAsync_ReturnAttractions_WhenApiResponseIsSuccessful(string responseContext, int expectedCount, string expectedId, string expectedName)
        {
            //Arrenge: In this section, we are setting up the mock HttpMessageHandler to return a successful response
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContext)
            };

            //Setting up the SendAsyncFunc delegate to return the response
            _mockHttpMessageHandler.SendAsyncFunc = (request, cancellationToken) =>
            {
                return Task.FromResult(response);
            };


            //Act: In this section, we are calling the SearchAttractionsAsync method of the TicketmasterService
            var result = await _ticketmasterService.SearchAttractionsAsync("test");

            //Assert: In this section, we are verifying the result of the SearchAttractionsAsync method
            Assert.NotNull(result); //Checking if the result is not null
            Assert.Equal(expectedCount, result.Count); //Checking if the count of the result is equal to the expected count
            Assert.Equal(expectedId, result[0].id); //Checking if the id of the first item in the result is equal to the expected id
            Assert.Equal(expectedName, result[0].name); //Checking if the name of the first item in the result is equal to the expected name

        }

        //Test method to test the SearchAttractionsAsync method of the TicketmasterService, I am testing if the method throws an exception when the API response is not successful.
        [Fact]
        public async Task SearchAttractionsAsync_ReturnEmptyAttracionsList_WhenApiResponseIsNotSuccessful()
        {
            // Arrange: In this section, we are setting up the mock HttpMessageHandler to return a failed response
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            // Setting up the SendAsyncFunc delegate to return the response
            _mockHttpMessageHandler.SendAsyncFunc = (request, cancellationToken) =>
            {
                return Task.FromResult(response);
            };

            // Act and Assert:In this section, we are calling the SearchAttractionsAsync method of the TicketmasterService and verifying  an empty list is returned if an exception is thrown  
            Assert.Empty(await _ticketmasterService.SearchAttractionsAsync("test"));

        }



        //Test method to test the GetAttractionByIdAsync method of the TicketmasterService, I am testing if the method returns the expected result when the API response is successful.
        [Theory]
        [InlineData("{\"id\": \"1\", \"name\": \"Attraction 1\"}", "1", "Attraction 1")] //Inlinedata attribute is used to pass the test data to the test method, and define the expected result
        public async Task GetAttractionByIdAsync_ReturnAttraction_WhenApiResponseIsSuccessful(string responseContext, string expectedId, string expectedName)
        {
            // Arrange: In this section, we are setting up the mock HttpMessageHandler to return a successful response
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContext)
            };

            // Setting up the SendAsyncFunc delegate to return the response
            _mockHttpMessageHandler.SendAsyncFunc = (request, cancellationToken) =>
            {
                return Task.FromResult(response);
            };

            // Act: In this section, we are calling the GetAttractionByIdAsync method of the TicketmasterService
            var result = await _ticketmasterService.GetAttractionByIdAsync("1");

            // Assert: In this section, we are verifying the result of the GetAttractionByIdAsync method
            Assert.NotNull(result); //Checking if the result is not null
            Assert.Equal(expectedId, result.id); //Checking if the id of the result is equal to the expected id
            Assert.Equal(expectedName, result.name); //Checking if the name of the result is equal to the expected name
        }

    }

}
