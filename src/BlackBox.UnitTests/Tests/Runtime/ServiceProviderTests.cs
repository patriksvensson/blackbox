//
// Copyright 2011 Patrik Svensson
//
// This file is part of BlackBox.
//
// BlackBox is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BlackBox is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License
// along with BlackBox. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Runtime
{
    [TestFixture]
    public class ServiceProviderTests
    {
        #region Private Nested Test Classes
        private class Service : IService
        {
            public string Value { get; set; }
            public Service(string value) { this.Value = value; }
        }
        private interface IService
        {
            string Value { get; }
        }
        #endregion

        [Test]
        public void ServiceProvider_Register()
        {
            ServiceProvider provider = new ServiceProvider();
            provider.Register<Service>(new Service("service"));
            provider.Register<Service, IService>(new Service("iservice"));
            Assert.IsTrue(provider.Services.ContainsKey(typeof(Service)));
            Assert.IsNotNull(provider.Services[typeof(Service)]);
            Assert.IsInstanceOf<Service>(provider.Services[typeof(Service)]);
            Assert.IsTrue(provider.Services.ContainsKey(typeof(IService)));
            Assert.IsNotNull(provider.Services[typeof(IService)]);
            Assert.IsInstanceOf<Service>(provider.Services[typeof(IService)]);
        }

        [Test]
        public void ServiceProvider_Unregister()
        {
            ServiceProvider provider = new ServiceProvider();
            provider.Register<Service>(new Service("service"));
            provider.Register<Service, IService>(new Service("iservice"));
            provider.Unregister<Service>();
            provider.Unregister<IService>();            
            Assert.IsFalse(provider.Services.ContainsKey(typeof(Service)));
            Assert.IsFalse(provider.Services.ContainsKey(typeof(IService)));
        }

        [Test]
        public void ServiceProvider_Resolve()
        {
            ServiceProvider provider = new ServiceProvider();
            provider.Register<Service>(new Service("service"));
            provider.Register<Service, IService>(new Service("iservice"));
            Service service = provider.Resolve<Service>();
            Assert.IsNotNull(service);
            Assert.AreEqual("service", service.Value);
            IService iservice = provider.Resolve<IService>();
            Assert.IsNotNull(iservice);
            Assert.AreEqual("iservice", iservice.Value);
        }

        [Test]
        public void ServiceProvider_GetService()
        {
            ServiceProvider provider = new ServiceProvider();
            provider.Register<Service>(new Service("service"));
            provider.Register<Service, IService>(new Service("iservice"));
            Service service = provider.GetService(typeof(Service)) as Service;
            Assert.IsNotNull(service);
            Assert.AreEqual("service", service.Value);
            IService iservice = provider.GetService(typeof(IService)) as IService;
            Assert.IsNotNull(iservice);
            Assert.AreEqual("iservice", iservice.Value);
        }
    }
}
