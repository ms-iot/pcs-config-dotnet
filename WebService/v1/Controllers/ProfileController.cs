// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.IoTSolutions.UIConfig.Services;
using Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Filters;
using Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Models;

namespace Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Controllers
{
    [Route(Version.PATH + "/profiles"), TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class ProfileController : Controller
    {
        private readonly IStorage storage;

        public ProfileController(IStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        public async Task<ProfileListApiModel> GetAllAsync()
        {
            return new ProfileListApiModel(await this.storage.GetAllProfilesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ProfileApiModel> GetAsync(string id)
        {
            return new ProfileApiModel(await this.storage.GetProfileAsync(id));
        }

        [HttpPost]
        public async Task<ProfileApiModel> CreateAsync([FromBody] ProfileApiModel input)
        {
            return new ProfileApiModel(await this.storage.CreateProfileAsync(input.ToServiceModel()));
        }

        [HttpPut("{id}")]
        public async Task<ProfileApiModel> UpdateAsync(string id, [FromBody] ProfileApiModel input)
        {
            return new ProfileApiModel(await this.storage.UpdateProfileAsync(id, input.ToServiceModel(), input.ETag));
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await this.storage.DeleteProfileAsync(id);
        }
    }
}
