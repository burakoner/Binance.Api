using ApiSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.ApiClient.Enums
{
    /// <summary>
    /// Download status
    /// </summary>
    public enum DownloadStatus
    {
        /// <summary>
        /// Processing
        /// </summary>
        [Map("processing")]
        Processing,
        /// <summary>
        /// Ready for download
        /// </summary>
        [Map("completed")]
        Completed
    }
}
