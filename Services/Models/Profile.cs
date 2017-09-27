// Copyright (c) Microsoft. All rights reserved.

using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.IoTSolutions.UIConfig.Services.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string ETag { get; set; }
        public JObject DesiredProperties;
    }
}
