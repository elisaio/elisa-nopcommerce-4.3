﻿using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.API.ElisaIntegration.Factories;
using Nop.Plugin.API.ElisaIntegration.Services;
using Nop.Services.Orders;

namespace Nop.Plugin.API.ElisaIntegration.Infrastructure
{
    /// <summary>
    /// Represents a plugin dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<ElisaAPIIntegrationModelFactory>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomCartService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}
