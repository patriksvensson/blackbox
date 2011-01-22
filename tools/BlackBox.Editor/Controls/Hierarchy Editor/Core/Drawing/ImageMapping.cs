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
using System.Drawing;

namespace BlackBox.Editor
{
    public class ImageMapping
    {
        #region Private Fields

        private string m_key;
        private Image m_image;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return m_key; }
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image
        {
            get { return m_image; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMapping"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="image">The image.</param>
        public ImageMapping(string key, Image image)
        {
            m_key = key;
            m_image = image;
        }
    }
}
