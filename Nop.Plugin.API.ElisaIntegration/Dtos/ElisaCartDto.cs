using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Dtos
{
    public class ElisaCartDto
    {
        public ElisaCartDto() 
        {
            Items = new List<CartItems>();
        }

        //[JsonProperty("elisa_reference")]
        public string ElisaReference { get; set; }

        //[JsonProperty("products")]
        public IList<CartItems> Items { get; set; }

        #region Nested class
        public class CartItems 
        {
            //[JsonProperty("id")]
            public int Id { get; set; }

            //[JsonProperty("parentid")]
            public int ParentId { get; set; }

            //[JsonProperty("qty")]
            public int Quantity { get; set; }
            
            //[JsonProperty("price")]
            public decimal Price { get; set; }

            public string AttributeXML { get; set; }
        }
        #endregion
    }
}
