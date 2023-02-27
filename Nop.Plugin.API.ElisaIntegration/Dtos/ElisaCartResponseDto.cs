using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Dtos
{
    public class ElisaCartResponseDto
    {
        public ElisaCartResponseDto() 
        {
            Errors = new List<string>();
        }

        [JsonProperty("elisa_cart_id")]
        public Guid ElisaCartId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public IList<string> Errors { get; set; }
    }
}
