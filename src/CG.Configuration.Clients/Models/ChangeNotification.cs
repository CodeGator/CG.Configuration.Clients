using System;

namespace CG.Configuration.Clients.Models
{
    /// <summary>
    /// This class represents a change notification, from the server.
    /// </summary>
    internal class ChangeNotification
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the settings key that changed.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// This property contains the security identifier for the application 
        /// that changed.
        /// </summary>
        public string Sid { get; set; }

        #endregion
    }
}
