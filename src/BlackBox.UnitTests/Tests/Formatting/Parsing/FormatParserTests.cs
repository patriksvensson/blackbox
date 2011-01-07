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
using BlackBox.Formatting;
using BlackBox.Formatting.Renderers;

namespace BlackBox.UnitTests.Tests.Formatting
{
    [TestFixture]
    public class FormatParserTests
    {
        [Test]
        public void FormatParser_ParseLiteral()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("Hello World");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<LiteralNode>(nodes[0]);
            Assert.AreEqual("Hello World", ((LiteralNode)nodes[0]).Literal);
        }

        [Test]
        public void FormatParser_ParseQuotedLiteral()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("'Hello World'");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<LiteralNode>(nodes[0]);
            Assert.AreEqual("'Hello World'", ((LiteralNode)nodes[0]).Literal);
        }

        [Test]
        public void FormatParser_ParseQuotedRenderer()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("'$(time())'");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(3, nodes.Length);
            Assert.IsInstanceOf<LiteralNode>(nodes[0]);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[1]);
            Assert.IsInstanceOf<LiteralNode>(nodes[2]);
            Assert.AreEqual("'", ((LiteralNode)nodes[0]).Literal);
            Assert.AreEqual("time", ((FormatRendererNode)nodes[1]).Name);
            Assert.AreEqual("'", ((LiteralNode)nodes[2]).Literal);
        }

        [Test]
        public void FormatParser_ParseRendererWithNoArguments()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(time())");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[0]);
            Assert.AreEqual(0, ((FormatRendererNode)nodes[0]).Arguments.Count);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FormatPatternException), ExpectedMessage = "Format renderer block is not well formed.")]
        public void FormatParser_ParseFaultyRenderer_MissingEndParenthesis()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(time()");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FormatPatternException), ExpectedMessage = "Not a valid renderer.")]
        public void FormatParser_ParseFaultyRenderer_RendererMissingParenthesises()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(time)");
        }

        [Test]
        public void FormatParser_ParseRendererWithSingleArgument()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(time(format='HH:mm:ss.fff'))");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[0]);
            Assert.AreEqual("time", ((FormatRendererNode)nodes[0]).Name);
            Assert.AreEqual(1, ((FormatRendererNode)nodes[0]).Arguments.Count);
            Assert.AreEqual("format", ((FormatRendererNode)nodes[0]).Arguments[0].Name);
            Assert.AreEqual("HH:mm:ss.fff", ((FormatRendererNode)nodes[0]).Arguments[0].Value);
        }

        [Test]
        public void FormatParser_ParseRendererWithMultipleArgument()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(time(format='HH:mm:ss.fff', utc=true))");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[0]);
            Assert.AreEqual("time", ((FormatRendererNode)nodes[0]).Name);
            Assert.AreEqual(2, ((FormatRendererNode)nodes[0]).Arguments.Count);
            Assert.AreEqual("format", ((FormatRendererNode)nodes[0]).Arguments[0].Name);
            Assert.AreEqual("HH:mm:ss.fff", ((FormatRendererNode)nodes[0]).Arguments[0].Value);
            Assert.AreEqual("utc", ((FormatRendererNode)nodes[0]).Arguments[1].Name);
            Assert.AreEqual("true", ((FormatRendererNode)nodes[0]).Arguments[1].Value);
        }

        public void FormatParser_ParseComplexWithEscapedBackSlash()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(basedir())\\$(time(format='yyyy-MM-dd'))-$(level()).log");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(6, nodes.Length);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[0]);
            Assert.IsInstanceOf<LiteralNode>(nodes[1]);
            Assert.AreEqual("\\", ((LiteralNode)nodes[1]).Literal);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[2]);
            Assert.IsInstanceOf<LiteralNode>(nodes[3]);
            Assert.AreEqual("-", ((LiteralNode)nodes[3]).Literal);
            Assert.IsInstanceOf<FormatRendererNode>(nodes[4]);
            Assert.IsInstanceOf<LiteralNode>(nodes[5]);
            Assert.AreEqual(".log", ((LiteralNode)nodes[5]).Literal);
        }

        [Test]
        public void FormatParser_ParseSingleTransformerWithOneRendererThatHasNoArguments()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(rot13(randomword()))");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<FormatTransformerNode>(nodes[0]);
            Assert.AreEqual("rot13", ((FormatTransformerNode)nodes[0]).Name);
            Assert.IsNotNull(((FormatTransformerNode)nodes[0]).Renderer);
            Assert.IsNotNull("randomword", ((FormatTransformerNode)nodes[0]).Renderer.Name);
        }

        [Test]
        public void FormatParser_ParseSingleTransformerWithSingleRendererThatHasSingleArgument()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(rot13(randomword(seed=128)))");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<FormatTransformerNode>(nodes[0]);
            Assert.AreEqual("rot13", ((FormatTransformerNode)nodes[0]).Name);
            Assert.IsNotNull(((FormatTransformerNode)nodes[0]).Renderer);
            Assert.IsNotNull("randomword", ((FormatTransformerNode)nodes[0]).Renderer.Name);
            Assert.AreEqual(1, ((FormatTransformerNode)nodes[0]).Renderer.Arguments.Count);
            Assert.AreEqual("seed", ((FormatTransformerNode)nodes[0]).Renderer.Arguments[0].Name);
            Assert.AreEqual("128", ((FormatTransformerNode)nodes[0]).Renderer.Arguments[0].Value);
        }

        [Test]
        public void FormatParser_ParseSingleTransformerWithSingleArgumentAndSingleRendererThatHasSingleArgument()
        {
            FormatPatternNode[] nodes = FormatPatternParser.Parse("$(rot13(randomword(seed=128), shift=13))");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Length);
            Assert.IsInstanceOf<FormatTransformerNode>(nodes[0]);
            Assert.AreEqual("rot13", ((FormatTransformerNode)nodes[0]).Name);
            Assert.AreEqual("shift", ((FormatTransformerNode)nodes[0]).Arguments[0].Name);
            Assert.AreEqual("13", ((FormatTransformerNode)nodes[0]).Arguments[0].Value);
            Assert.IsNotNull(((FormatTransformerNode)nodes[0]).Renderer);
            Assert.IsNotNull("randomword", ((FormatTransformerNode)nodes[0]).Renderer.Name);
            Assert.AreEqual(1, ((FormatTransformerNode)nodes[0]).Renderer.Arguments.Count);
            Assert.AreEqual("seed", ((FormatTransformerNode)nodes[0]).Renderer.Arguments[0].Name);
            Assert.AreEqual("128", ((FormatTransformerNode)nodes[0]).Renderer.Arguments[0].Value);
        }
    }
}
