﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34003
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Microsoft.WindowsAzure.Management.HDInsight.Tests.Gherkin
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class New_AzureHDInsightClusterCmdletFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "New-AzureHDInsightClusterCmdlet.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "New-AzureHDInsightCluster Cmdlet", " In order to manage my HDInsight clusters on my Azure subscription\n As an IT prof" +
                    "essional\n  I want to be able to execute a PowerShell command that creates an HDI" +
                    "nsight cluster", ProgrammingLanguage.CSharp, new string[] {
                        "CheckIn"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "New-AzureHDInsightCluster Cmdlet")))
            {
                Microsoft.WindowsAzure.Management.HDInsight.Tests.Gherkin.New_AzureHDInsightClusterCmdletFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 7
#line 8
     testRunner.Given("I have installed the AzureHDInsight Cmdlets", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
       testRunner.When("I am using the \"New-AzureHDInsightCluster\" PowerShell Cmdlet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("There Exists a Remove-AzureHDInsightCluster PowerShell Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void ThereExistsARemove_AzureHDInsightClusterPowerShellCmdlet()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("There Exists a Remove-AzureHDInsightCluster PowerShell Cmdlet", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 12
  testRunner.Then("There exists a \"New-AzureHDInsightCluster\" PowerShell Cmdlet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("There is only 1 ParameterSet specified for the Get-AzureHDInsightCluster PowerShe" +
            "ll Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void ThereIsOnly1ParameterSetSpecifiedForTheGet_AzureHDInsightClusterPowerShellCmdlet()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("There is only 1 ParameterSet specified for the Get-AzureHDInsightCluster PowerShe" +
                    "ll Cmdlet", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 15
  testRunner.Then("there exists a \"Cluster By Name (with Specific Subscription Credential)\" paramete" +
                    "r set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 16
   testRunner.And("there exists a \"Cluster By Config (with Specific Subscription Credential)\" parame" +
                    "ter set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
   testRunner.And("there exists no further parameter sets", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        public virtual void NoParameterSetHasTwoParametersInTheSameLocation(string parameterSetName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter set has two parameters in the same location", exampleTags);
#line 19
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 20
   testRunner.And(string.Format("I am using the \"{0}\" parameter set", parameterSetName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.Then("no parameter in the set shares the same position with another parameter from the " +
                    "set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter set has two parameters in the same location")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Cluster By Name (with Specific Subscription Credential)")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:ParameterSetName", "Cluster By Name (with Specific Subscription Credential)")]
        public virtual void NoParameterSetHasTwoParametersInTheSameLocation_ClusterByNameWithSpecificSubscriptionCredential()
        {
            this.NoParameterSetHasTwoParametersInTheSameLocation("Cluster By Name (with Specific Subscription Credential)", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter set has two parameters in the same location")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Cluster By Config (with Specific Subscription Credential)")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:ParameterSetName", "Cluster By Config (with Specific Subscription Credential)")]
        public virtual void NoParameterSetHasTwoParametersInTheSameLocation_ClusterByConfigWithSpecificSubscriptionCredential()
        {
            this.NoParameterSetHasTwoParametersInTheSameLocation("Cluster By Config (with Specific Subscription Credential)", ((string[])(null)));
        }
        
        public virtual void NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline(string parameterSetName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter set has two parameters that accept their value from the pipeline", exampleTags);
#line 27
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 28
   testRunner.And(string.Format("I am using the \"{0}\" parameter set", parameterSetName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.Then("only one parameter in the set is set to take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter set has two parameters that accept their value from the pipeline")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Cluster By Name (with Specific Subscription Credential)")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:ParameterSetName", "Cluster By Name (with Specific Subscription Credential)")]
        public virtual void NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline_ClusterByNameWithSpecificSubscriptionCredential()
        {
            this.NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline("Cluster By Name (with Specific Subscription Credential)", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter set has two parameters that accept their value from the pipeline")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Cluster By Config (with Specific Subscription Credential)")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:ParameterSetName", "Cluster By Config (with Specific Subscription Credential)")]
        public virtual void NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline_ClusterByConfigWithSpecificSubscriptionCredential()
        {
            this.NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline("Cluster By Config (with Specific Subscription Credential)", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter in any set shares a name or alias with another")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void NoParameterInAnySetSharesANameOrAliasWithAnother()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter in any set shares a name or alias with another", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 36
  testRunner.Then("no parameter in any parameter set shares a name or alias with another parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter lacks either a Getter or a Setter")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void NoParameterLacksEitherAGetterOrASetter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter lacks either a Getter or a Setter", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 39
  testRunner.Then("no parameter lacks either a Getter or a Setter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("I can use the \"Cluster By Name (with Specific Subscription Credential)\" parameter" +
            " set")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void ICanUseTheClusterByNameWithSpecificSubscriptionCredentialParameterSet()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can use the \"Cluster By Name (with Specific Subscription Credential)\" parameter" +
                    " set", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 42
  testRunner.When("I am using the \"Cluster By Name (with Specific Subscription Credential)\" paramete" +
                    "r set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 43
  testRunner.Then("there exists a \"Name\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
   testRunner.And("it is specified as parameter 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
   testRunner.And("it can take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
   testRunner.And("there exists a \"Subscription\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
   testRunner.And("it is specified as parameter 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
      testRunner.And("there exists a \"Certificate\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
   testRunner.And("it is of type \"X509Certificate2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
   testRunner.And("it is specified as parameter 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
   testRunner.And("there exists a \"Location\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
   testRunner.And("it is specified as parameter 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
   testRunner.And("there exists a \"DefaultStorageAccountName\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
   testRunner.And("it is specified as parameter 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
   testRunner.And("there exists a \"DefaultStorageAccountKey\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
   testRunner.And("it is specified as parameter 5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
   testRunner.And("there exists a \"DefaultStorageContainerName\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
   testRunner.And("it is specified as parameter 6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
   testRunner.And("there exists a \"Credential\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
   testRunner.And("it is of type \"PSCredential\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
   testRunner.And("it is specified as parameter 7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
   testRunner.And("there exists a \"ClusterSizeInNodes\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
   testRunner.And("it is of type \"Int32\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
   testRunner.And("it is specified as parameter 9", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
      testRunner.And("there exists a \"EndPoint\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
   testRunner.And("it is of type \"Uri\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
   testRunner.And("it is specified as parameter 10", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
      testRunner.And("there exists a \"CloudServiceName\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
   testRunner.And("it is specified as parameter 11", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
   testRunner.And("there exists a \"Version\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
   testRunner.And("there are no additional parameters in the parameter set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("I can use the \"Cluster By Confg (with Specific Subscription Credential)\" paramete" +
            "r set")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void ICanUseTheClusterByConfgWithSpecificSubscriptionCredentialParameterSet()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can use the \"Cluster By Confg (with Specific Subscription Credential)\" paramete" +
                    "r set", ((string[])(null)));
#line 104
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 105
  testRunner.When("I am using the \"Cluster By Config (with Specific Subscription Credential)\" parame" +
                    "ter set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 106
  testRunner.Then("there exists a \"Name\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 107
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
   testRunner.And("it is specified as parameter 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
   testRunner.And("there exists a \"Config\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
   testRunner.And("it is of type \"AzureHDInsightConfig\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
   testRunner.And("it is specified as parameter 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
   testRunner.And("it can take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
   testRunner.And("there exists a \"Subscription\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
   testRunner.And("it is specified as parameter 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
      testRunner.And("there exists a \"Certificate\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
   testRunner.And("it is of type \"X509Certificate2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
   testRunner.And("it is specified as parameter 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
   testRunner.And("there exists a \"Location\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
   testRunner.And("it is specified as parameter 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
   testRunner.And("there exists a \"Credential\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
   testRunner.And("it is of type \"PSCredential\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
   testRunner.And("it is specified as parameter 5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
   testRunner.And("there exists a \"EndPoint\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
   testRunner.And("it is of type \"Uri\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
   testRunner.And("it is specified as parameter 7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
      testRunner.And("there exists a \"CloudServiceName\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
   testRunner.And("it is specified as parameter 8", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
   testRunner.And("there exists a \"Version\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 147
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
   testRunner.And("there are no additional parameters in the parameter set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
