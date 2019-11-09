﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class ConfigurationSourceFactoryFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanCreateAConfigurationSourceThatExistsInConfig()
        {
            using (var source = ConfigurationSourceFactory.Create("fileSource"))
            {
                Assert.AreEqual(typeof(FileConfigurationSource), source.GetType());
            }
        }

        [TestMethod]
        public void DefaultConfigurationSourceIsSystemSource()
        {
            using (var defaultSource = ConfigurationSourceFactory.Create())
            {
                Assert.AreEqual(typeof(SystemConfigurationSource), defaultSource.GetType());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequestForNullNameThrows()
        {
            ConfigurationSourceFactory.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void RequestForNonExistentNameThrows()
        {
            ConfigurationSourceFactory.Create("invalid");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void RequestForNameWithoutSectionThrows()
        {
            System.Configuration.Configuration configuration = ConfigurationTestHelper.GetConfigurationForCustomFile("test.exe.config");
            configuration.Sections.Remove(ConfigurationSourceSection.SectionName);
            configuration.Save();

            AppDomainSetup setupInfo = new AppDomainSetup();
            setupInfo.ConfigurationFile = configuration.FilePath;
            setupInfo.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            AppDomain newDomain = AppDomain.CreateDomain("test", null, setupInfo);

            try
            {
                ConfigurationSourceFactoryFixtureHelper helper =
                    (ConfigurationSourceFactoryFixtureHelper)newDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ConfigurationSourceFactoryFixtureHelper).FullName);

                helper.RequestForNameWithoutSectionThrows();
            }
            finally
            {
                AppDomain.Unload(newDomain);
            }
        }
    }

    public class ConfigurationSourceFactoryFixtureHelper : MarshalByRefObject
    {
        public void RequestForNameWithoutSectionThrows()
        {
            ConfigurationSourceFactory.Create("name");
        }
    }
}
