using System;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel for handling errors in the application.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Request ID associated with the error.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Indicates whether the Request ID should be shown.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
