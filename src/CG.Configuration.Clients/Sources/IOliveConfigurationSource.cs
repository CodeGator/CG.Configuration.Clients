using CG.Configuration.Clients.Options;
using Microsoft.Extensions.Configuration;
using System;

namespace CG.Configuration.Clients.Sources
{
    public interface IOliveConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// This property contains the options for the source.
        /// </summary>
        OliveConfigurationOptions Options { get; }
    }
}
