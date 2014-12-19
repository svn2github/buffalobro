﻿namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal abstract class AzureHDInsightCommandBase : IAzureHDInsightCommandBase
    {
        public abstract void EndProcessing();
    }
}
