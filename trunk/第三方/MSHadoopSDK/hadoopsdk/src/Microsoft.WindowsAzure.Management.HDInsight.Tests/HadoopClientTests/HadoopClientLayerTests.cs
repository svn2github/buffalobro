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
namespace Microsoft.WindowsAzure.Management.HDInsight.Tests.HadoopClientTests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Hadoop.Client;
    using Microsoft.Hadoop.Client.ClientLayer;
    using Microsoft.Hadoop.Client.HadoopJobSubmissionPocoClient;
    using Microsoft.Hadoop.Client.WebHCatRest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;
    using Microsoft.WindowsAzure.Management.HDInsight.JobSubmission;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities;
    using Microsoft.WindowsAzure.Management.HDInsight.Tests.ClientAbstractionTests;
    using Microsoft.WindowsAzure.Management.HDInsight.Tests.RestSimulator;
    using Microsoft.WindowsAzure.Management.HDInsight.Tests.Scenario;
    using Moq;

    [TestClass]
    public class HadoopClientLayerTests : IntegrationTestBase
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Nightly")]
        public void CanCreateMapReduceJob_AgainstAzure()
        {
            this.ApplyIndividualTestMockingOnly();
            this.CanCreateMapReduceJob();
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Nightly")]
        public void CanCreateStreamingMapReduceJob_AgainstAzure()
        {
            this.ApplyIndividualTestMockingOnly();
            this.CanCreateStreamingMapReduceJob();
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Nightly")]
        public void CanCreateHiveJob_AgainstAzure()
        {
            this.ApplyIndividualTestMockingOnly();
            this.CanCreateHiveJobs();
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Nightly")]
        public void CanListFullyDetailedJobs_AgainstAzure()
        {
            this.ApplyIndividualTestMockingOnly();
            this.CanListFullyDetailedJobs();
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Nightly")]
        public void CanStopJobs_AgainstAzure()
        {
            this.ApplyIndividualTestMockingOnly();
            this.CanStopJob();
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanStopJob()
        {
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();
            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var hiveJob = new HiveJobCreateParameters()
            {
                JobName = "show tables jobDetails",
                StatusFolder = "/tables",
                Query = "show tables"
            };

            var jobCreationDetails = hadoopClient.CreateHiveJob(hiveJob);
            var stoppedJob = hadoopClient.StopJob(jobCreationDetails.JobId);
            WaitForJobCancelation(hadoopClient, stoppedJob.JobId);
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanAddLogWriterToHDInsightClient()
        {
            var stringLogWriter = new StringLogWriter();
            IHadoopClientExtensions.GetPollingInterval = () => 0;
            var creds = IntegrationTestBase.GetValidCredentials();
            var subscriptionCreds = new HDInsightCertificateCredential(creds.SubscriptionId, creds.Certificate);
            var hdInsightClient = HDInsightClient.Connect(subscriptionCreds);
            hdInsightClient.AddLogWriter(stringLogWriter);
            var firstCluster = this.GetRandomCluster();

            await hdInsightClient.CreateClusterAsync(firstCluster);
            var firstClusterFromServer = hdInsightClient.GetCluster(firstCluster.Name);

            Assert.IsNotNull(firstClusterFromServer);
            var expectedMessage = string.Format(
                CultureInfo.InvariantCulture, "Creating cluster '{0}' in location {1}", firstCluster.Name, firstCluster.Location);

            Assert.IsTrue(
                stringLogWriter.Lines.Any(message => message.IndexOf(expectedMessage, StringComparison.Ordinal) > -1));
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanAddLogWriterToHadoopClient()
        {
            IHadoopClientExtensions.GetPollingInterval = () => 0;
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();
            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var stringLogWriter = new StringLogWriter();
            hadoopClient.AddLogWriter(stringLogWriter);
            var hiveJob = new HiveJobCreateParameters()
            {
                JobName = Guid.NewGuid().ToString(),
                StatusFolder = "/tables",
                Query = "show tables"
            };

            var jobCreationDetails = hadoopClient.WaitForJobCompletion(hadoopClient.CreateHiveJob(hiveJob), TimeSpan.FromMilliseconds(500), GetAbstractionContext().CancellationToken);
            Assert.IsNotNull(jobCreationDetails);

            var expectedMessage = string.Format(
                CultureInfo.InvariantCulture, "Starting jobDetails '{0}'.", hiveJob.JobName);

            Assert.IsTrue(
                stringLogWriter.Lines.Any(message => message.IndexOf(expectedMessage, StringComparison.Ordinal) > -1));
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanWaitForAJob()
        {
            IHadoopClientExtensions.GetPollingInterval = () => 0;
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();
            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var hiveJob = new HiveJobCreateParameters()
            {
                JobName = "show tables jobDetails",
                StatusFolder = "/tables",
                Query = "show tables"
            };

            var jobCreationDetails = hadoopClient.WaitForJobCompletion(hadoopClient.CreateHiveJob(hiveJob), TimeSpan.FromMilliseconds(500), GetAbstractionContext().CancellationToken);
            Assert.IsNotNull(jobCreationDetails);
            Assert.AreEqual("Completed", jobCreationDetails.StatusCode.ToString());
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanStopWaitingForAJobWhenDurrationExpires()
        {
            try
            {
                IHadoopClientExtensions.GetPollingInterval = () => 0;
                var remoteConnectionCredentials = GetRemoteConnectionCredentials();
                var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
                var hiveJob = new HiveJobCreateParameters()
                {
                    JobName = "show tables jobDetails timeout",
                    StatusFolder = "/tables",
                    Query = "show tables"
                };

                hadoopClient.WaitForJobCompletion(hadoopClient.CreateHiveJob(hiveJob), TimeSpan.FromMilliseconds(5), GetAbstractionContext().CancellationToken);
                Assert.Fail("The expected exception was not thrown.");
            }
            catch (TimeoutException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Timeout waiting for jobDetails completion"), "The expected exception was not of the right type.");
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanWaitForAJobThatFailed()
        {
            IHadoopClientExtensions.GetPollingInterval = () => 0;
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();
            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var hiveJob = new HiveJobCreateParameters()
            {
                JobName = "show tables Fail jobDetails",
                StatusFolder = "/tables",
                Query = "show tables"
            };

            var jobCreationDetails = hadoopClient.WaitForJobCompletion(hadoopClient.CreateHiveJob(hiveJob), TimeSpan.FromMilliseconds(500), GetAbstractionContext().CancellationToken);
            Assert.IsNotNull(jobCreationDetails);
            Assert.AreEqual("Failed", jobCreationDetails.StatusCode.ToString());
        }


        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanCreateClustersInParallel()
        {
            IHadoopClientExtensions.GetPollingInterval = () => 0;
            var creds = IntegrationTestBase.GetValidCredentials();
            var subscriptionCreds = new HDInsightCertificateCredential(creds.SubscriptionId, creds.Certificate);
            var hadoopClient = HDInsightClient.Connect(subscriptionCreds);

            var firstCluster = this.GetRandomCluster();
            var secondCluster = this.GetRandomCluster();

            await hadoopClient.CreateClusterAsync(firstCluster);
            await hadoopClient.CreateClusterAsync(secondCluster);
            var firstClusterFromServer = hadoopClient.GetCluster(firstCluster.Name);
            var secondClusterFromServer = hadoopClient.GetCluster(secondCluster.Name);

            Assert.IsNotNull(firstClusterFromServer);
            Assert.IsNotNull(secondClusterFromServer);
        }

        internal class MockIHadoopClientFactoryManager : IHadoopClientFactoryManager
        {
            public IJobSubmissionClient retval;
            public MockIHadoopClientFactoryManager(IJobSubmissionClient retval)
            {
                this.retval = retval;
            }

            public void RegisterFactory<TCredentials, TServiceInterface, TServiceImplementation>()
                where TCredentials : IJobSubmissionClientCredential
                where TServiceInterface : IHadoopClientFactory<TCredentials>
                where TServiceImplementation : class, TServiceInterface
            {
            }

            public void UnregisterFactory<TCredentials>() where TCredentials : IJobSubmissionClientCredential
            {
            }

            public IJobSubmissionClient Create(IJobSubmissionClientCredential credentials)
            {
                return retval;
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanStopWaitingIfWeHaveLostConnections()
        {
            var client = new Moq.Mock<IJobSubmissionClient>(MockBehavior.Loose);
            client.Setup(c => c.CreateHiveJob(It.IsAny<HiveJobCreateParameters>()))
                  .Returns(new JobCreationResults() { JobId = "1234" });

            client.Setup(c => c.GetJobAsync(It.IsAny<string>()))
                  .Throws(new HttpLayerException(HttpStatusCode.BadGateway, "Some message"));

            var factory = new MockIHadoopClientFactoryManager(client.Object);

            ServiceLocator.Instance.Locate<IServiceLocationIndividualTestManager>().Override<IHadoopClientFactoryManager>(factory);

            try
            {
                IHadoopClientExtensions.GetPollingInterval = () => 0;
                var remoteConnectionCredentials = GetRemoteConnectionCredentials();
                var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
                var hiveJob = new HiveJobCreateParameters()
                {
                    JobName = "show tables Fail jobDetails",
                    StatusFolder = "/tables",
                    Query = "show tables"
                };

                hadoopClient.WaitForJobCompletion(hadoopClient.CreateHiveJob(hiveJob), TimeSpan.FromMilliseconds(500), GetAbstractionContext().CancellationToken);
                Assert.Fail("The expected exception was not thrown.");
            }
            catch (HttpLayerException ex)
            {
                Assert.AreEqual(HttpStatusCode.BadGateway, ex.RequestStatusCode, "The exception did not contain the correct status code the one returned was {0}", ex.RequestStatusCode);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanCreateMapReduceJob()
        {
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var mapReduceJob = new MapReduceJobCreateParameters()
            {
                ClassName = "pi",
                JobName = "pi estimation jobDetails",
                JarFile = "/example/hadoop-examples.jar",
                StatusFolder = "/piresults"
            };

            mapReduceJob.Arguments.Add("16");
            mapReduceJob.Arguments.Add("10000");
            var jobCreationDetails = hadoopClient.CreateMapReduceJob(mapReduceJob);
            WaitForJobCompletion(jobCreationDetails, hadoopClient);
            Assert.IsNull(jobCreationDetails.ErrorCode, "Should not fail mr jobDetails submission");
            Assert.IsNotNull(jobCreationDetails.JobId, "Should have a non-null jobDetails id");
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanCreateStreamingMapReduceJob()
        {
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var mapReduceJob = new StreamingMapReduceJobCreateParameters()
            {
                JobName = "englebert jobDetails",
                StatusFolder = "/engoutput",
                Mapper = "/example/engleert.exe",
                Input = "/example/engleert.exe",
                Output = Guid.NewGuid().ToString()
            };

            var jobCreationDetails = hadoopClient.CreateStreamingJob(mapReduceJob);
            WaitForJobCompletion(jobCreationDetails, hadoopClient);
            Assert.IsNull(jobCreationDetails.ErrorCode, "Should not fail streaming mr jobDetails submission");
            Assert.IsNotNull(jobCreationDetails.JobId, "Should have a streaming non-null jobDetails id");
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanCreateHiveJobs()
        {
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var hiveJob = new HiveJobCreateParameters()
            {
                JobName = "show tables jobDetails",
                StatusFolder = "/tables",
                Query = "show tables"
            };

            var jobCreationDetails = hadoopClient.CreateHiveJob(hiveJob);
            WaitForJobCompletion(jobCreationDetails, hadoopClient);
            Assert.IsNull(jobCreationDetails.ErrorCode, "Should not fail hive jobDetails submission");
            Assert.IsNotNull(jobCreationDetails.JobId, "Should have a non-null jobDetails id");
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanCreateSqoopJob_RemoteCreds()
        {
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var sqoopJob = new SqoopJobCreateParameters()
            {
                StatusFolder = "/tables",
                Command = "show tables"
            };

            var jobCreationDetails = hadoopClient.CreateSqoopJob(sqoopJob);
            WaitForJobCompletion(jobCreationDetails, hadoopClient);
            Assert.IsNull(jobCreationDetails.ErrorCode, "Should not fail sqoop jobDetails submission");
            Assert.IsNotNull(jobCreationDetails.JobId, "Should have a non-null jobDetails id");
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanCreateSqoopJob_SubscriptionCreds()
        {
            var certificateCredentials = GetHDInsightCertificateCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var sqoopJob = new SqoopJobCreateParameters()
            {
                StatusFolder = "/tables",
                Command = "show tables"
            };

            var jobCreationDetails = hadoopClient.CreateSqoopJob(sqoopJob);
            WaitForJobCompletion(jobCreationDetails, hadoopClient);
            Assert.IsNull(jobCreationDetails.ErrorCode, "Should not fail sqoop jobDetails submission");
            Assert.IsNotNull(jobCreationDetails.JobId, "Should have a non-null jobDetails id");
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanListFullyDetailedJobs()
        {
            var remoteConnectionCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(remoteConnectionCredentials);
            var jobs = hadoopClient.ListJobs();
            foreach (var job in jobs.Jobs)
            {
                Assert.IsNotNull(job.SubmissionTime, "expected fully materialized objects");
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanGetJobOutput()
        {
            var certificateCredentials = GetHDInsightCertificateCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            var output = hadoopClient.GetJobOutput(job.JobId);
            var content = new StreamReader(output).ReadToEnd();
            Assert.IsTrue(content.Length >= 0);
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanGetJobErrorLogs()
        {
            var certificateCredentials = GetHDInsightCertificateCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            var output = hadoopClient.GetJobErrorLogs(job.JobId);
            var content = new StreamReader(output).ReadToEnd();
            Assert.IsTrue(content.Length >= 0);
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanGetJobTaskLogSummary()
        {
            var certificateCredentials = GetHDInsightCertificateCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            var output = hadoopClient.GetJobTaskLogSummary(job.JobId);
            var content = new StreamReader(output).ReadToEnd();
            Assert.IsTrue(content.Length >= 0);
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanDownloadJobTaskLogs()
        {
            var certificateCredentials = GetHDInsightCertificateCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            var logsDirectory = Directory.CreateDirectory(Guid.NewGuid().ToString());

            hadoopClient.DownloadJobTaskLogs(job.JobId, logsDirectory.Name);
            var downloadedFilesCount = Directory.EnumerateFiles(logsDirectory.Name).ToList();
            Assert.IsTrue(downloadedFilesCount.Count > 0);
        }


        [TestMethod]
        [TestCategory("CheckIn")]
        public void CannotGetJobOutput_WithRemoteCredentials()
        {
            var certificateCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            AssertStorageOperationsNotSupported(() => hadoopClient.GetJobOutput(job.JobId));
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CannotGetJobErrorLogs_WithRemoteCredentials()
        {
            var certificateCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            AssertStorageOperationsNotSupported(() => hadoopClient.GetJobErrorLogs(job.JobId));
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CannotGetJobTaskLogSummary_WithRemoteCredentials()
        {
            var certificateCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            AssertStorageOperationsNotSupported(() => hadoopClient.GetJobTaskLogSummary(job.JobId));
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CannotDownloadJobTaskLogs_WithRemoteCredentials()
        {
            var certificateCredentials = GetRemoteConnectionCredentials();

            var hadoopClient = JobSubmissionClientFactory.Connect(certificateCredentials);
            var jobs = hadoopClient.ListJobs();
            var job = jobs.Jobs.First(j => !string.IsNullOrEmpty(j.StatusDirectory));
            AssertStorageOperationsNotSupported(() => hadoopClient.DownloadJobTaskLogs(job.JobId, Guid.NewGuid().ToString()));
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanGetStorageLocationOfRootOutputDirectory()
        {
            var storageAccount = IntegrationTestBase.GetWellKnownStorageAccounts().First();
            var statusPath = HDInsightHadoopClient.GetStatusDirectoryPath("/StatusDirectory", storageAccount, "hdp", "stdout");
            string expectedPath = String.Format(CultureInfo.InvariantCulture, "{0}{1}@{2}/StatusDirectory/stdout", Constants.WabsProtocolSchemeName, storageAccount.Container, storageAccount.Name);
            Assert.AreEqual(expectedPath, statusPath.AbsoluteUri);
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public void CanGetStorageLocationOfUserOutputDirectory()
        {
            var storageAccount = IntegrationTestBase.GetWellKnownStorageAccounts().First();
            var statusPath = HDInsightHadoopClient.GetStatusDirectoryPath("StatusDirectory", storageAccount, "hdp", "stdout");
            string expectedPath = String.Format(CultureInfo.InvariantCulture, "{0}{1}@{2}/user/hdp/StatusDirectory/stdout", Constants.WabsProtocolSchemeName, storageAccount.Container, storageAccount.Name);
            Assert.AreEqual(expectedPath, statusPath.AbsoluteUri);
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanRethrowExceptionInRestLayer_ListJobs()
        {
            var remoteConnectionCredentials = new BasicAuthCredential()
            {
                UserName = IntegrationTestBase.TestCredentials.HadoopUserName,
                Password = IntegrationTestBase.TestCredentials.AzurePassword,
                Server = GatewayUriResolver.GetGatewayUri(IntegrationTestBase.TestCredentials.WellKnownCluster.DnsName)
            };
            var jobsPocoClient = new RemoteHadoopJobSubmissionPocoClientFactory().Create(remoteConnectionCredentials, GetAbstractionContext());

            this.ApplyIndividualTestMockingOnly();
            this.EnableHttpSpy();
            this.EnableHttpMock((ca) => new HttpResponseMessageAbstraction(
                                        HttpStatusCode.NotFound, new HttpResponseHeadersAbstraction(), "ListJobs failed."));
            try
            {
                await jobsPocoClient.ListJobs();
                Assert.Fail("should have thrown.");
            }
            catch (HttpLayerException httpLayerException)
            {
                Assert.AreEqual("Request failed with code:NotFound\r\nContent:ListJobs failed.", httpLayerException.Message);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanRethrowExceptionInRestLayer_GetJobs()
        {
            var remoteConnectionCredentials = new BasicAuthCredential()
            {
                UserName = IntegrationTestBase.TestCredentials.HadoopUserName,
                Password = IntegrationTestBase.TestCredentials.AzurePassword,
                Server = GatewayUriResolver.GetGatewayUri(IntegrationTestBase.TestCredentials.WellKnownCluster.DnsName)
            };
            var jobsPocoClient = new RemoteHadoopJobSubmissionPocoClientFactory().Create(remoteConnectionCredentials, GetAbstractionContext());

            this.ApplyIndividualTestMockingOnly();
            this.EnableHttpSpy();
            this.EnableHttpMock((ca) => new HttpResponseMessageAbstraction(
                                        HttpStatusCode.NotFound, new HttpResponseHeadersAbstraction(), "GetJob failed."));
            try
            {
                await jobsPocoClient.GetJob(Guid.NewGuid().ToString());
                Assert.Fail("should have thrown.");
            }
            catch (HttpLayerException httpLayerException)
            {
                Assert.AreEqual("Request failed with code:NotFound\r\nContent:GetJob failed.", httpLayerException.Message);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanRethrowExceptionInRestLayer_StopJobs()
        {
            var remoteConnectionCredentials = new BasicAuthCredential()
            {
                UserName = IntegrationTestBase.TestCredentials.HadoopUserName,
                Password = IntegrationTestBase.TestCredentials.AzurePassword,
                Server = GatewayUriResolver.GetGatewayUri(IntegrationTestBase.TestCredentials.WellKnownCluster.DnsName)
            };
            var jobsPocoClient = new RemoteHadoopJobSubmissionPocoClientFactory().Create(remoteConnectionCredentials, GetAbstractionContext());

            this.ApplyIndividualTestMockingOnly();
            this.EnableHttpSpy();
            this.EnableHttpMock((ca) => new HttpResponseMessageAbstraction(
                                        HttpStatusCode.NotFound, new HttpResponseHeadersAbstraction(), "StopJob failed."));
            try
            {
                await jobsPocoClient.StopJob(Guid.NewGuid().ToString());
                Assert.Fail("should have thrown.");
            }
            catch (HttpLayerException httpLayerException)
            {
                Assert.AreEqual("Request failed with code:NotFound\r\nContent:StopJob failed.", httpLayerException.Message);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanRethrowExceptionInRestLayer_CreateMapReduce()
        {
            var remoteConnectionCredentials = new BasicAuthCredential()
            {
                UserName = IntegrationTestBase.TestCredentials.HadoopUserName,
                Password = IntegrationTestBase.TestCredentials.AzurePassword,
                Server = GatewayUriResolver.GetGatewayUri(IntegrationTestBase.TestCredentials.WellKnownCluster.DnsName)
            };
            var jobsPocoClient = new RemoteHadoopJobSubmissionPocoClientFactory().Create(remoteConnectionCredentials, GetAbstractionContext());

            this.ApplyIndividualTestMockingOnly();
            this.EnableHttpSpy();
            this.EnableHttpMock((ca) => new HttpResponseMessageAbstraction(
                                        HttpStatusCode.NotFound, new HttpResponseHeadersAbstraction(), "SubmitMapReduceJob failed."));
            try
            {
                var mapReduceJobDefinition = new MapReduceJobCreateParameters()
                {
                    JobName = "pi estimation jobDetails",
                    ClassName = "pi",
                    JarFile = Constants.WabsProtocolSchemeName + "container@hostname/examples.jar"
                };
                mapReduceJobDefinition.Arguments.Add("16");
                mapReduceJobDefinition.Arguments.Add("10000");

                await jobsPocoClient.SubmitMapReduceJob(mapReduceJobDefinition);
                Assert.Fail("should have thrown.");
            }
            catch (HttpLayerException httpLayerException)
            {
                Assert.AreEqual("Request failed with code:NotFound\r\nContent:SubmitMapReduceJob failed.", httpLayerException.Message);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        public async Task CanRethrowExceptionInRestLayer_CreateHive()
        {
            var remoteConnectionCredentials = new BasicAuthCredential()
            {
                UserName = IntegrationTestBase.TestCredentials.HadoopUserName,
                Password = IntegrationTestBase.TestCredentials.AzurePassword,
                Server = GatewayUriResolver.GetGatewayUri(IntegrationTestBase.TestCredentials.WellKnownCluster.DnsName)
            };
            var jobsPocoClient = new RemoteHadoopJobSubmissionPocoClientFactory().Create(remoteConnectionCredentials, GetAbstractionContext());

            this.ApplyIndividualTestMockingOnly();
            this.EnableHttpSpy();
            this.EnableHttpMock((ca) => new HttpResponseMessageAbstraction(
                                        HttpStatusCode.NotFound, new HttpResponseHeadersAbstraction(), "SubmitHiveJob failed."));
            try
            {
                var hiveJobDefinition = new HiveJobCreateParameters()
                {
                    JobName = "pi estimation jobDetails",
                    Query = "Show tables"
                };
                hiveJobDefinition.Arguments.Add("16");
                hiveJobDefinition.Arguments.Add("10000");

                await jobsPocoClient.SubmitHiveJob(hiveJobDefinition);
                Assert.Fail("should have thrown.");
            }
            catch (HttpLayerException httpLayerException)
            {
                Assert.AreEqual("Request failed with code:NotFound\r\nContent:SubmitHiveJob failed.", httpLayerException.Message);
            }
        }

        private static JobSubmissionCertificateCredential GetHDInsightCertificateCredentials()
        {
            var creds = IntegrationTestBase.GetValidCredentials();
            var remoteConnectionCredentials =
                new JobSubmissionCertificateCredential(
                    new HDInsightCertificateCredential() { SubscriptionId = creds.SubscriptionId, Certificate = creds.Certificate },
                    IntegrationTestBase.TestCredentials.WellKnownCluster.DnsName);
            return remoteConnectionCredentials;
        }

        private static void WaitForJobCompletion(JobCreationResults jobDetails, IJobSubmissionClient client)
        {
            var jobInProgress = client.GetJob(jobDetails.JobId);
            while (jobInProgress.StatusCode != Hadoop.Client.JobStatusCode.Completed && jobInProgress.StatusCode != Hadoop.Client.JobStatusCode.Failed)
            {
                jobInProgress = client.GetJob(jobInProgress.JobId);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        private static void WaitForJobCancelation(IJobSubmissionClient client, string jobId)
        {
            var jobInProgress = client.GetJob(jobId);
            try
            {
                while (jobInProgress != null && jobInProgress.StatusCode != Hadoop.Client.JobStatusCode.Canceled)
                {
                    jobInProgress = client.GetJob(jobInProgress.JobId);
                    Thread.Sleep(TimeSpan.FromSeconds(0.25));
                }
            }
            catch (HttpRequestException)
            {
            }
        }

        private static BasicAuthCredential GetRemoteConnectionCredentials()
        {
            var testCluster = SyncClientScenarioTests.GetHttpAccessEnabledCluster();
            var remoteConnectionCredentials = new BasicAuthCredential()
            {
                UserName = testCluster.HttpUserName,
                Password = testCluster.HttpPassword,
                Server = GatewayUriResolver.GetGatewayUri(testCluster.ConnectionUrl)
            };

            return remoteConnectionCredentials;
        }


        private static void AssertStorageOperationsNotSupported(Action action)
        {
            try
            {
                action();
                Assert.Fail("Test was expected to throw an exception");
            }
            catch (NotSupportedException notSupported)
            {
                Assert.AreEqual(notSupported.Message, "Access to cluster resources requires Subscription details.");
            }
        }
    }
}
