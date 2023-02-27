using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Models
{
    public partial class ConfigurationModel
    {
        [NopResourceDisplayName("Plugin.API.ElisaIntegration.Configuration.Token")]
        public string Token { get; set; }
    }
}
