using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mycroft
{
    /// <summary>
    /// Starts Mycroft's network communications and owns resources in the server
    /// </summary>
    class Server
    {

        /// <summary>
        /// Formats a SHA-1 hash of the certificate (thumbprint) to be used for searching 
        /// in the user's certificate store
        /// </summary>
        /// <returns>
        /// Returns the thumbprint, stripped of non-alphanumeric characters and capitalized
        /// </returns>
        internal static string FormatCertificateThumbprint(string thumbprint)
        {
            return Regex.Replace(thumbprint, @"[^a-zA-Z0-9]","", RegexOptions.IgnoreCase).ToUpper();
        }
    }
}
