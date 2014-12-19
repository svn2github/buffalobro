﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
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
    public partial class Get_AzureHDInsightClusterCmdletFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Get-AzureHDInsightClusterCmdlet.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Get-AzureHDInsightCluster Cmdlet", " In order to manage my HDInsight clusters on my Azure subscription\r\n As an IT pro" +
                    "fessional\r\n  I want to be able to execute a PowerShell command that returns the " +
                    "HDInsight clusters", ProgrammingLanguage.CSharp, new string[] {
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Get-AzureHDInsightCluster Cmdlet")))
            {
                Microsoft.WindowsAzure.Management.HDInsight.Tests.Gherkin.Get_AzureHDInsightClusterCmdletFeature.FeatureSetup(null);
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
        testRunner.And("I am using the \"Get-AzureHDInsightCluster\" PowerShell Cmdlet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("There Exists a Get-AzureHDInsightCluster PowerShell Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void ThereExistsAGet_AzureHDInsightClusterPowerShellCmdlet()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("There Exists a Get-AzureHDInsightCluster PowerShell Cmdlet", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 12
  testRunner.Then("There exists a \"Get-AzureHDInsightCluster\" PowerShell Cmdlet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("There is only 1 ParameterSet specified for the Get-AzureHDInsightCluster PowerShe" +
            "ll Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
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
  testRunner.Then("there exists a \"Cluster By Name (with Specific Subscription Credentials)\" paramet" +
                    "er set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 16
   testRunner.And("there exists no further parameter sets", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        public virtual void NoParameterSetHasTwoParametersInTheSameLocation(string parameterSetName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter set has two parameters in the same location", exampleTags);
#line 18
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 19
  testRunner.When(string.Format("I am using the \"{0}\" parameter set", parameterSetName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
  testRunner.Then("no parameter in the set shares the same position with another parameter from the " +
                    "set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter set has two parameters in the same location")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Cluster By Name (with Specific Subscription Credentials)")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:ParameterSetName", "Cluster By Name (with Specific Subscription Credentials)")]
        public virtual void NoParameterSetHasTwoParametersInTheSameLocation_ClusterByNameWithSpecificSubscriptionCredentials()
        {
            this.NoParameterSetHasTwoParametersInTheSameLocation("Cluster By Name (with Specific Subscription Credentials)", ((string[])(null)));
        }
        
        public virtual void NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline(string parameterSetName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter set has two parameters that accept their value from the pipeline", exampleTags);
#line 25
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 26
  testRunner.When(string.Format("I am using the \"{0}\" parameter set", parameterSetName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 27
  testRunner.Then("only one parameter in the set is set to take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter set has two parameters that accept their value from the pipeline")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Cluster By Name (with Specific Subscription Credentials)")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:ParameterSetName", "Cluster By Name (with Specific Subscription Credentials)")]
        public virtual void NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline_ClusterByNameWithSpecificSubscriptionCredentials()
        {
            this.NoParameterSetHasTwoParametersThatAcceptTheirValueFromThePipeline("Cluster By Name (with Specific Subscription Credentials)", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter in any set shares a name or alias with another")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void NoParameterInAnySetSharesANameOrAliasWithAnother()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter in any set shares a name or alias with another", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 33
  testRunner.Then("no parameter in any parameter set shares a name or alias with another parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("No parameter lacks either a Getter or a Setter")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void NoParameterLacksEitherAGetterOrASetter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No parameter lacks either a Getter or a Setter", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 36
  testRunner.Then("no parameter lacks either a Getter or a Setter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("I can use the \"Cluster By Name (with Specific Subscription Credentials)\" paramete" +
            "r set")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Get-AzureHDInsightCluster Cmdlet")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CheckIn")]
        public virtual void ICanUseTheClusterByNameWithSpecificSubscriptionCredentialsParameterSet()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can use the \"Cluster By Name (with Specific Subscription Credentials)\" paramete" +
                    "r set", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 39
  testRunner.When("I am using the \"Cluster By Name (with Specific Subscription Credentials)\" paramet" +
                    "er set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
  testRunner.Then("there exists a \"Name\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
   testRunner.And("it is of type \"String\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
   testRunner.And("it is an optional parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
   testRunner.And("it is specified as parameter 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
   testRunner.And("it can take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
   testRunner.And("there exists a \"SubscriptionId\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
   testRunner.And("it is of type \"Guid\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
   testRunner.And("it is specified as parameter 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
      testRunner.And("there exists a \"Certificate\" Cmdlet parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
   testRunner.And("it is of type \"X509Certificate2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
   testRunner.And("it is a required parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
   testRunner.And("it is specified as parameter 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
   testRunner.And("it can not take its value from the pipeline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
      testRunner.And("there are no additional parameters in the parameter set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
