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
namespace Microsoft.WindowsAzure.Management.HDInsight.TestUtilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Management.Automation;
    using System.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Hadoop.Client;
    using Microsoft.Hadoop.Client.HadoopJobSubmissionPocoClient.RemoteHadoop;
    using Microsoft.Hadoop.Client.Storage;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.Configuration;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning.Asv;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning.AzureManagementClient;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning.PocoClient;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning.RestClient;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters.BaseInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.PSCmdlets;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Logging;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.JobSubmission.PocoClient;
    using Microsoft.WindowsAzure.Management.HDInsight.Logging;
    using Microsoft.WindowsAzure.Management.HDInsight.Tests;
    using Microsoft.WindowsAzure.Management.HDInsight.Tests.RestSimulator;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities.PowerShellTestAbstraction.Concreates;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities.PowerShellTestAbstraction.Interfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities.RestSimulator;

    public class IntegrationTestBase : DisposableObject
    {
        public IntegrationTestBase()
        {
            HDInsightClient.DefaultPollingInterval = TimeSpan.FromSeconds(1);
            var underlying = new HttpClientAbstractionFactory();
            this.factory = new HttpAbstractionSimulatorFactory(underlying);
        }

        private HttpAbstractionSimulatorFactory factory;
        private bool httpSpyEnabled;

        public virtual void Initialize()
        {
            HDInsightClient.DefaultPollingInterval = TimeSpan.FromSeconds(1);
            IHadoopClientExtensions.GetPollingInterval = () => Constants.PollingInterval;
            this.ApplyFullMocking();
            this.ResetIndividualMocks();
            this.httpSpyEnabled = false;
            HDInsightManagementRestSimulatorClient.ResetConnectivityDefaultsAllClusters();
        }

        public virtual void TestCleanup()
        {
            HDInsightClient.DefaultPollingInterval = TimeSpan.FromSeconds(1);
            IHadoopClientExtensions.GetPollingInterval = () => Constants.PollingInterval;
            this.ApplyFullMocking();
            this.ResetIndividualMocks();
            this.httpSpyEnabled = false;
            HDInsightManagementRestSimulatorClient.ResetConnectivityDefaultsAllClusters();
        }

        protected void EnableHttpSpy()
        {
            var manager = ServiceLocator.Instance.Locate<IServiceLocationIndividualTestManager>();
            manager.Override<IHttpClientAbstractionFactory>(this.factory);
            this.httpSpyEnabled = true;
        }

        internal void EnableHttpMock(Func<IHttpClientAbstraction, IHttpResponseMessageAbstraction> asyncMoc)
        {
            if (!this.httpSpyEnabled)
            {
                this.EnableHttpSpy();
            }
            this.factory.AsyncMock = asyncMoc;
        }

        protected void DisableHttpMock()
        {
            this.factory.AsyncMock = null;
        }

        protected void ClearHttpCalls()
        {
            this.factory.Clients.Clear();
        }

        internal IEnumerable<Tuple<IHttpClientAbstraction, IHttpResponseMessageAbstraction>> GetHttpCalls()
        {
            return this.factory.Clients;
        }

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "This is a bad pattern but is used strictly for test purposes.  This should be cleaned up later and should not be duplicated as a pattern elsewhere. [TGS]")]
        public static readonly IntegrationTestManager TestManager = new IntegrationTestManager();
        public static AzureTestCredentials TestCredentials { get; private set; }
        internal static Dictionary<string, string> testToClusterMap = new Dictionary<string, string>();

        protected static AzureTestCredentials GetCredentials(string name)
        {
            return TestManager.GetCredentials(name);
        }

        private static AzureTestCredentials CloneForEnvironment(AzureTestCredentials orig, int index)
        {
            AzureTestCredentials retval = new AzureTestCredentials();
            retval.AzurePassword = orig.AzurePassword;
            retval.AzureUserName = orig.AzureUserName;
            retval.Certificate = orig.Certificate;
            retval.CredentialsName = orig.CredentialsName;
            retval.HadoopUserName = orig.HadoopUserName;
            retval.InvalidCertificate = orig.InvalidCertificate;
            retval.SubscriptionId = orig.SubscriptionId;
            retval.ResourceProviderProperties = orig.ResourceProviderProperties;
            retval.WellKnownCluster = new KnownCluster()
            {
                Cluster = orig.WellKnownCluster.Cluster,
                DnsName = orig.WellKnownCluster.DnsName,
                Version = orig.WellKnownCluster.Version
            };
            retval.Environments = new CreationDetails[0];
            var env = retval.Environments[0] = new CreationDetails();
            var origEnv = orig.Environments[index];
            retval.CloudServiceName = orig.CloudServiceName;
            env.DefaultStorageAccount = new StorageAccountCredentials()
            {
                Container = origEnv.DefaultStorageAccount.Container,
                Key = origEnv.DefaultStorageAccount.Key,
                Name = origEnv.DefaultStorageAccount.Name
            };
            retval.Endpoint = orig.Endpoint;
            env.Location = origEnv.Location;
            retval.EnvironmentType = orig.EnvironmentType;
            List<StorageAccountCredentials> storageAccounts = new List<StorageAccountCredentials>();
            foreach (var storageAccountCredentials in origEnv.AdditionalStorageAccounts)
            {
                var account = new StorageAccountCredentials()
                {
                    Container = storageAccountCredentials.Container,
                    Key = storageAccountCredentials.Key,
                    Name = storageAccountCredentials.Name
                };
                storageAccounts.Add(account);
            }
            env.AdditionalStorageAccounts = storageAccounts.ToArray();
            List<MetastoreCredentials> stores = new List<MetastoreCredentials>();
            foreach (var metastoreCredentials in origEnv.HiveStores)
            {
                var metaStore = new MetastoreCredentials()
                {
                    Database = metastoreCredentials.Database,
                    Description = metastoreCredentials.Description,
                    SqlServer = metastoreCredentials.SqlServer
                };
            }
            env.HiveStores = stores.ToArray();
            stores.Clear();
            foreach (var metastoreCredentials in origEnv.OozieStores)
            {
                var metaStore = new MetastoreCredentials()
                {
                    Database = metastoreCredentials.Database,
                    Description = metastoreCredentials.Description,
                    SqlServer = metastoreCredentials.SqlServer
                };
            }
            env.OozieStores = stores.ToArray();
            return retval;
        }

        public static IEnumerable<WabStorageAccountConfiguration> GetWellKnownStorageAccounts()
        {
            var accounts = new List<WabStorageAccountConfiguration>();
            accounts.AddRange(IntegrationTestBase.TestCredentials.Environments.Select(env => new WabStorageAccountConfiguration(env.DefaultStorageAccount.Name, env.DefaultStorageAccount.Key, env.DefaultStorageAccount.Container)));
            accounts.AddRange(IntegrationTestBase.TestCredentials.Environments.SelectMany(env => env.AdditionalStorageAccounts).Select(acc => new WabStorageAccountConfiguration(acc.Name, acc.Key, acc.Container)));
            return accounts;
        }

        public static IEnumerable<AzureTestCredentials> GetAllCredentials()
        {
            return TestManager.GetAllCredentials();
        }

        public static AzureTestCredentials GetCredentialsForLocation(string name, string location)
        {
            var namedCreds = GetCredentials(name);
            for (int i = 0; i < namedCreds.Environments.Length; i++)
            {
                if (namedCreds.Environments[i].Location == location)
                {
                    return CloneForEnvironment(namedCreds, i);
                }
            }
            return null;
        }

        public static AzureTestCredentials GetCredentialsForLocation(string location)
        {
            return GetCredentialsForLocation(TestCredentialsNames.Default, location);
        }

        public static AzureTestCredentials GetCredentialsForEnvironmentType(EnvironmentType type)
        {
            var environments = TestManager.GetAllCredentials().ToArray();
            for (int i = 0; i < environments.Length; i++)
            {
                if (environments[i].EnvironmentType == type)
                {
                    return environments[i];
                }
            }
            return null;
        }

        protected static string ClusterPrefix;
        private static IHDInsightCertificateCredential validCredentials;
        private static IHDInsightCertificateCredential invalidSubscriptionId;
        private static IHDInsightCertificateCredential invalidCertificate;
        public TestContext TestContext { get; set; }

        public static class TestCredentialsNames
        {
            public const string Default = "default";
        }

        public static void TestRunCleanup()
        {
            // First get the simulator clusters.
            var runManager = ServiceLocator.Instance.Locate<IServiceLocationSimulationManager>();
            runManager.MockingLevel = ServiceLocationMockingLevel.ApplyTestRunMockingOnly;
            var factory = ServiceLocator.Instance.Locate<IHDInsightClientFactory>();
            var creds = GetCredentials(TestCredentialsNames.Default);
            var client = factory.Create(new HDInsightCertificateCredential(creds.SubscriptionId, new X509Certificate2(creds.Certificate)));
            var clusters = client.ListClusters().ToList();
            var simClusters = clusters.Where(c => c.Name.StartsWith(ClusterPrefix, StringComparison.OrdinalIgnoreCase));

            foreach (var cluster in simClusters)
            {
                client.DeleteCluster(cluster.Name);
            }

            // Next get the live clusters.
            runManager.MockingLevel = ServiceLocationMockingLevel.ApplyNoMocking;
            factory = ServiceLocator.Instance.Locate<IHDInsightClientFactory>();
            client = factory.Create(new HDInsightCertificateCredential(creds.SubscriptionId, new X509Certificate2(creds.Certificate)));
            clusters = client.ListClusters().ToList();
            var liveClusters = clusters.Where(c => c.Name.StartsWith(ClusterPrefix, StringComparison.OrdinalIgnoreCase));

            foreach (var cluster in liveClusters)
            {
                client.DeleteCluster(cluster.Name);
            }
        }

        private static List<Type> types = new List<Type>();

        public static void TestRunSetup()
        {
            // This is to ensure that all key assemblies are loaded before IOC registration is required.
            // This is only necessary for the test system as load order is correct for a production run.
            types.Add(typeof(NewAzureHDInsightClusterCmdlet));
            // Sets the simulator
            var runManager = ServiceLocator.Instance.Locate<IServiceLocationSimulationManager>();
            ServiceLocator.Instance.Locate<ILogger>().AddWriter(new ConsoleLogWriter(Severity.None, Verbosity.Diagnostic));
            runManager.RegisterType<IAsvValidatorClientFactory, AsvValidatorSimulatorClientFactory>();
            runManager.RegisterType<IHDInsightManagementRestClientFactory, HDInsightManagementRestSimulatorClientFactory>();
            runManager.RegisterType<IRdfeServiceRestClientFactory, RdfeServiceRestSimulatorClientFactory>();
            runManager.RegisterType<ISubscriptionRegistrationClientFactory, SubscriptionRegistrationSimulatorClientFactory>();
            runManager.RegisterType<IAzureHDInsightClusterConfigurationAccessorFactory, AzureHDInsightClusterConfigurationAccessorSimulatorFactory>();
            runManager.RegisterInstance<IWabStorageAbstractionFactory>(StorageAccountSimulatorFactory.Instance);
            runManager.RegisterType<IAzureHDInsightSubscriptionsFileManagerFactory, AzureHDInsightSubscriptionsFileManagerSimulatorFactory>();
            runManager.RegisterType<IRemoteHadoopJobSubmissionPocoClientFactory, HadoopJobSubmissionPocoSimulatorClientFactory>();
            runManager.RegisterType<IHDInsightJobSubmissionPocoClientFactory, HadoopJobSubmissionPocoSimulatorClientFactory>();
            runManager.RegisterType<IAzureHDInsightConnectionSessionManagerFactory, AzureHDInsightConnectionSessionManagerSimulatorFactory>();
            runManager.RegisterType<IBufferingLogWriterFactory, BufferingLogWriterFactory>();

            var testManager = new IntegrationTestManager();
            if (!testManager.RunAzureTests())
            {
                Assert.Inconclusive("Azure tests are not configured on this machine.");
            }
            IntegrationTestBase.TestCredentials = testManager.GetCredentials("default");
            if (IntegrationTestBase.TestCredentials == null)
            {
                Assert.Inconclusive("No entry was found in the credential config file for the specified test configuration.");
            }

            // Sets the certificate
            var defaultCertificate = new X509Certificate2(IntegrationTestBase.TestCredentials.Certificate); ;


            // Sets the test static properties
            IntegrationTestBase.ClusterPrefix = string.Format("CLITest-{0}", Environment.GetEnvironmentVariable("computername") ?? "unknown");

            // Sets the credential objects
            var tempCredentials = new HDInsightCertificateCredential()
            {
                SubscriptionId = TestCredentials.SubscriptionId,
                Certificate = defaultCertificate
            };
            IntegrationTestBase.validCredentials = ServiceLocator.Instance
                                                .Locate<IHDInsightSubscriptionCertificateCredentialsFactory>()
                                                .Create(tempCredentials);
            tempCredentials = new HDInsightCertificateCredential() { SubscriptionId = Guid.NewGuid(), Certificate = defaultCertificate };
            IntegrationTestBase.invalidSubscriptionId = ServiceLocator.Instance
                                                     .Locate<IHDInsightSubscriptionCertificateCredentialsFactory>()
                                                     .Create(tempCredentials);
            tempCredentials = new HDInsightCertificateCredential()
            {
                SubscriptionId = TestCredentials.SubscriptionId,
                Certificate = new X509Certificate2(TestCredentials.InvalidCertificate)
            };
            IntegrationTestBase.invalidCertificate = ServiceLocator.Instance
                                                  .Locate<IHDInsightSubscriptionCertificateCredentialsFactory>()
                                                  .Create(tempCredentials);

            // Prepares the environment 
            IntegrationTestBase.CleanUpClusters();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Need secure string for pscredential object.")]
        protected static PSCredential GetPSCredential(string userName, string password)
        {
            var securePassword = new SecureString();

            foreach (var character in password)
            {
                securePassword.AppendChar(character);
            }

            return new PSCredential(userName, securePassword);
        }

        private IRunspace runspace;

        protected IRunspace GetPowerShellRunspace()
        {
            if (this.runspace.IsNull())
            {
                var loc = typeof(GetAzureHDInsightClusterCmdlet).Assembly.Location;
                this.runspace = Help.SafeCreate(() => RunspaceAbstraction.Create());
                this.runspace.NewPipeline()
                             .AddCommand("Import-Module")
                             .WithParameter("Name", loc)
                             .Invoke();
            }
            return this.runspace;
        }

        public void ApplyIndividualTestMockingOnly()
        {
            HDInsightClient.DefaultPollingInterval = TimeSpan.FromMinutes(1);
            var manager = ServiceLocator.Instance.Locate<IServiceLocationSimulationManager>();
            manager.MockingLevel = ServiceLocationMockingLevel.ApplyIndividualTestMockingOnly;
        }

        public void ResetIndividualMocks()
        {
            var manager = ServiceLocator.Instance.Locate<IServiceLocationIndividualTestManager>();
            manager.Reset();
        }

        public void ApplySimulatorMockingOnly()
        {
            var manager = ServiceLocator.Instance.Locate<IServiceLocationSimulationManager>();
            manager.MockingLevel = ServiceLocationMockingLevel.ApplyTestRunMockingOnly;
        }

        public void ApplyFullMocking()
        {
            var manager = ServiceLocator.Instance.Locate<IServiceLocationSimulationManager>();
            manager.MockingLevel = ServiceLocationMockingLevel.ApplyFullMocking;
        }

        public void ApplyNoMocking()
        {
            HDInsightClient.DefaultPollingInterval = TimeSpan.FromMinutes(1);
            var manager = ServiceLocator.Instance.Locate<IServiceLocationSimulationManager>();
            manager.MockingLevel = ServiceLocationMockingLevel.ApplyNoMocking;
        }

        public void SetHDInsightManagementRestSimulatorClientOperationTime(int milliseconds)
        {
            HDInsightManagementRestSimulatorClient.OperationTimeToCompletionInMilliseconds = milliseconds;
        }

        protected static PSCredential GetAzurePsCredentials()
        {
            return GetPSCredential(IntegrationTestBase.TestCredentials.AzureUserName, IntegrationTestBase.TestCredentials.AzurePassword);
        }

        public static IHDInsightCertificateCredential GetValidCredentials()
        {
            return validCredentials;
        }

        protected static IHDInsightCertificateCredential GetInvalidSubscriptionIdCredentials()
        {
            return invalidSubscriptionId;
        }

        protected static IHDInsightCertificateCredential GetInvalidCertificateCredentials()
        {
            return invalidCertificate;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Azure names must be lowercase.")]
        public string GetRandomClusterName()
        {
            // Random DNS name.
            var time = DateTime.UtcNow;
            var machineName = Environment.GetEnvironmentVariable("computername") ?? "unknown";
            var retval = string.Format("{0}-{1}{2}{3}-{4}",
                                       ClusterPrefix,
                                       time.Month.ToString("00"),
                                       time.Day.ToString("00"),
                                       time.Hour.ToString("00"),
                                       Guid.NewGuid().ToString("N")).ToLowerInvariant();
            testToClusterMap.Add(retval, System.Environment.StackTrace.ToString());
            return retval;
        }

        public string GetRandomValidPassword()
        {
            return Guid.NewGuid().ToString().ToUpperInvariant().Replace('A', 'a').Replace('B', 'b').Replace('C', 'c') + "forTest!";
        }

        public ClusterCreateParameters GetRandomCluster()
        {
            // Creates the cluster
            return new ClusterCreateParameters
            {
                Name = this.GetRandomClusterName(),
                UserName = TestCredentials.AzureUserName,
                Password = this.GetRandomValidPassword(),
                Location = "East US 2",
                Version = "default",
                DefaultStorageAccountName = TestCredentials.Environments[0].DefaultStorageAccount.Name,
                DefaultStorageAccountKey = TestCredentials.Environments[0].DefaultStorageAccount.Key,
                DefaultStorageContainer = TestCredentials.Environments[0].DefaultStorageAccount.Container,
                ClusterSizeInNodes = 3
            };
        }

        protected static void CleanUpClusters()
        {
            var credentials = IntegrationTestBase.GetValidCredentials();
            using (var client = ServiceLocator.Instance.Locate<IHDInsightManagementPocoClientFactory>().Create(credentials, GetAbstractionContext()))
            {
                var clusters = client.ListContainers();
                clusters.WaitForResult();
                foreach (var cluster in clusters.Result.Where(c => c.Name.StartsWith(ClusterPrefix)))
                {
                    client.DeleteContainer(cluster.Name, cluster.Location).WaitForResult();
                }
            }
        }

        protected static void DeleteClusters(IHDInsightCertificateCredential credentials, string location)
        {
            var client = HDInsightClient.Connect(new HDInsightCertificateCredential(credentials.SubscriptionId, credentials.Certificate));
            var clusters = client.ListClusters().Where(cluster => cluster.Location == location).ToList();

            Parallel.ForEach(clusters, cluster => client.DeleteCluster(cluster.Name));
        }

        public static HDInsight.IAbstractionContext GetAbstractionContext()
        {
            return Help.SafeCreate(() => new AbstractionContext(CancellationToken.None));
        }

        protected static void DeleteClustersWithVersion(IHDInsightCertificateCredential credentials, string version)
        {
            var client = HDInsightClient.Connect(new HDInsightCertificateCredential(credentials.SubscriptionId, credentials.Certificate));
            var clusters = client.ListClusters().Where(cluster => cluster.Version == version).ToList();

            Parallel.ForEach(clusters, cluster => client.DeleteCluster(cluster.Name));
        }
    }
}