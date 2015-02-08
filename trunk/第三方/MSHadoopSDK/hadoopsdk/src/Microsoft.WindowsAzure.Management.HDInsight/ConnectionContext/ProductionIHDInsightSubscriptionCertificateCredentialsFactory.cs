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
namespace Microsoft.WindowsAzure.Management.HDInsight
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;

    internal class ProductionIHDInsightSubscriptionCertificateCredentialsFactory : IHDInsightSubscriptionCertificateCredentialsFactory
    {
        private const string EndPoint = @"https://management.core.windows.net:8443/";

        private const string Namespace = @"hdinsight";

        public IHDInsightCertificateCredential Create(IHDInsightCertificateCredential credentials)
        {
            credentials.ArgumentNotNull("credentials");
            return new HDInsightCertificateCredential()
            {
                Certificate = credentials.Certificate,
                DeploymentNamespace = credentials.DeploymentNamespace,
                Endpoint = credentials.Endpoint,
                SubscriptionId = credentials.SubscriptionId
            };
        }
    }
}