using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using System.Collections.Generic;

namespace Nop.Plugin.API.ElisaIntegration
{
    /// <summary>
    /// Elisa API Integration plugin
    /// </summary>
    public class ElisaPluginProcessor : BasePlugin
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor
        public ElisaPluginProcessor(ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper) 
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/ElisaConfiguration/Configure";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //locales
            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugin.API.ElisaIntegration.Configuration.Token"] = "Token",
                ["Plugin.API.ElisaIntegration.Configuration.Token.Hint"] = "Token will generate automatically by cliking on generate token button",
                ["Plugin.API.ElisaIntegration.Configuration.GeneratePassword"] = "Generate token",
                ["Plugin.API.ElisaIntegration.Configuration.GenerateAPIToken"] = "Generate API Token"
            });

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            var setting = _settingService.GetSetting(ElisaPluginDefaults.Token);
            _settingService.DeleteSetting(setting);

            //locales
            _localizationService.DeletePluginLocaleResources("Plugin.API.ElisaIntegration");

            base.Uninstall();
        }
        #endregion
    }
}
