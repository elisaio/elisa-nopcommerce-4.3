using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Services
{
    /// <summary>
    /// Represents plugin event consumer
    /// </summary>
    public class EventConsumer : IConsumer<EntityInsertedEvent<Order>>
    {
        #region Fields
        private readonly CustomCartService _customCartService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IStoreContext _storeContext;
        #endregion

        #region Ctor
        public EventConsumer(CustomCartService customCartService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IStoreContext storeContext) 
        {
            _customCartService = customCartService;
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _storeContext = storeContext;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handle order placed event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(EntityInsertedEvent<Order> eventMessage) 
        {
            _logger.Error("Order placed event called...!");
            if (eventMessage.Entity == null)
                return;

            var order = eventMessage.Entity;
            var elisaCartId = _httpContextAccessor.HttpContext.Session.Get<Guid>(ElisaPluginDefaults.ElisaCartId);
            if (elisaCartId != Guid.Empty)
            {
                var customCart = _customCartService.GetCustomCartByElisaCartId(elisaCartId);
                if (customCart != null)
                {
                    _genericAttributeService.SaveAttribute<Guid>(order, ElisaPluginDefaults.ElisaReference, customCart.ElisaCartId, _storeContext.CurrentStore.Id);

                    _customCartService.DeleteCustomCart(customCart);
                }
            }
        }
        #endregion
    }
}
