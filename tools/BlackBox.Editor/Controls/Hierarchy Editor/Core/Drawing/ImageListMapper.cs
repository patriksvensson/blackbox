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
using System.Windows.Forms;

namespace BlackBox.Editor
{
    /// <summary>
    /// Class for mapping a key with an image in an image list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImageListMapper<T>
    {
        #region Private fields

        private Dictionary<string, int> m_dictionary;
        private ImageList m_imageList;
        private string m_defaultKey;

        #endregion

        #region Properties

        /// <summary>
        /// The image list.
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                // Return the image list.
                return m_imageList;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageListMapper()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imageList"></param>
        public ImageListMapper(ImageList imageList)
        {
            m_dictionary = new Dictionary<string, int>();
            m_imageList = imageList ?? new ImageList();
            m_imageList.ColorDepth = ColorDepth.Depth24Bit;
            m_defaultKey = string.Empty;
        }

        #endregion

        /// <summary>
        /// Sets the default key that will be used if
        /// an image can't be found for a specific key.
        /// </summary>
        /// <param name="key"></param>
        public void SetDefaultKey(T key)
        {
            // Get a unique string representation of the key.
            string keyString = this.GetKey(key);

            // Set the default key.
            m_defaultKey = keyString;
        }

        /// <summary>
        /// Sets the default key and image that will be used if
        /// an image can't be found for a specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="image"></param>
        public void SetDefaultKey(T key, Image image)
        {
            // Get a unique string representation of the key.
            string keyString = this.GetKey(key);

            // Add the image.
            this.AddImage(key, image);

            // Set the default key.
            m_defaultKey = keyString;
        }

        /// <summary>
        /// Gets the string representation of a key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetKey(T key)
        {
            // Get the string representation for the key.
            // This must be unique so we use the hash code.
            return key.GetHashCode().ToString();
        }

        /// <summary>
        /// Adds an image to the image list.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="image"></param>
        public void AddImage(T key, Image image)
        {
            // Get a unique string representation of the key.
            string keyString = this.GetKey(key);

            // Add the image to the image list.
            m_imageList.Images.Add(keyString, image);
            int index = m_imageList.Images.IndexOfKey(keyString);

            // Already in the dictionary?
            if (m_dictionary.ContainsKey(keyString))
            {
                m_dictionary.Remove(keyString);
            }

            // Add the image to the dictionary for faster access.
            m_dictionary.Add(keyString, index);
        }

        /// <summary>
        /// Removes an image from the image list.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveImage(T key)
        {
            // Get a unique string representation of the key.
            string keyString = this.GetKey(key);

            // Make sure there is an item with this key.
            if (!m_dictionary.ContainsKey(keyString))
                return;

            // Get the index of the item.
            int index = m_dictionary[keyString];

            // Remove the item.
            m_imageList.Images.RemoveAt(index);
        }

        /// <summary>
        /// Gets the image for a specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Image GetImage(T key)
        {
            // Get a unique string representation of the key.
            string keyString = this.GetKey(key);

            // Make sure there is an item with this key.
            if (!m_dictionary.ContainsKey(keyString))
                return null;

            // Get the index of the item.
            int index = m_dictionary[keyString];

            // Return the image.
            return m_imageList.Images[index];
        }

        /// <summary>
        /// Gets the image index for a specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetImageIndex(T key)
        {
            // Get a unique string representation of the key.
            string keyString = this.GetKey(key);

            // Make sure there is an item with this key.
            if (!m_dictionary.ContainsKey(keyString))
            {
                // No default key? If so, then exit.
                if (string.IsNullOrEmpty(m_defaultKey))
                    return -1;

                // Is there a default key in the dictionary?
                // If not, then exit.
                if (!m_dictionary.ContainsKey(m_defaultKey))
                    return -1;

                // Lets use the default key instead.
                keyString = m_defaultKey;
            }

            // Get the index of the item.
            int index = m_dictionary[keyString];

            // Return the index.
            return index;
        }
    }
}
