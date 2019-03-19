using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443
{

    /// <summary>
    /// Represents a request to understand how
    /// much time has passed on the current level.
    /// </summary>
    class TimeElapsedRequest : Request<TimeElapsedRequest.Args>
    {
        /// <summary>
        /// Arguments object for this request.
        /// </summary>
        public class Args : EventArgs
        {
            public int ElapsedTime { get; set; }
        }

        public TimeElapsedRequest(int requestID) : base("GET_LEVEL_TIME_REMAINING:" + requestID)
        { }
    }
}
