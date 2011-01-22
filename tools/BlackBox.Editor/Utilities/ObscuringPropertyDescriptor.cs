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
	public class ObscuringPropertyDescriptor : PropertyDescriptor, INotifyPropertyChanged
	{
		private object _owner;
		private PropertyInfo _property;

		public ObscuringPropertyDescriptor(object owner, PropertyInfo property)
			: base(property.Name, null)
		{
			_owner = owner;
			_property = property;
		}

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override Type ComponentType
		{
			get { return _owner.GetType(); }
		}

		public override object GetValue(object component)
		{
			return _property.GetValue(_owner, null);
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		public override Type PropertyType
		{
			get { return _property.PropertyType; }
		}

		public override void ResetValue(object component)
		{			
		}

		public override void SetValue(object component, object value)
		{
			_property.SetValue(_owner, value, null);
            this.PropertyChanged(this, new PropertyChangedEventArgs(_property.Name));
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = (s, a) => { };

        #endregion
    }
}
