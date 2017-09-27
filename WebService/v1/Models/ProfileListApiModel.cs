// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.IoTSolutions.UIConfig.Services.Models;
using Newtonsoft.Json;

namespace Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Models
{
    public class ProfileListApiModel
    {
        public IEnumerable<ProfileApiModel> Items { get; set; }

        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        public ProfileListApiModel(IEnumerable<Profile> models)
        {
            this.Items = models.Select(m => new ProfileApiModel(m));

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"ProfileList;{Version.Number}" },
                { "$url", $"/{Version.Path}/Profiles" }
            };
        }
    }
}
