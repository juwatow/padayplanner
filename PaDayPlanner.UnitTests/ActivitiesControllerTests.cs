using System;
using Xunit;
using Moq;
using PaDayPlanner.Api.Repositories;
using PaDayPlanner.Api.Controllers;
using System.Threading.Tasks;
using PaDayPlanner.Api.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PaDayPlanner.Api.Dtos;
using FluentAssertions;
using System.Collections.Generic;

namespace PaDayPlanner.UnitTests;

public class ActivitiesControllerTests
{
    private readonly Mock<IActivitiesRepository> repositoryStub = new();
    private readonly Mock<ILogger<ActivitiesController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    public async Task GetActivityAsync_WithUnexistingActivity_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetActivityAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Activity)null);

        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetActivityAsync(Guid.NewGuid());

        // Assert
        // Assert.IsType<NotFoundResult>(result.Result);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetActivityAsync_WithExistingActivity_ReturnsExpectedActivity()
    {
        // Arrange
        var expectedActivity = CreateRandomActivity();
        repositoryStub.Setup(repo => repo.GetActivityAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedActivity);

        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetActivityAsync(Guid.NewGuid());

        // Assert
        result.Value.Should().BeEquivalentTo(expectedActivity);
    }

    [Fact]
    public async Task GetActivitiesAsync_WithExistingActivities_ReturnsAllActivities()
    {
        // Arrange
        var expectedActivities = new[] { CreateRandomActivity(), CreateRandomActivity(), CreateRandomActivity() };
        repositoryStub.Setup(repo => repo.GetActivitiesAsync())
            .ReturnsAsync(expectedActivities);
        
        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        var actualActivities = await controller.GetActivitiesAsync();

        // Assert
        actualActivities.Should().BeEquivalentTo(expectedActivities);

    }

    [Fact]
    public async Task GetActivitiesAsync_WithNameToMatch_ReturnsMatchingActivities()
    {
        // Arrange
        var allActivities = new[]
        { 
            new Activity() { Name = "MarchBreak with the Raptors" }, 
            new Activity() { Name = "MarchBreak at Art Gallery of Ontario" }, 
            new Activity() { Name = "Swimming" } 
        };

        var nameToMatch = "March";

        repositoryStub.Setup(repo => repo.GetActivitiesAsync())
            .ReturnsAsync(allActivities);
        
        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<ActivityDto> foundActivities = await controller.GetActivitiesAsync(nameToMatch);

        // Assert
        foundActivities.Should().OnlyContain(
            item => item.Name == allActivities[0].Name || item.Name == allActivities[1].Name
        );

    }

    [Fact]
    public async Task CreateActivityAsync_WithActivityToCreate_ReturnsCreatedActivity()
    {
        // Arrange
        var activityToCeate = new CreateActivityDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            rand.Next(1000)
        );
    
        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.CreateActivityAsync(activityToCeate);

        // Assert
        var createdActivity = (result.Result as CreatedAtActionResult).Value as ActivityDto;
        activityToCeate.Should().BeEquivalentTo(
            createdActivity,
            options => options.ComparingByMembers<ActivityDto>().ExcludingMissingMembers());
        createdActivity.Id.Should().NotBeEmpty();

    }

    [Fact]
    public async Task UpdateActivityAsync_WithUnexistingActivity_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetActivityAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Activity)null);

        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

         var activityToUpdate = new UpdateActivityDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            rand.Next(1000)
         );

        // Act
        var result = await controller.UpdateActivityAsync(Guid.NewGuid(), activityToUpdate);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateActivityAsync_WithExistingActivity_ReturnsNoContent()
    {
        // Arrange
        var existingActivity = CreateRandomActivity();
        repositoryStub.Setup(repo => repo.GetActivityAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingActivity);

        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        var activityId = existingActivity.Id;
        var activityToUpdate = new UpdateActivityDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            existingActivity.Price + 10
        );

        // Act
        var result = await controller.UpdateActivityAsync(activityId, activityToUpdate);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteActivityAsync_WithUnexistingActivity_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetActivityAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Activity)null);

        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.DeleteActivityAsync(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteActivityAsync_WithExistingActivity_ReturnsNoContent()
    {
        // Arrange
        var existingActivity = CreateRandomActivity();
        repositoryStub.Setup(repo => repo.GetActivityAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingActivity);

        var controller = new ActivitiesController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.DeleteActivityAsync(existingActivity.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private Activity CreateRandomActivity()
    {
        return new() {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            BusinessOwner = Guid.NewGuid().ToString(),
            Price = rand.Next(1000)
        };
    }
}