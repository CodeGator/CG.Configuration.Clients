using CG.DataAnnotations;
using CG.Configuration.Clients.Options;
using CG.Configuration.Clients.Sources;
using CG.Validations;
using System;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IConfigurationBuilder"/>
    /// type, for registering types related to this extension.
    /// </summary>
    public static partial class ConfigurationBuilderExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method adds an CG.Olive configuration source to the specified 
        /// configuration builder.
        /// </summary>
        /// <param name="builder">The builder to use for the operation.</param>
        /// <param name="optionsDelegate">The options delegate to use for the operation.</param>
        /// <returns>The value of the <paramref name="builder"/> parameter.</returns>
        public static IConfigurationBuilder AddOlive(
            this IConfigurationBuilder builder,
            Action<OliveConfigurationOptions> optionsDelegate
            )
        {
            // Validate the arguments before attempting to use them.
            Guard.Instance().ThrowIfNull(builder, nameof(builder))
                .ThrowIfNull(optionsDelegate, nameof(optionsDelegate));

            // Create the options.
            var options = new OliveConfigurationOptions();

            // Call the delegate.
            optionsDelegate(options);

            // Validate the results.
            options.ThrowIfInvalid();

            // Create an Olive source.
            var source = new OliveConfigurationSource(
                options
                );
                        
            // Add the source to the builder.
            builder.Sources.Add(source);

            // Return the builder.
            return builder;
        }

        #endregion
    }
}
