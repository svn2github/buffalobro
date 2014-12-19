﻿// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License.  You may obtain a copy
// of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
// WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABLITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

namespace Microsoft.Hadoop.WebClient.JobManagement
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Class that communicates with the WebHCat REST interface.
    /// </summary>
    public class WebHCatHttpClient : IWebHCatHttpClient
    {
        /// <summary>
        /// Gets or sets the handler to use when creating an HttpClient.
        /// </summary>
        internal WebRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Gets or Sets Uri for the WebHCat endpoint.
        /// </summary>
        internal Uri WebHCatUri { get; set; }

        public WebHCatHttpClient(Uri webHcatUri)
        {
            this.WebHCatUri = webHcatUri;
        }

        /// <inheritdoc/>
        public Task<string> ExecuteHiveJob(string query, string statusDir)
        {
            throw new System.NotImplementedException();
        }
    }
}
