using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Core.Http.Extensions;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Services
{
    public partial class CustomShoppingCartService : ShoppingCartService, IShoppingCartService
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        #endregion

        #region Ctor
        public CustomShoppingCartService(
            CatalogSettings catalogSettings,
            IAclService aclService,
            IActionContextAccessor actionContextAccessor,
            ICacheKeyService cacheKeyService,
            ICheckoutAttributeParser checkoutAttributeParser,
            ICheckoutAttributeService checkoutAttributeService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IDateRangeService dateRangeService,
            IDateTimeHelper dateTimeHelper,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeParser productAttributeParser,
            IProductAttributeService productAttributeService,
            IProductService productService,
            IRepository<ShoppingCartItem> sciRepository,
            IShippingService shippingService,
            IStaticCacheManager staticCacheManager,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IUrlHelperFactory urlHelperFactory,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            OrderSettings orderSettings,
            ShoppingCartSettings shoppingCartSettings,
            IHttpContextAccessor httpContextAccessor) : base(
                catalogSettings,
                aclService,
                actionContextAccessor,
                cacheKeyService,
                checkoutAttributeParser,
                checkoutAttributeService,
                currencyService,
                customerService,
                dateRangeService,
                dateTimeHelper,
                eventPublisher,
                genericAttributeService,
                localizationService,
                permissionService,
                priceCalculationService,
                priceFormatter,
                productAttributeParser,
                productAttributeService,
                productService,
                sciRepository,
                shippingService,
                staticCacheManager,
                storeContext,
                storeMappingService,
                urlHelperFactory,
                urlRecordService,
                workContext,
                orderSettings,
                shoppingCartSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _priceCalculationService = priceCalculationService;
            _productAttributeParser = productAttributeParser;
            _shoppingCartSettings = shoppingCartSettings;
        }
        #endregion

        #region Overriden methods
        /// <summary>
        /// Gets the shopping cart unit price (one item)
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customer">Customer</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Product attributes (XML format)</param>
        /// <param name="customerEnteredPrice">Customer entered price (if specified)</param>
        /// <param name="rentalStartDate">Rental start date (null for not rental products)</param>
        /// <param name="rentalEndDate">Rental end date (null for not rental products)</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
        /// <param name="discountAmount">Applied discount amount</param>
        /// <param name="appliedDiscounts">Applied discounts</param>
        /// <returns>Shopping cart unit price (one item)</returns>
        public override decimal GetUnitPrice(Product product,
            Customer customer,
            ShoppingCartType shoppingCartType,
            int quantity,
            string attributesXml,
            decimal customerEnteredPrice,
            DateTime? rentalStartDate, DateTime? rentalEndDate,
            bool includeDiscounts,
            out decimal discountAmount,
            out List<Discount> appliedDiscounts)
        {
            //Custom code by Ajay Chauhan on 17-03-2022
            //Call base class method in default behaviour and call custom logic based on elisa session existance
            //store unique elisa cart Id in session
            var elisaCsrtId = _httpContextAccessor.HttpContext.Session.Get<Guid>(ElisaPluginDefaults.ElisaCartId);
            bool isSessionExists = false;

            if (elisaCsrtId == null || elisaCsrtId == Guid.Empty) 
            {
                
                return base.GetUnitPrice(product, customer, shoppingCartType, quantity, attributesXml, customerEnteredPrice, rentalStartDate, rentalEndDate, includeDiscounts, out discountAmount, out appliedDiscounts);
            }
            else
                isSessionExists = true;

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            discountAmount = decimal.Zero;
            appliedDiscounts = new List<Discount>();

            decimal finalPrice;

            var combination = _productAttributeParser.FindProductAttributeCombination(product, attributesXml);
            if (combination?.OverriddenPrice.HasValue ?? false)
            {
                finalPrice = _priceCalculationService.GetFinalPrice(product,
                        customer,
                        combination.OverriddenPrice.Value,
                        decimal.Zero,
                        includeDiscounts,
                        quantity,
                        product.IsRental ? rentalStartDate : null,
                        product.IsRental ? rentalEndDate : null,
                        out discountAmount, out appliedDiscounts);
            }
            else
            {
                //summarize price of all attributes
                var attributesTotalPrice = decimal.Zero;
                var attributeValues = _productAttributeParser.ParseProductAttributeValues(attributesXml);
                if (attributeValues != null)
                {
                    foreach (var attributeValue in attributeValues)
                    {
                        attributesTotalPrice += _priceCalculationService.GetProductAttributeValuePriceAdjustment(product, attributeValue, customer, product.CustomerEntersPrice ? (decimal?)customerEnteredPrice : null);
                    }
                }

                //Custom code by Ajay Chauhan on 17-03-2022
                //Call base class method in default behaviour and call custom logic based on elisa session existance
                //get price of a product (with previously calculated price of all attributes)
                if (product.CustomerEntersPrice || (isSessionExists && customerEnteredPrice > 0))
                {
                    finalPrice = customerEnteredPrice;
                }
                else
                {
                    int qty;
                    if (_shoppingCartSettings.GroupTierPricesForDistinctShoppingCartItems)
                    {
                        //the same products with distinct product attributes could be stored as distinct "ShoppingCartItem" records
                        //so let's find how many of the current products are in the cart                        
                        qty = GetShoppingCart(customer, shoppingCartType: shoppingCartType, productId: product.Id)
                            .Sum(x => x.Quantity);

                        if (qty == 0)
                        {
                            qty = quantity;
                        }
                    }
                    else
                    {
                        qty = quantity;
                    }

                    finalPrice = _priceCalculationService.GetFinalPrice(product,
                        customer,
                        attributesTotalPrice,
                        includeDiscounts,
                        qty,
                        product.IsRental ? rentalStartDate : null,
                        product.IsRental ? rentalEndDate : null,
                        out discountAmount, out appliedDiscounts);
                }
            }

            //rounding
            if (_shoppingCartSettings.RoundPricesDuringCalculation)
                finalPrice = _priceCalculationService.RoundPrice(finalPrice);

            return finalPrice;
        }
        #endregion
    }
}
