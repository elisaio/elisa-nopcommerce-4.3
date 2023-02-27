using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.API.ElisaIntegration.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.API.ElisaIntegration.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class ElisaConfigurationController : BasePluginController
    {
        #region Fields
        private readonly IEncryptionService _encryptionService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor
        public ElisaConfigurationController(IEncryptionService encryptionService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _encryptionService = encryptionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var token = _settingService.GetSettingByKey<string>(ElisaPluginDefaults.Token);

            var model = new ConfigurationModel()
            {
                Token = token
            };

            return View("~/Plugins/API.ElisaIntegration/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (string.IsNullOrEmpty(model.Token))
            {
                model.Token = _encryptionService.EncryptText(CommonHelper.GenerateRandomDigitCode(10));
            }

            //load settings for a chosen store scope
            _settingService.SetSetting<string>(ElisaPluginDefaults.Token, model.Token);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
        #endregion
    }
}
