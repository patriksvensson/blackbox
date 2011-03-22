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
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using BlackBox.Formatting.Renderers;

namespace BlackBox.Formatting
{
	/// <summary>
	/// Factory class for creating format patterns.
	/// </summary>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	public sealed class FormatPatternFactory<TContext>
	{
		private readonly FormatRendererTypeMap<TContext> _typeMap;

		internal FormatPatternFactory()
			: this(null)
		{
		}

		internal FormatPatternFactory(FormatRendererTypeMap<TContext> typeMap)
		{
			_typeMap = typeMap ?? new FormatRendererTypeMap<TContext>();
		}

		/// <summary>
		/// Creates a format pattern from the specified string pattern.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <returns></returns>
		public FormatPattern<TContext> Create(string pattern)
		{
			FormatPatternNode[] nodes = FormatPatternParser.Parse(pattern);
			List<FormatRenderer<TContext>> result = new List<FormatRenderer<TContext>>();
			foreach (FormatPatternNode node in nodes)
			{
				// Is this a literal node?
				LiteralNode literalNode = node as LiteralNode;
				if (literalNode != null)
				{
					// Create the literal renderer.
					var literal = this.CreateLiteral(literalNode);
					result.Add(literal);
					continue;
				}

				// Is this a renderer node?
				FormatRendererNode rendererNode = node as FormatRendererNode;
				if (rendererNode != null)
				{
					// Create the format renderer.
					var renderer = this.CreateRenderer(rendererNode);
					result.Add(renderer);
					continue;
				}

				// No other node types are handled at this level.
				throw new FormatPatternException("Unhandled node type '{0}'.", node.GetType().FullName);
			}
			return new FormatPattern<TContext>(pattern, result.ToArray());
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private FormatRenderer<TContext> CreateLiteral(LiteralNode node)
		{
			return new LiteralRenderer<TContext>(node.Literal);
		}

		private FormatRenderer<TContext> CreateRenderer(FormatRendererNode node)
		{
			if(node == null)
			{
				throw new InvalidOperationException("Encountered empty renderer node.");
			}

			FormatRenderer<TContext> renderer;
			FormatTransformerNode transformerNode = node as FormatTransformerNode;

			if (transformerNode != null)
			{
				// Create the child renderer.
				var childRenderer = this.CreateRenderer(transformerNode.Renderer);
				if (childRenderer == null)
				{
					// We could not create the child renderer.
					throw new FormatPatternException("Could not create child renderer node '{0}'.", (transformerNode.Renderer.Name));
				}

				// Get the type of the transformer from the type map.
				Type transformerType = _typeMap.GetRendererType(node.Name);
				if (transformerType == null)
				{
					// We could not find the transformer.
					throw new FormatPatternException("Could not find transformer '{0}' for context type '{1}'.", node.Name, typeof(TContext).FullName);
				}

				// If this is a generic transformer?
				if (transformerType.IsGenericType && transformerType.ContainsGenericParameters)
				{
					// Create a generic type of it with the context as argument.
					transformerType = transformerType.MakeGenericType(typeof(TContext));
				}

				// Create the transformer.
				renderer = Activator.CreateInstance(transformerType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
					null, new object[] { childRenderer }, null) as FormatRenderer<TContext>;
			}
			else
			{
				// Get the renderer type.
				Type rendererType = _typeMap.GetRendererType(node.Name);
				if (rendererType == null)
				{
					// We could not find the renderer.
					throw new FormatPatternException("Could not find renderer '{0}' for context type '{1}'.", node.Name, typeof(TContext).FullName);
				}

				// Is this a generic renderer?
				if (rendererType.IsGenericType && rendererType.ContainsGenericParameters)
				{
					// Make a generic type with the context.
					rendererType = rendererType.MakeGenericType(typeof(TContext));
				}

				// Create the renderer.
				renderer = Activator.CreateInstance(rendererType, true) as FormatRenderer<TContext>;
			}

			// Map all arguments.
			this.MapArguments(renderer, node.Arguments);

			// Return the transformer.
			return renderer;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private void MapArguments(FormatRenderer<TContext> renderer, IEnumerable<FormatArgumentNode> arguments)
		{
			if (!arguments.Any())
			{
				return;
			}

			// Get identifier to property mappings.
			var mappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyInfo property in renderer.GetType().GetPublicProperties())
			{
				if (!mappings.ContainsKey(property.Name))
				{
					mappings.Add(property.Name, property);
				}
			}

			// Iterate through all keys in the argument dictionary.
			foreach (FormatArgumentNode argument in arguments)
			{
				// Got a property mapping?
				if (mappings.ContainsKey(argument.Name))
				{
					// Get the property's type.
					Type propertyType = mappings[argument.Name].PropertyType;

					// Get the type converter.
					var typeConverter = TypeDescriptor.GetConverter(propertyType);
					if (typeConverter != null)
					{
						object value = typeConverter.ConvertFromInvariantString(argument.Value);
						mappings[argument.Name].SetValue(renderer, value, null);
					}
				}
			}
		}
	}
}
