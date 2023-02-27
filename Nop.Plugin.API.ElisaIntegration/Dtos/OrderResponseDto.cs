using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Dtos
{
    public class OrderResponseDto
    {
        public OrderResponseDto() 
        {
            OrderItems = new List<Products>();
        }

        [JsonProperty("id")]
        public int OrderId { get; set; }

        [JsonProperty("elisaReference")]
        public Guid ElisaReference { get; set; }

        [JsonProperty("time")]
        public string TimeStemp { get; set; }

        [JsonProperty("totalAmount")]
        public decimal OrderAmount { get; set; }

        [JsonProperty("products")]
        public IList<Products> OrderItems { get; set; }

        public class Products 
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("productId")]
            public int ProductId { get; set; }

            [JsonProperty("attributeId")]
            public int AttributeId { get; set; }

            [JsonProperty("qty")]
            public int Quantity { get; set; }

            [JsonProperty("unitPrice")]
            public decimal Price { get; set; }
        }
    }
}
