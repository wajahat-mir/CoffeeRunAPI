using CoffeeRunAPI.Controllers;
using CoffeeRunAPI.Models;
using CoffeeRunAPI.Repositories;
using CoffeeRunAPI.ViewModels;
using CoffeRunAPITests.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoffeRunAPITests
{
    public class RunControllerUnitTests
    {
        public static List<Run> GetRuns()
        {
            var runs = new List<Run>();
            runs.Add(new Run()
            {
                RunId = 1,
                Name = "MondayRun",
                TimeToRun = DateTime.Now,
                Runner = "",
                runStatus = RunStatus.Prepped,
                OwnerUserId = "1234567890"
            });

            runs.Add(new Run()
            {
                RunId = 2,
                Name = "TuesdayRun",
                TimeToRun = DateTime.Now.AddDays(-10),
                Runner = "CEO",
                runStatus = RunStatus.Complete,
                OwnerUserId = "0987654321"
            });
            return runs;
        }

        [Fact]
        public void Get_ReturnsOkResult_WithAllRuns()
        {
            var mapper = MapperService.DefaultMapper();

            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetAllRuns())
                .ReturnsAsync(GetRuns());

            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(result.Result.Result);
            Assert.Equal(200, okResult.StatusCode);
            var items = Assert.IsAssignableFrom<IEnumerable<RunViewModel>>(okResult.Value);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public void Get_ReturnsNotFoundResult_NoRuns()
        {
            var mapper = MapperService.DefaultMapper();

            IEnumerable<Run> runs = null;
            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetAllRuns())
                .ReturnsAsync(runs);

            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Get();
            var NotFoundResult = Assert.IsType<NotFoundResult>(result.Result.Result);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }

        [Fact]
        public void GetById_ReturnOkResult_WithRun()
        {
            var mapper = MapperService.DefaultMapper();

            var run = GetRuns()[0];
            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetRun(It.IsAny<int>()))
                .ReturnsAsync(run);

            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Get(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result.Result);
            Assert.Equal(200, okResult.StatusCode);
            var item = Assert.IsAssignableFrom<RunViewModel>(okResult.Value);
            Assert.Equal(1, item.RunId);
        }

        [Fact]
        public void GetById_ReturnNotFoundResult_NoRun()
        {
            var mapper = MapperService.DefaultMapper();

            Run run = null;
            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetRun(It.IsAny<int>()))
                .ReturnsAsync(run);

            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Get(1);
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void Patch_ReturnNotFoundResult_NoRun()
        {
            var mapper = MapperService.DefaultMapper();

            Run run = null;
            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetRun(It.IsAny<int>()))
                .ReturnsAsync(run);

            JsonPatchDocument<RunViewModel> patchRequest = new JsonPatchDocument<RunViewModel>();
            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Patch(1, patchRequest);
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void Patch_ReturnBadResult_CannotUpdateRun()
        {
            var mapper = MapperService.DefaultMapper();

            Run run = GetRuns()[0];
            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetRun(It.IsAny<int>()))
                .ReturnsAsync(run);
            mockRepo.Setup(repo => repo.UpdateRun(It.IsAny<Run>()))
                .ReturnsAsync(false);

            JsonPatchDocument<RunViewModel> patchRequest = new JsonPatchDocument<RunViewModel>();
            patchRequest.Replace(r => r.RunId, 1);
            patchRequest.Replace(r => r.runStatus, RunStatus.OnTheRun);

            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Patch(1, patchRequest);
            var BadResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(400, BadResult.StatusCode);
        }

        [Fact]
        public void Patch_ReturnOkResult_UpdateRun()
        {
            var mapper = MapperService.DefaultMapper();

            Run run = GetRuns()[0];
            var mockRepo = new Mock<IRunRepository>();
            mockRepo.Setup(repo => repo.GetRun(It.IsAny<int>()))
                .ReturnsAsync(run);
            mockRepo.Setup(repo => repo.UpdateRun(It.IsAny<Run>()))
                .ReturnsAsync(true);

            JsonPatchDocument<RunViewModel> patchRequest = new JsonPatchDocument<RunViewModel>();
            patchRequest.Replace(r => r.RunId, 1);
            patchRequest.Replace(r => r.runStatus, RunStatus.OnTheRun);

            var controller = new RunController(mockRepo.Object, mapper);
            var result = controller.Patch(1, patchRequest);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            var item = Assert.IsAssignableFrom<RunViewModel>(okResult.Value);
            Assert.Equal(1, item.RunId);
            Assert.Equal(RunStatus.OnTheRun, item.runStatus);
        }
    }
}
