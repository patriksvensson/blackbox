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

using BlackBox.Formatting;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public class FormatPatternFactoryTests
	{
		[Test]
		public void FormatPatternFactory_BuildPatternWithLiteral()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("Hello World!");
			Assert.IsNotNull(pattern);
			Assert.IsNotNull(pattern.Renderers);
			Assert.AreEqual(1, pattern.Renderers.Length);
			Assert.IsInstanceOf<LiteralRenderer>(pattern.Renderers[0]);
			Assert.AreEqual("Hello World!", ((LiteralRenderer)pattern.Renderers[0]).Literal);
		}

		[Test]
		public void FormatPatternFactory_BuildPatternWithRenderer()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(time(format='HH:mm:ss'))");
			Assert.IsNotNull(pattern);
			Assert.IsNotNull(pattern.Renderers);
			Assert.AreEqual(1, pattern.Renderers.Length);
			Assert.IsInstanceOf<TimeRenderer>(pattern.Renderers[0]);
			Assert.AreEqual("HH:mm:ss", ((TimeRenderer)pattern.Renderers[0]).Format);
		}

		[Test]
		[ExpectedException(typeof(FormatPatternException), ExpectedMessage = "Could not resolve renderer 'nonexistent'.")]
		public void FormatPatternFactory_BuildPatternWithRendererThatDoNotExist()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(nonexistent(format='HH:mm:ss'))");
		}

		[Test]
		public void FormatPatternFactory_BuildPatternWithTransformer()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(uppercase(message()))");
			Assert.IsNotNull(pattern);
			Assert.IsNotNull(pattern.Renderers);
			Assert.AreEqual(1, pattern.Renderers.Length);
			Assert.IsInstanceOf<UppercaseTransformer>(pattern.Renderers[0]);
			Assert.IsInstanceOf<MessageRenderer>(((UppercaseTransformer)pattern.Renderers[0]).Renderer);
		}

		[Test]
		[ExpectedException(typeof(FormatPatternException), ExpectedMessage = "Could not resolve renderer 'nonexistent'.")]
		public void FormatPatternFactory_BuildPatternWithNestedRendererThatDoNotExist()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(uppercase(nonexistent()))");
		}

		[Test]
		[ExpectedException(typeof(FormatPatternException), ExpectedMessage = "Transformer 'uppercase' has no attached renderer.")]
		public void FormatPatternFactory_BuildPatternWithTransformerThatHasNoRenderer()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(nonexistent(uppercase()))");
		}

		[Test]
		[ExpectedException(typeof(FormatPatternException), ExpectedMessage = "Could not resolve transformer 'nonexistent'.")]
		public void FormatPatternFactory_BuildPatternWithTransformerThatDoNotExist()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(nonexistent(message()))");
		}

		[Test]
		public void FormatPatternFactory_BuildPatternWithNestedTransformers()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("$(uppercase(rot13(message())))");
			Assert.IsNotNull(pattern);
			Assert.IsNotNull(pattern.Renderers);
			Assert.AreEqual(1, pattern.Renderers.Length);
			Assert.IsInstanceOf<UppercaseTransformer>(pattern.Renderers[0]);
			Assert.IsInstanceOf<Rot13Transformer>(((UppercaseTransformer)pattern.Renderers[0]).Renderer);
			Assert.IsInstanceOf<MessageRenderer>(((Rot13Transformer)((UppercaseTransformer)pattern.Renderers[0]).Renderer).Renderer);
		}

		[Test]
		public void FormatPatternFactory_BuildComplexPattern()
		{
			var factory = new FormatPatternFactory();
			var pattern = factory.Create("[$(time())] $(uppercase(rot13(message())))");
			Assert.IsNotNull(pattern);
			Assert.IsNotNull(pattern.Renderers);
			Assert.AreEqual(4, pattern.Renderers.Length);
			Assert.IsInstanceOf<LiteralRenderer>(pattern.Renderers[0]);
			Assert.IsInstanceOf<TimeRenderer>(pattern.Renderers[1]);
			Assert.IsInstanceOf<LiteralRenderer>(pattern.Renderers[2]);
			Assert.IsInstanceOf<UppercaseTransformer>(pattern.Renderers[3]);
			Assert.IsInstanceOf<Rot13Transformer>(((UppercaseTransformer)pattern.Renderers[3]).Renderer);
			Assert.IsInstanceOf<MessageRenderer>(((Rot13Transformer)((UppercaseTransformer)pattern.Renderers[3]).Renderer).Renderer);
		}
	}
}
