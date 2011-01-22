using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackBox.Editor
{
    public class LogConfigurationEventArgs : EventArgs
    {
        private readonly LogConfiguration _configuration;

        public LogConfiguration Configuration
        {
            get { return _configuration; }
        } 

        public LogConfigurationEventArgs(LogConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
