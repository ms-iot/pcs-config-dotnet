// Copyright (c) Microsoft. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.IoTSolutions.UIConfig.Services;
using Microsoft.Azure.IoTSolutions.UIConfig.Services.Models;
using Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Controllers;
using Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Models;
using Moq;
using Xunit;
using Newtonsoft.Json.Linq;

namespace WebService.Test.Controllers
{
    public class ProfileControllerTest
    {
        private readonly Mock<IStorage> mockStorage;
        private readonly ProfileController controller;

        public ProfileControllerTest()
        {
            this.mockStorage = new Mock<IStorage>();
            this.controller = new ProfileController(this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var models = new[]
            {
                new Profile
                {
                    Id = "id1",
                    DisplayName = "Profile1",
                    DesiredProperties = new JObject{
                        { "key1", "value1" }
                    },
                    ETag = "etag"
                },
                new Profile
                {
                    Id = "id2",
                    DisplayName = "Profile2",
                    DesiredProperties = new JObject{
                        { "key2", "value2" }
                    },
                    ETag = "etag"
                },
                new Profile
                {
                    Id = "id3",
                    DisplayName = "Profile3",
                    DesiredProperties = new JObject{
                        { "key3", "value3" }
                    },
                    ETag = "etag"
                },
            };

            this.mockStorage
                .Setup(x => x.GetAllProfilesAsync())
                .ReturnsAsync(models);

            var result = await this.controller.GetAllAsync();

            this.mockStorage
                .Verify(x => x.GetAllProfilesAsync(), Times.Once);

            Assert.Equal(result.Items.Count(), models.Length);
            foreach (var item in result.Items)
            {
                var model = models.Single(g => g.Id == item.Id);
                Assert.Equal(model.DisplayName, item.DisplayName);
                Assert.Equal(model.DesiredProperties, item.DesiredProperties);
                Assert.Equal(model.ETag, item.ETag);
            }
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            var profileId = "id";
            var displayName = "Profile";
            var desiredProperties = new JObject()
            {
                { "key", "value" }
            };
            var etag = "etag";

            this.mockStorage
                .Setup(x => x.GetProfileAsync(It.IsAny<string>()))
                .ReturnsAsync(new Profile
                {
                    Id = profileId,
                    DisplayName = displayName,
                    DesiredProperties = desiredProperties,
                    ETag = etag
                });

            var result = await this.controller.GetAsync(profileId);

            this.mockStorage
                .Verify(x => x.GetProfileAsync(
                    It.Is<string>(s => s == profileId)),
                    Times.Once);

            Assert.Equal(result.DisplayName, displayName);
            Assert.Equal(result.DesiredProperties, desiredProperties);
            Assert.Equal(result.ETag, etag);
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            var profileId = "id";
            var displayName = "Profile";
            var desiredProperties = new JObject()
            {
                { "key", "value" }
            };
            var etag = "etag";

            this.mockStorage
                .Setup(x => x.CreateProfileAsync(It.IsAny<Profile>()))
                .ReturnsAsync(new Profile
                {
                    Id = profileId,
                    DisplayName = displayName,
                    DesiredProperties = desiredProperties,
                    ETag = etag
                });

            var result = await this.controller.CreateAsync(new ProfileApiModel
            {
                DisplayName = displayName,
                DesiredProperties = desiredProperties
            });

            this.mockStorage
                .Verify(x => x.CreateProfileAsync(
                    It.Is<Profile>(m => m.DisplayName == displayName && m.DesiredProperties.GetValue("key").ToString() == "value")),
                    Times.Once);

            Assert.Equal(result.Id, profileId);
            Assert.Equal(result.DisplayName, displayName);
            Assert.Equal(result.DesiredProperties, desiredProperties);
            Assert.Equal(result.ETag, etag);
        }

        [Fact]
        public async Task UpdateAsyncTest()
        {
            var profileId = "id";
            var displayName = "Profile";
            var desiredProperties = new JObject()
            {
                { "key", "value" }
            };
            var etagOld = "oldTag";
            var etagNew = "newTag";

            this.mockStorage
                .Setup(x => x.UpdateProfileAsync(It.IsAny<string>(), It.IsAny<Profile>(), It.IsAny<string>()))
                .ReturnsAsync(new Profile
                {
                    Id = profileId,
                    DisplayName = displayName,
                    DesiredProperties = desiredProperties,
                    ETag = etagNew
                });

            var result = await this.controller.UpdateAsync(profileId,
                new ProfileApiModel
                {
                    DisplayName = displayName,
                    DesiredProperties = desiredProperties,
                    ETag = etagOld
                });

            this.mockStorage
                .Verify(x => x.UpdateProfileAsync(
                    It.Is<string>(s => s == profileId),
                    It.Is<Profile>(m => m.DisplayName == displayName && m.DesiredProperties.GetValue("key").ToString() == "value"),
                    It.Is<string>(s => s == etagOld)),
                    Times.Once);

            Assert.Equal(result.Id, profileId);
            Assert.Equal(result.DisplayName, displayName);
            Assert.Equal(result.DesiredProperties, desiredProperties);
            Assert.Equal(result.ETag, etagNew);
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var profileId = "id";

            this.mockStorage
                .Setup(x => x.DeleteProfileAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(0));

            await this.controller.DeleteAsync(profileId);

            this.mockStorage
                .Verify(x => x.DeleteProfileAsync(
                    It.Is<string>(s => s == profileId)),
                    Times.Once);
        }
    }
}
