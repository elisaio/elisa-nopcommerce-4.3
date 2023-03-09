using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.API.ElisaIntegration.Dtos;
using Nop.Plugin.API.ElisaIntegration.Factories;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Web.Framework.Controllers;
using System;

namespace Nop.Plugin.API.ElisaIntegration.Controllers
{
    public class ElisaAPIIntegrartionContorller : BasePluginController
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ElisaAPIIntegrationModelFactory _elisaAPIIntegrationModelFactory;
        private readonly IPluginService _pluginService;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public ElisaAPIIntegrartionContorller(IHttpContextAccessor httpContextAccessor,
            ElisaAPIIntegrationModelFactory elisaAPIIntegrationModelFactory,
            IPluginService pluginService,
            ISettingService settingService)
        {
            _httpContextAccessor = httpContextAccessor;
            _elisaAPIIntegrationModelFactory = elisaAPIIntegrationModelFactory;
            _pluginService = pluginService;
            _settingService = settingService;
        }
        #endregion

        #region Utilities
        protected bool ValidateToken() 
        {
            bool isValid = false;
            //load settings for a chosen store scope
            var token = _settingService.GetSettingByKey<string>(ElisaPluginDefaults.Token);

            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization")) 
            {
                token = "Bearer " + token;
                string requestToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Contains("Authorization") ? _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Authorization ", String.Empty) : _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                if (string.Equals(requestToken, token))
                    isValid = true;
            }
            return isValid;
        }
        #endregion

        #region Methods

        [HttpGet]
        [Route("/elisa/version")]
        public IActionResult GetPluginVersion()
        {
            APIResponseDto response = new APIResponseDto();

            var isValid = ValidateToken();
            if (isValid)
            {
                var pluginDescriptor = _pluginService.GetPluginDescriptorBySystemName<IPlugin>("API.ElisaIntegration", LoadPluginsMode.InstalledOnly);
                if (pluginDescriptor == null)
                {
                    response.IsSuccess = false;
                    response.Errors.Add(new APIResponseDto.Error { Message = "Plugin not found...!" });

                    
                }

                response.IsSuccess = true;
                response.Data = pluginDescriptor.Version;

                return Ok(response);
            }

            response.IsSuccess = false;
            response.Errors.Add(new APIResponseDto.Error { Message = "Unauthorized, Invalid token...!" });

            return BadRequest(response);
        }

        [HttpPost]
        [Route("/elisa/products/{timestamp}/{page}")]
        public IActionResult GetProducts(string timestamp, int page)
        {
            APIResponseDto response = new APIResponseDto();

            var isValid = ValidateToken();
            if (isValid)
            {
                var result = _elisaAPIIntegrationModelFactory.PrepareProdutsJsonSerilization(timestamp, page);

                if (string.IsNullOrEmpty(result))
                    return BadRequest();

                return Ok(result);
            }
            response.IsSuccess = false;
            response.Errors.Add(new APIResponseDto.Error { Message = "Unauthorized, Invalid token...!" });

            return BadRequest(response);
        }

        [HttpPost]
        [Route("/elisa/cart_create")]
        public IActionResult CreateShoppingCart([FromBody] ElisaCartDto cartItems) 
        {
            APIResponseDto response = new APIResponseDto();

            var isValid = ValidateToken();
            if (isValid)
            {
                var jsonResponse = _elisaAPIIntegrationModelFactory.PrepareElisaCustomCart(cartItems);

                if (jsonResponse == null)
                    return BadRequest();

                return Ok(jsonResponse);
            }
            response.IsSuccess = false;
            response.Errors.Add(new APIResponseDto.Error { Message = "Unauthorized, Invalid token...!" });

            return BadRequest(response);
        }

        [HttpPost]
        [Route("/elisa/orders/{timestamp}")]
        public IActionResult LoadOrders(string timestamp)
        {
            APIResponseDto response = new APIResponseDto();

            var isValid = ValidateToken();
            if (isValid)
            {
                var result = _elisaAPIIntegrationModelFactory.PreapreElisaOrders(timestamp);

                if (string.IsNullOrEmpty(result))
                    return BadRequest();

                return Ok(result);
            }
            response.IsSuccess = false;
            response.Errors.Add(new APIResponseDto.Error { Message = "Unauthorized, Invalid token...!" });

            return BadRequest(response);
        }
        #endregion
    }
}
