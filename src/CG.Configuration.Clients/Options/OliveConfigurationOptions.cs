using CG.Options;
using CG.Configuration.Clients.Providers;
using System.ComponentModel.DataAnnotations;

namespace CG.Configuration.Clients.Options
{
    /// <summary>
    /// This class represent configuration options for the 
    /// <see cref="OliveConfigurationProvider"/> provider.
    /// </summary>
    public class OliveConfigurationOptions : OptionsBase
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a URL for the server connection.
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// This property contains a security identifer for the server
        /// application.
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// This property contains a security key for the server application.
        /// </summary>
        public string SKey { get; set; }

        /// <summary>
        /// This property contains an optional environment name.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// This property indicates whether the provider should reload data whenver 
        /// the remote data source detects a change.
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// This property indicates whether the provider should only respond to
        /// change notifications from the current application.
        /// </summary>
        public bool FilterChangesByApplication { get; set; }

        /// <summary>
        /// This property indicates whether the provider should only respond to
        /// change notifications for a specified set of keys.
        /// </summary>
        public string[] FilterChangesByKeys { get; set; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="OliveConfigurationOptions"/>
        /// class.
        /// </summary>
        public OliveConfigurationOptions()
        {
            // Create any defaults.
            Url = "https://localhost:5005/";
            FilterChangesByApplication = true;
            FilterChangesByKeys = new string[0];
        }

        #endregion
    }
}
