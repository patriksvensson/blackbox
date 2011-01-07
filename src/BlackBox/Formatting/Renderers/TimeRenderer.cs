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
using System.Globalization;

namespace BlackBox.Formatting.Renderers
{
    [FormatRendererType("time")]
    internal sealed class TimeRenderer : FormatRenderer<ILogEntry>
    {
        public string Format { get; set; }
        public CultureInfo Culture { get; set; }

        internal TimeRenderer()
        {
            this.Format = "HH:mm:ss.fff";
            this.Culture = CultureInfo.InvariantCulture;
        }

        public override string Render(ILogEntry entry)
        {
            if (entry == null)
            {
                return string.Empty;
            }
            return entry.Timestamp.ToString(this.Format, this.Culture);
        }
    }
}
