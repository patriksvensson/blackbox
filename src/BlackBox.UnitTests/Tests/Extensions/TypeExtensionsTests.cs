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
using System.Reflection;

namespace BlackBox.UnitTests.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        #region Private Nested Test Classes
        private abstract class TestBaseClass
        {
            public string StringValue { get; set; }
            private float HiddenFloatValue { get; set; }
        }
        private class TestClass : TestBaseClass
        {
            public int IntegerValue { get; set; }
            public decimal DecimalValue { get; set; }
            private DateTime HiddenDateTimeValue { get; set; }
        }
        private class TestClass2 : TestBaseClass
        {
        }
        private class GenericTestBaseClass<T>
        {
        }
        private class GenericTestClass : GenericTestBaseClass<int>
        {
        }
        #endregion

        [Test]
        public void TypeExtensions_IsNonAbstractClass()
        {
            Assert.IsTrue(typeof(TestClass).IsNonAbstractClass());
            Assert.IsFalse(typeof(TestBaseClass).IsNonAbstractClass());
        }

        [Test]
        public void TypeExtensions_Inherits()
        {
            Assert.IsTrue(typeof(TestClass).Inherits<TestBaseClass>());
            Assert.IsTrue(typeof(TestClass).Inherits(typeof(TestBaseClass)));
            Assert.IsFalse(typeof(TestClass2).Inherits<TestClass>());
            Assert.IsFalse(typeof(TestClass2).Inherits(typeof(TestClass)));
        }

        [Test]
        public void TypeExtensions_InheritsGenericType()
        {
            Assert.IsTrue(typeof(GenericTestClass).InheritsGenericType(typeof(GenericTestBaseClass<>)));
            Assert.IsFalse(typeof(TestClass).InheritsGenericType(typeof(GenericTestBaseClass<>)));
        }

        [Test]
        public void TypeExtensions_GetPublicProperties()
        {
            PropertyInfo[] properties = typeof(TestClass).GetPublicProperties();
            Assert.AreEqual(3, properties.Count());
        }

        [Test]
        public void TypeExtensions_GetAllProperties()
        {
            PropertyInfo[] properties = typeof(TestClass).GetAllProperties();
            Assert.AreEqual(5, properties.Count());
        }
    }
}
