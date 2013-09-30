﻿// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using OutcoldSolutions.ConfigTransformationTool.ExtendedTranforms;

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    using System;
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class TransformationTaskSpec : BaseSpec
    {
        [Test]
        public void Transfarmotaion_Should_Happend()
        {
            const string Source = @"<?xml version=""1.0""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >

	<custom>
		<groups>
			<group name=""TestGroup1"">
				<values>
					<value key=""Test1"" value=""True"" />
					<value key=""Test2"" value=""600"" />
				</values>
			</group>

			<group name=""TestGroup2"">
				<values>
					<value key=""Test3"" value=""True"" />
				</values>
			</group>

		</groups>
	</custom>
	
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	
	<custom>
		<groups>
			<group name=""TestGroup1"">
				<values>
					<value key=""Test2"" value=""601"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
				</values>
			</group>
		</groups>
	</custom>
	
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend.config");
            string transformFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: false);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile);

            // Check that transformation happend
            Assert.IsTrue(fileContent.Contains(@"value=""601"""));
        }

        [Test]
        public void TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines()
        {
            const string Source = @"<?xml version=""1.0""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWillBeReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"" xdt:Transform=""Replace""  xdt:Locator=""Match(name)"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Expected = @"<?xml version=""1.0""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            var sut = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true
            };

            // Act
            var actualResult = sut.Execute(resultFile);
            var actualContents = File.ReadAllText(resultFile);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(Expected, actualContents);
        }

        [Test]
        public void TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration()
        {
            const string Source = @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWillBeReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"" xdt:Transform=""Replace""  xdt:Locator=""Match(name)"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            var sut = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true
            };

            // Act
            var actualResult = sut.Execute(resultFile);
            var actualContents = File.ReadAllText(resultFile);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(Expected, actualContents);
        }

        [Test]
        public void TransfarmotaionWithNewLineInValue_ShouldKeepNewLine()
        {
            const string Source = @"<?xml version=""1.0""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >

	<value key=""Test1"" value=""Tru
e"" />

</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	
	<value key=""Test1"" value=""60
1&lt;group name=&quot;&quot;"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
	
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineInValue_ShouldKeepNewLine.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineInValue_ShouldKeepNewLine_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineInValue_ShouldKeepNewLine_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile);

            Assert.IsTrue(fileContent.Contains(string.Format(@"value=""60{0}1&lt;group name=&quot;&quot;""", Environment.NewLine)));
        }

        [Test]
        public void TransfarmotaionWithChineseCharaters_ShouldTransform()
        {
            const string Source = @"<?xml version=""1.0"" encoding=""utf-8""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >
	<custom key=""Test"" value="""" />
</configuration>";

            const string Transform = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	<custom key=""Test"" value=""倉頡; 仓颉"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithChineseCharaters_ShouldTransform.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithChineseCharaters_ShouldTransform_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithChineseCharaters_ShouldTransform_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);

            Assert.IsTrue(fileContent.Contains(@"value=""倉頡; 仓颉"""));
        }

        [Test]
        public void Issue442278()
        {
            const string Source = @"<?xml version=""1.0""?>
<connectionStrings>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
</connectionStrings>";

            const string Transform = @"<?xml version=""1.0""?>
<connectionStrings xdt:Transform=""Replace"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
        <a>aaa</a>
        <b>bbb</b>
        <c>ccc</c>
</connectionStrings>";

            const string Result = @"<?xml version=""1.0""?>
<connectionStrings>
    <a>aaa</a>
    <b>bbb</b>
    <c>ccc</c>
</connectionStrings>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Issue442278.config");
            string transformFile = Path.Combine(baseDirectory, "Issue442278_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Issue442278_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true
            };

            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);

            Assert.AreEqual(Result, fileContent);
        }

        [Test]
        public void Issue442278_CanSpecifyIndentChars()
        {
            const string Source = @"<?xml version=""1.0""?>
<connectionStrings>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
</connectionStrings>";

            const string Transform = @"<?xml version=""1.0""?>
<connectionStrings xdt:Transform=""Replace"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
        <a>aaa</a>
        <b>bbb</b>
        <c>ccc</c>
</connectionStrings>";

            const string Result = @"<?xml version=""1.0""?>
<connectionStrings>
  <a>aaa</a>
  <b>bbb</b>
  <c>ccc</c>
</connectionStrings>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Issue442278.config");
            string transformFile = Path.Combine(baseDirectory, "Issue442278_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Issue442278_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true,
                IndentChars = "  "
            };

            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);

            Assert.AreEqual(Result, fileContent);
        }

        [Test]
        public void Merge_SingleElementAbsent()
        {
            string source = El("A").MakeString();
            string transform = Trans("A", El("Child", At("xdt:Transform", "Merge"))).MakeString();
            string expected = El("A", El("Child")).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_SingleElementPresent()
        {
            string source = El("A", El("Child")).MakeString();
            string transform = Trans("A", El("Child", At("xdt:Transform", "Merge"))).MakeString();
            string expected = El("A", El("Child")).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_ExistingElementHasDifferentNamespace_AnotherElementCreated()
        {
            string source = El("A", MyNs("a"), El("a:Child")).MakeString();
            string transform = Trans("A", El("Child", At("xdt:Transform", "Merge"))).MakeString();
            string expected = El("A", MyNs("a"), El("a:Child"), El("Child")).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_WithSubchildren()
        {
            string source = El("A").MakeString();
            string transform = Trans("A",
                                     El("Child", At("xdt:Transform", "Merge"),
                                        El("Subchild"))).MakeString();
            string expected = El("A", El("Child", El("Subchild"))).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_WithOtherTransformationSpecifiedForItsChildAndElementsExist_OtherTransformExecuted()
        {
            string source = El("A",
                               El("Child",
                                  El("Subchild"))).MakeString();
            string transform = Trans("A",
                                     El("Child", At("xdt:Transform", "Merge"),
                                        El("Subchild", At("my-attr", "my-value"), At("xdt:Transform", "SetAttributes(my-attr)")))).MakeString();
            string expected = El("A",
                                 El("Child",
                                    El("Subchild", At("my-attr", "my-value")))).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_TransformSpecifiesSomNonKeyAttributes_AttributesAreMergedCorrectly()
        {
            string source = El("A",
                               El("Child", At("key", "num1"), At("old", "old-value"))).MakeString();
            string transform = Trans("A",
                                     El("Child", At("key", "num1"), At("new", "new-value"), At("xdt:Transform", "Merge(key)"))).MakeString();
            string expected = El("A",
                                 El("Child", At("key", "num1"), At("old", "old-value"), At("new", "new-value"))).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_SeveralChildsWithTheNameExists_MergeRespectsKeyAttributes()
        {
            string source = El("A",
                               El("Child", At("key", "num1")),
                               El("Child", At("key", "num2")),
                               El("Child", At("key", "num3"))).MakeString();
            string transform = Trans("A",
                                     El("Child", At("key", "num2"), At("new", "new-value"), At("xdt:Transform", "Merge(key)"))).MakeString();
            string expected = El("A",
                               El("Child", At("key", "num1")),
                               El("Child", At("key", "num2"), At("new", "new-value")),
                               El("Child", At("key", "num3"))).MakeString();
            Check(source, transform, expected);
        }

        [Test]
        public void Merge_ArgumentAttributeNotPresent_Error()
        {
            string source = El("A", El("Child")).MakeString();
            string transform = Trans("A", El("Child", At("xdt:Transform", "Merge(x)"))).MakeString();
            Check(source, transform, string.Empty, expectSuccess: false);
        }

        [Test]
        public void Transform_IdentCharsNotSpecified_SilentlyAppliesDefaultIndentChars()
        {
            string source = El("A").MakeString();
            string transform = Trans("A", El("Child", At("xdt:Transform", "Merge"))).MakeString();
            string expected = El("A", El("Child").WithIndent(4)).MakeString();
            Check(source, transform, expected, identChars: null);
        }

        [Test]
        public void AttributeRegexReplace_BasicCase()
        {
            string source = El("A", At("a", "aaa-bbb-ccc")).MakeString();
            string transform = Trans("A", At("xdt:Transform", "AttributeRegexReplace(Attribute='a', Pattern='(a+)-bbb-(c{3})', Replacement='$1-REPLACED-$2')")).MakeString();
            string expected = El("A", At("a", "aaa-REPLACED-ccc")).MakeString();
            Check(source, transform, expected, identChars: null);
        }

        private void Check(string source, string transform, string expected, bool expectSuccess = true, string identChars = "  ", [CallerMemberName] string testName = "")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, testName + ".source.xml");
            string transformFile = Path.Combine(baseDirectory, testName + ".transform.xml");
            string resultFile = Path.Combine(baseDirectory, testName + ".result.xml");

            // Create source file
            this.WriteToFile(sourceFile, source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true,
                IndentChars = identChars
            };

            bool success = task.Execute(resultFile);
            Assert.AreEqual(expectSuccess, success);
            if (expectSuccess)
            {
                string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);
                Assert.AreEqual(expected, fileContent);
            }
        }

        private XmlElementCtor El(string name, params XmlNodeCtor[] content)
        {
            return new XmlElementCtor(name, content);
        }

        private XmlElementCtor Trans(string name, params XmlNodeCtor[] content)
        {
            return new XmlElementCtor(name, new XmlNodeCtor[] {XdtNs(), XdtImport()}.Concat(content).ToArray());
        }

        private XmlAttributeCtor At(string name, string value)
        {
            return new XmlAttributeCtor(name, value);
        }

        private XmlAttributeCtor XdtNs()
        {
            return new XmlAttributeCtor("xmlns:xdt", "http://schemas.microsoft.com/XML-Document-Transform");
        }

        private XmlAttributeCtor MyNs(string id)
        {
            return new XmlAttributeCtor("xmlns:" + id, "http://my-ns.org/" + id);
        }

        private XmlElementCtor XdtImport()
        {
            return new XmlElementCtor("xdt:Import",
                                      At("assembly", "ctt.console"),
                                      At("namespace", typeof (Merge).Namespace));
        }
    }
}