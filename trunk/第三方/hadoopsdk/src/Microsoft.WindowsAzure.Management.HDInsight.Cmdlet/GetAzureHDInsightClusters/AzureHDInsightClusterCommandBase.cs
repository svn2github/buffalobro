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

namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters
{
    using System.Threading;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters.BaseInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters.Extensions;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.ServiceLocation;

    internal abstract class AzureHDInsightClusterCommandBase : AzureHDInsightCommandBase, IAzureHDInsightClusterCommandBase
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public override CancellationToken CancellationToken
        {
            get { return this.tokenSource.Token; }
        }

        public string Name { get; set; }

        public override void Cancel()
        {
            this.tokenSource.Cancel();
        }

        protected IHDInsightClient GetClient()
        {
            IHDInsightClient clientInstance =
                ServiceLocator.Instance.Locate<IAzureHDInsightClusterManagementClientFactory>().Create(this.GetSubscriptionCertificateCredentials());
            clientInstance.SetCancellationSource(this.tokenSource);
            if (this.Logger.IsNotNull())
            {
                clientInstance.AddLogWriter(this.Logger);
            }

            return clientInstance;
        }
    }
}
