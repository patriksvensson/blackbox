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
using System.ComponentModel;
using System.Reflection;

namespace BlackBox.Editor
{
	public class ObscuringTypeDescriptor : ICustomTypeDescriptor, INotifyPropertyChanged, IDisposable
	{
		private PropertyDescriptorCollection _propertyCache;
		private object _owner;
        private bool _disposed;

		public ObscuringTypeDescriptor(object obj)
		{
			_owner = obj;
		}

		#region ICustomTypeDescriptor Members

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return new AttributeCollection(null);
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
            if (_propertyCache == null)
			{
				List<PropertyDescriptor> descriptors = new List<PropertyDescriptor>();
				foreach (PropertyInfo property in _owner.GetType().GetProperties())
				{
					if (property.GetCustomAttributes(typeof(SkipSerializationAttribute), true).FirstOrDefault() != null)
					{
						continue;
					}

					if (!property.CanRead || !property.CanWrite)
					{
						continue;
					}

                    var descriptor = new ObscuringPropertyDescriptor(_owner, property);
                    descriptor.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);

					descriptors.Add(descriptor);
				}
                _propertyCache = new PropertyDescriptorCollection(descriptors.ToArray());
			}
            return _propertyCache;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		#endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = (s, a) => { };

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    foreach (ObscuringPropertyDescriptor descriptor in _propertyCache)
                    {
                        descriptor.PropertyChanged -= new PropertyChangedEventHandler(OnPropertyChanged);
                    }
                }
                _disposed = true;
            }
        }

        #endregion

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChanged(this, e);
        }
    }
}
