using Nop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Domain
{
    /// <summary>
    /// Represents a custom cart created by elisa
    /// </summary>
    public class CustomCart : BaseEntity
    {
        public CustomCart() 
        {
            ElisaCartId = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the elisa cart id system generated
        /// </summary>
        public Guid ElisaCartId { get; set; }

        /// <summary>
        /// Gets or sets the elisa customer reference
        /// </summary>
        public string ElisaReference { get; set; }

    }
}
