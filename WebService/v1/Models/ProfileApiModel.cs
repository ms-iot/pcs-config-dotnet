// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using Microsoft.Azure.IoTSolutions.UIConfig.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Models
{
    public class ProfileApiModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("DesiredProperties")]
        public JObject DesiredProperties { get; set; }

        [JsonProperty("ETag")]
        public string ETag { get; set; }

        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        public ProfileApiModel()
        {
        }

        public ProfileApiModel(Profile model)
        {
            this.Id = model.Id;
            this.DisplayName = model.DisplayName;
            this.DesiredProperties = model.DesiredProperties;
            this.ETag = model.ETag;

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"DeviceGroup;{Version.Number}" },
                { "$url", $"/{Version.Path}/devicegroups/{model.Id}" }
            };
        }

        public Profile ToServiceModel()
        {
            return new Profile
            {
                DisplayName = this.DisplayName,
                DesiredProperties = this.DesiredProperties
            };
        }
    }
}
