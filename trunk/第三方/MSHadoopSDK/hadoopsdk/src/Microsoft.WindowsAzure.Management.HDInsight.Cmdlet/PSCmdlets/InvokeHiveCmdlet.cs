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
namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.PSCmdlets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Management.Automation;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.BaseCommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.CommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;
    using Microsoft.WindowsAzure.Management.HDInsight.Logging;

    /// <summary>
    /// Cmdlet that lists submits a jobDetails to a running HDInsight cluster.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, AzureHdInsightPowerShellConstants.Hive)]
    [Alias(AzureHdInsightPowerShellConstants.HiveCmd)]
    public class InvokeHiveCmdlet : AzureHDInsightCmdlet, INewAzureHDInsightHiveJobDefinitionBase
    {
        private readonly IInvokeHiveCommand command;
        private readonly INewAzureHDInsightHiveJobDefinitionCommand hiveJobDefinitionCommand;

        /// <summary>
        /// Initializes a new instance of the InvokeHiveCmdlet class.
        /// </summary>
        public InvokeHiveCmdlet()
        {
            this.command = ServiceLocator.Instance.Locate<IAzureHDInsightCommandFactory>().CreateInvokeHive();
            this.hiveJobDefinitionCommand = ServiceLocator.Instance.Locate<IAzureHDInsightCommandFactory>().CreateNewHiveDefinition();
        }

        /// <inheritdoc />
        protected override void StopProcessing()
        {
            this.command.Cancel();
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, Position = 1,
                   HelpMessage = "The query to run in the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasQuery)]
        public string Query
        {
            get { return this.hiveJobDefinitionCommand.Query; }
            set { this.hiveJobDefinitionCommand.Query = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The name of the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasJobName)]
        public string JobName
        {
            get { return this.hiveJobDefinitionCommand.JobName; }
            set { this.hiveJobDefinitionCommand.JobName = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The query file to run in the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasQueryFile)]
        public string File
        {
            get { return this.hiveJobDefinitionCommand.File; }
            set { this.hiveJobDefinitionCommand.File = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The parameters for the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasParameters)]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public Hashtable Defines
        {
            get { return this.hiveJobDefinitionCommand.Defines; }
            set { this.hiveJobDefinitionCommand.Defines = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The files for the jobDetails.")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public string[] Files
        {
            get { return this.hiveJobDefinitionCommand.Files; }
            set { this.hiveJobDefinitionCommand.Files = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The hive arguments for the jobDetails.")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public string[] Arguments
        {
            get { return this.hiveJobDefinitionCommand.Arguments; }
            set { this.hiveJobDefinitionCommand.Arguments = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The output location to use for the jobDetails.")]
        public string StatusFolder
        {
            get { return this.hiveJobDefinitionCommand.StatusFolder; }
            set { this.hiveJobDefinitionCommand.StatusFolder = value; }
        }

        /// <inheritdoc />
        protected override void EndProcessing()
        {
            var currentConnection = this.AssertValidConnection();
            this.hiveJobDefinitionCommand.EndProcessing().Wait();
            var hiveJob = this.hiveJobDefinitionCommand.Output.Last();
            this.command.JobDefinition = hiveJob;
            this.command.Output.CollectionChanged += this.OutputItemAdded;
            this.command.Connection = currentConnection;
            try
            {
                this.command.Logger = this.Logger;
                var task = this.command.EndProcessing();
                var token = this.command.CancellationToken;
                while (!task.IsCompleted)
                {
                    this.WriteDebugLog();
                    task.Wait(1000, token);
                    if (this.command.JobDetailsStatus.IsNotNull())
                    {
                        var msg = string.Format(CultureInfo.CurrentCulture, "Waiting for jobDetails : {0}", this.command.JobId);
                        var record = new ProgressRecord(0, msg, this.command.JobDetailsStatus.StatusCode.ToString() + " : " + this.command.JobDetailsStatus.PercentComplete);
                        this.WriteProgress(record);
                    }
                    while (this.queue.Count > 0)
                    {
                        lock (this.queue)
                        {
                            this.WriteObject(this.queue.Dequeue(), true);
                        }
                    }
                }
                if (task.IsFaulted)
                {
                    throw new AggregateException(task.Exception);
                }
                this.WriteDebugLog();
            }
            catch (Exception ex)
            {
                var type = ex.GetType();
                this.Logger.Log(Severity.Error, Verbosity.Normal, this.FormatException(ex));
                this.WriteDebugLog();
                if (type == typeof(AggregateException) || type == typeof(TargetInvocationException) || type == typeof(TaskCanceledException))
                {
                    ex.Rethrow();
                }
                else
                {
                    throw;
                }
            }
            this.WriteDebugLog();
            while (this.queue.Count > 0)
            {
                lock (this.queue)
                {
                    this.WriteObject(this.queue.Dequeue(), true);
                }
            }
        }

        private Queue<object> queue = new Queue<object>();

        private void OutputItemAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                lock (this.queue)
                {
                    this.queue.Enqueue(e.NewItems);
                }
            }
        }
    }
}
