using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Plugin.API.ElisaIntegration.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Data
{
    [SkipMigrationOnUpdate]
    [NopMigration("2020/02/05 14:44:17:6455442", "API.ElisaIntegration base schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        #region Fields

        protected IMigrationManager _migrationManager;

        #endregion

        #region Ctor

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            _migrationManager.BuildTable<CustomCart>(Create);
            _migrationManager.BuildTable<CustomCartItems>(Create);
        }

        #endregion
    }
}
