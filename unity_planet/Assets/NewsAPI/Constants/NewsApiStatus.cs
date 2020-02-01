using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAPI.Constants
{
    public enum NewsApiStatus
    {
        Unknown,
        
        /// <summary>
        /// Request was successful
        /// </summary>
        Ok,
        /// <summary>
        /// Request failed
        /// </summary>
        Error
    }
}
