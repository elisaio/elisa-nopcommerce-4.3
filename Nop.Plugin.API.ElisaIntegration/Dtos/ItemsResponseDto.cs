using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nop.Plugin.API.ElisaIntegration.Dtos
{
    public class ItemsResponseDto 
    {
        public ItemsResponseDto() 
        {
            Products = new List<CustomProductModel>();
        }
        [JsonProperty("items")]
        public IList<CustomProductModel> Products { get; set; }

        #region Nested classes

        public class CustomProductModel
        {
            public CustomProductModel()
            {
                OtherImage = new List<string>();
                AvailableProductAttributes = new List<ProductAttributesModel>();
                AssociatedProductAttributes = new List<AssociatedProductAttributesModel>();
            }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("parent_id")]
            public int ParentId { get; set; }

            [JsonProperty("attr_mapping_id")]
            public int AttributeCombinationId { get; set; }

            [JsonProperty("sku")]
            public string Sku { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("price")]
            public decimal Price { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("url")]
            public string ProductDetailUrl { get; set; }

            [JsonProperty("type")]
            public string ProductType { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("main_image")]
            public string MainImage { get; set; }

            [JsonProperty("other_image")]
            public IList<string> OtherImage { get; set; }

            [JsonProperty("inventory")]
            public int Inventory { get; set; }

            [JsonProperty("allowoutofstock")]
            public bool AllowOutOfStock { get; set; }

            [JsonProperty("manage_stock")]
            public bool ManageStock { get; set; }

            [JsonProperty("configurable_options")]
            public IList<ProductAttributesModel> AvailableProductAttributes { get; set; }

            [JsonProperty("options")]
            public IList<AssociatedProductAttributesModel> AssociatedProductAttributes { get; set; }

            [JsonProperty("attributexml")]
            public string AttributeXML { get; set; }

            #region Nested class

            public class ProductAttributesModel
            {
                public ProductAttributesModel()
                {
                    AttributeValues = new List<string>();
                }

                [JsonProperty("id")]
                public int AttributeId { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("position")]
                public int Position { get; set; }

                [JsonProperty("values")]
                public IList<string> AttributeValues { get; set; }
            }

            public class AssociatedProductAttributesModel
            {
                [JsonProperty("id")]
                public int ProductAttibuteId { get; set; }

                [JsonProperty("value")]
                public string ProductAttributeValue { get; set; }
            }
            #endregion
        }

        #endregion

        [JsonProperty("total")]
        public int ProductCount { get; set; }
    }
}
