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

namespace BlackBox
{
    internal sealed class ServiceProvider : IServiceLocator, IServiceProvider
    {
        private readonly Dictionary<Type, object> _services;

        internal Dictionary<Type, object> Services
        {
            get { return _services; }
        }

        internal ServiceProvider()
        {
            _services = new Dictionary<Type, object>();
        }

        public void Register<T>(T instance)
        {
            if (!_services.ContainsKey(instance.GetType()))
            {
                _services.Add(instance.GetType(), instance);
            }
        }

        public void Register<T, V>(T instance)
            where T : V
        {
            if (!_services.ContainsKey(typeof(V)))
            {
                _services.Add(typeof(V), instance);
            }
        }

        public void Unregister<T>()
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _services.Remove(typeof(T));
            }
        }

        public T Resolve<T>()
        {
            if (_services.ContainsKey(typeof(T)))
            {
                return (T)_services[typeof(T)];
            }
            return default(T);
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                return _services[serviceType];
            }
            return null;            
        }

        #endregion
    }
}
