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

namespace BlackBox.UnitTests.Tests.Extensions
{
    [TestFixture]
    public class AttributeExtensionsTests
    {
        #region Nested Classes
        #region Attribute Classes
        [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
        private sealed class TestBaseClassAttribute : Attribute
        {
            public string Value { get; set; }
            public TestBaseClassAttribute(string value)
            {
                this.Value = value;
            }
        }
        [AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
        private sealed class TestClassAttribute : Attribute
        {
            public string Value { get; set; }
            public TestClassAttribute(string value)
            {
                this.Value = value;
            }
        }
        #endregion
        #region Test Classes
        [TestClass("test0")]
        [TestBaseClass("testbase0")]
        private abstract class TestBaseClass
        {
        }
        [TestClass("test1")]
        [TestClass("test2")]
        private sealed class TestClass : TestBaseClass
        {
        }
        #endregion
        #endregion

        [Test]
        public void AttributeExtensions_GetSingleAttribute_NotInherited()
        {
            TestBaseClassAttribute attribute = typeof(TestClass).GetAttribute<TestBaseClassAttribute>(false);
            Assert.IsNull(attribute);
        }

        [Test]
        public void AttributeExtensions_GetSingleAttribute_Inherited()
        {
            TestBaseClassAttribute attribute = typeof(TestClass).GetAttribute<TestBaseClassAttribute>(true);
            Assert.IsNotNull(attribute);
        }

        [Test]
        public void AttributeExtensions_GetMultipleAttributes_NotInherited()
        {
            var attributes = typeof(TestClass).GetAttributes<TestClassAttribute>(false).ToArray();
            Assert.AreEqual(2, attributes.Length);
            Assert.AreEqual(1, attributes.Where(x => x.Value.Equals("test1")).Count());
            Assert.AreEqual(1, attributes.Where(x => x.Value.Equals("test2")).Count());
        }

        [Test]
        public void AttributeExtensions_GetMultipleAttributes_Inherited()
        {
            var attributes = typeof(TestClass).GetAttributes<TestClassAttribute>(true).ToArray();
            Assert.AreEqual(3, attributes.Length);
            Assert.AreEqual(1, attributes.Where(x => x.Value.Equals("test0")).Count());
            Assert.AreEqual(1, attributes.Where(x => x.Value.Equals("test1")).Count());
            Assert.AreEqual(1, attributes.Where(x => x.Value.Equals("test2")).Count());
        }
    }
}
