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

using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Extensions
{
	[TestFixture]
	public class CharExtensionsTests
	{
		[Test]
		public void CharExtensions_IsWhiteSpace()
		{
			Assert.IsFalse('a'.IsWhitespace());
			Assert.IsFalse('z'.IsWhitespace());
			Assert.IsFalse('1'.IsWhitespace());
			Assert.IsTrue(' '.IsWhitespace());
			Assert.IsTrue('\t'.IsWhitespace());
		}

		[Test]
		public void CharExtensions_IsDigit()
		{
			Assert.IsFalse('a'.IsDigit());
			Assert.IsFalse('z'.IsDigit());
			Assert.IsTrue('0'.IsDigit());
			Assert.IsTrue('1'.IsDigit());
			Assert.IsTrue('2'.IsDigit());
			Assert.IsTrue('3'.IsDigit());
			Assert.IsTrue('4'.IsDigit());
			Assert.IsTrue('5'.IsDigit());
			Assert.IsTrue('6'.IsDigit());
			Assert.IsTrue('7'.IsDigit());
			Assert.IsTrue('8'.IsDigit());
			Assert.IsTrue('9'.IsDigit());
		}

		[Test]
		public void CharExtensions_IsLetter()
		{
			Assert.IsFalse('1'.IsLetter());
			Assert.IsFalse('*'.IsLetter());
			Assert.IsTrue('a'.IsLetter());
			Assert.IsTrue('b'.IsLetter());
			Assert.IsTrue('c'.IsLetter());
			Assert.IsTrue('d'.IsLetter());
			Assert.IsTrue('e'.IsLetter());
			Assert.IsTrue('f'.IsLetter());
			Assert.IsTrue('g'.IsLetter());
			Assert.IsTrue('h'.IsLetter());
			Assert.IsTrue('i'.IsLetter());
			Assert.IsTrue('j'.IsLetter());
			Assert.IsTrue('k'.IsLetter());
			Assert.IsTrue('l'.IsLetter());
			Assert.IsTrue('m'.IsLetter());
			Assert.IsTrue('n'.IsLetter());
			Assert.IsTrue('o'.IsLetter());
			Assert.IsTrue('p'.IsLetter());
			Assert.IsTrue('q'.IsLetter());
			Assert.IsTrue('r'.IsLetter());
			Assert.IsTrue('s'.IsLetter());
			Assert.IsTrue('t'.IsLetter());
			Assert.IsTrue('u'.IsLetter());
			Assert.IsTrue('v'.IsLetter());
			Assert.IsTrue('w'.IsLetter());
			Assert.IsTrue('x'.IsLetter());
			Assert.IsTrue('y'.IsLetter());
			Assert.IsTrue('z'.IsLetter());
			Assert.IsTrue('å'.IsLetter());
			Assert.IsTrue('ä'.IsLetter());
			Assert.IsTrue('ö'.IsLetter());
		}
	}
}
