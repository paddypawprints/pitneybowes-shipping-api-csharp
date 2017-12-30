using System;
using System.Collections.Generic;
using System.Net;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Exception if the Shipping API call fails
    /// </summary>
    public class ShippingAPIException : Exception
    {
        /// <summary>
        /// Response returned by the API
        /// </summary>
        public ShippingApiResponse ErrorResponse { get; set; }
        /// <summary>
        /// Default constructor - does nothing
        /// </summary>
        public ShippingAPIException()
        {
        }
        /// <summary>
        /// Constructor, set the API response object
        /// </summary>
        /// <param name="response">As returned by the web service classes</param>
        public ShippingAPIException(ShippingApiResponse response) : this()
        {
            ErrorResponse = response;
        }
        /// <summary>
        /// Set the response object and error message
        /// </summary>
        /// <param name="response">As returned by the web service classes</param>
        /// <param name="message"></param>
        public ShippingAPIException(ShippingApiResponse response, string message): this(message)
        {
            ErrorResponse = response;
        }
        /// <summary>
        /// Set an error message - response object will be null
        /// </summary>
        /// <param name="message"></param>
        public ShippingAPIException(string message): base(message)
        {
        }
        /// <summary>
        /// Set a message, inner exception. Response object will be null.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ShippingAPIException(string message, Exception inner): base(message, inner)
        {
        }
        /// <summary>
        /// Set a message, inner exception and a response object.
        /// </summary>
        /// <param name="response">As returned by the web service classes</param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ShippingAPIException(ShippingApiResponse response, string message, Exception inner): this(message, inner)
        {
            ErrorResponse = response;
        }

    }
}
