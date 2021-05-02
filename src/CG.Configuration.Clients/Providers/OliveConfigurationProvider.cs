using CG.Configuration.Clients.Models;
using CG.Configuration.Clients.Sources;
using CG.Validations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace CG.Configuration.Clients.Providers
{
    /// <summary>
    /// This class represents a CG.Olive configuration provider.
    /// </summary>
    internal class OliveConfigurationProvider : ConfigurationProvider
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a reference to the provider source.
        /// </summary>
        protected IOliveConfigurationSource Source { get; }

        /// <summary>
        /// This property contains a reference to a signalR hub.
        /// </summary>
        protected HubConnection Hub { get; set; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="OliveConfigurationProvider"/>
        /// class.
        /// </summary>
        /// <param name="source">The configuration source to use for the provider.</param>
        public OliveConfigurationProvider(
            IOliveConfigurationSource source
            )
        {
            // Validate the arguments before attempting to use them.
            Guard.Instance().ThrowIfNull(source, nameof(source));

            // Save the reference.
            Source = source;

            // Initialize the provider.
            Initialize();
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method loads (or reloads) the data for the provider.
        /// </summary>
        public override void Load()
        {
            // Create the HTTP client.
            using var client = new HttpClient();

            // Set the base address.
            client.BaseAddress = new Uri(
                Source.Options.Url
                );

            // Create the REST endpoint.
            var url = $"api/configuration";

            // Create json for the body.
            var json = "{" +
                    "\"Sid\": \"" + Source.Options.Sid + "\"," +
                    "\"SKey\": \"" + Source.Options.SKey + "\"," +
                    "\"Environment\": \"" + Source.Options.Environment + "\"" +
                    "}";

            // Make the body of the REST call.
            var body = new StringContent(
                json,
                Encoding.UTF8,
                MediaTypeNames.Application.Json
                );

            // Make the REST call.
            var response = client.PostAsync(
                url,
                body
                ).Result;

            // Ensure we succeeded.
            response.EnsureSuccessStatusCode();

            // Read the content.
            json = response.Content.ReadAsStringAsync().Result;

            // Convert the json to an object array.
            var items = JsonConvert.DeserializeObject<KeyValuePair<string, string>[]>(
                json
                );

            // Clear any cached data.
            Data.Clear();

            // Loop through the values.
            foreach (var kvp in items)
            {
                // Add each key-value-pair to the cache.
                Data.Add(kvp);
            }

            // Give the base class a chance.
            base.Load();
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method is called to initialize the SignalR back-channel.
        /// </summary>
        private void Initialize()
        {
            // Should we setup a back-channel?
            if (Source.Options.ReloadOnChange)
            {
                // Format the address.
                var address = $"{Source.Options.Url}";
                
                // Create a signalR hub builder.
                var builder = new HubConnectionBuilder()
                    .WithUrl($"{address}/_backchannel")
                    .WithAutomaticReconnect();

                // Create the signalR hub.
                Hub = builder.Build();

                // Wire up a back-channel handler.
                Hub.On(
                    "ChangeSetting", 
                    (string json) => OnNotification(json)
                    );

                // Start the signalR hub.
                Hub.StartAsync().Wait();
            }

            // Load the provider's data.
            Load();

            // Reset any associated change tokens.
            base.OnReload();
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to process change notifications from the 
        /// SignalR back-channel.
        /// </summary>
        /// <param name="json">The JSON body of the notification.</param>
        private void OnNotification(
            string json
            )
        {
            // Convert the JSON to a model.
            var model = JsonConvert.DeserializeObject<ChangeNotification>(
                json
                );

            // Did we succeed?
            if (null != model)
            {
                // Should we filter by application?
                if (Source.Options.FilterChangesByApplication)
                {
                    // Do the applications not match?
                    if (Source.Options.Sid != model.Sid)
                    {
                        return; // Nothing to do.
                    }
                }

                // Should we filter by keys?
                if (Source.Options.FilterChangesByKeys.Any())
                {
                    // Does the key match not match?
                    if (false == Source.Options.FilterChangesByKeys
                            .Any(x => x.StartsWith(model.Key))
                            )
                    {
                        return; // Nothing to do.
                    }
                }
            }            

            // Load the provider's data.
            Load();

            // Reset any associated change tokens.
            base.OnReload();
        }

        #endregion
    }
}
