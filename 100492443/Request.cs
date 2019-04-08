using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443
{
    /// <summary>
    /// Represents a request that can be submitted to the
    /// CritterWorld environment.
    /// </summary>
    abstract class Request<T> where T : EventArgs
    {
        /// <summary>
        /// The message associated with this request.
        /// </summary>
        public string RequestMessage { get; private set; }

        /// <summary>
        /// Event for a resolution of this request.
        /// </summary>
        public event EventHandler<T> RequestResolved;

        /// <summary>
        /// Creates a new request with a specified message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        public Request(string requestMessage)
        {
            RequestMessage = requestMessage;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Request() : this("")
        { }

        /// <summary>
        /// This method is called when the request has been
        /// resolved and should be removed.
        /// </summary>
        public void OnRequestResolved(T args)
        {
            RequestResolved?.Invoke(this, args);
        }
    }

    /// <summary>
    /// Non-generic version of the <see cref="Request{T}"/> class.
    /// </summary>
    abstract class Request : Request<EventArgs>
    { }
}
