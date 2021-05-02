using CG.Configuration.Clients.Options;
using CG.Configuration.Clients.Providers;
using CG.Validations;
using Microsoft.Extensions.Configuration;
using System;

namespace CG.Configuration.Clients.Sources
{
    internal class OliveConfigurationSource : IOliveConfigurationSource
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the options for the source.
        /// </summary>
        public OliveConfigurationOptions Options { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="OliveConfigurationSource"/>
        /// class.
        /// </summary>
        /// <param name="options">The options to use for the source.</param>
        public OliveConfigurationSource(
            OliveConfigurationOptions options
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(options, nameof(options));

            // Save the references.
            Options = options;
        }

        #endregion
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method is called by the builder to create a provider.
        /// </summary>
        /// <param name="builder">The builder to use for the operation.</param>
        /// <returns>An <see cref="IConfigurationProvider"/> object.</returns>
        public IConfigurationProvider Build(
            IConfigurationBuilder builder
            )
        {
            // Validate the arguments before attempting to use them.
            Guard.Instance().ThrowIfNull(builder, nameof(builder));

            // Create the provider.
            return new OliveConfigurationProvider(this);
        }

        #endregion
    }
}
