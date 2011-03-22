using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace BlackBox
{
    /// <summary>
    /// Log sink that writes messages to the event log name.
    /// </summary>
    [LogSinkType("eventlog")]
    public sealed class EventLogSink : FormatLogSink
    {
        /// <summary>
        /// Gets or sets the event source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the event log name.
        /// </summary>
        /// <value>The log.</value>
        public string Log { get; set; }

        /// <summary>
        /// Gets or sets the name of the target machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public string MachineName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogSink"/> class.
        /// </summary>
        public EventLogSink()
        {
            this.Source = string.Empty;
            this.Log = "Application";            
        }

		/// <summary>
		/// Initializes the log sink.
		/// </summary>
		/// <param name="context"></param>
		protected internal override void Initialize(InitializationContext context)
        {
            if (string.IsNullOrEmpty(this.Source))
            {
                throw new BlackBoxException("Event log source have not been set.");
            }
            if (string.IsNullOrEmpty(this.Log))
            {
                throw new BlackBoxException("Event log name have not been set.");
            }

            base.Initialize(context);
        }

        /// <summary>
        /// Performs the writing of the specified entry to the event log.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected override void WriteEntry(ILogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            // Does the source exist?
            if (!EventLog.SourceExists(this.Source))
            {
                if (string.IsNullOrEmpty(this.MachineName))
                {
                    // Create the event source.
                    EventLog.CreateEventSource(this.Source, this.Log);
                }
                else
                {
                    // Create the remove event source.
                    EventSourceCreationData data = new EventSourceCreationData(this.Source, this.Log);
                    data.MachineName = this.MachineName;
                    EventLog.CreateEventSource(data);
                }
            }

            // Get the stuff we need.
            string message = this.FormatEntry(entry);
            EventLogEntryType entryType = EventLogSink.GetEventLogEntryType(entry.Level);

            // Write the entry to the event log.
            EventLog.WriteEntry(this.Source, message, entryType);
        }

        private static EventLogEntryType GetEventLogEntryType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Fatal: return EventLogEntryType.Error;
                case LogLevel.Error: return EventLogEntryType.Error;
                case LogLevel.Warning: return EventLogEntryType.Warning;
                case LogLevel.Information: return EventLogEntryType.Information;
                case LogLevel.Verbose: return EventLogEntryType.Information;
                case LogLevel.Debug: return EventLogEntryType.Information;
                default:
                    string message = "Encountered unknown log level '{0}'.";
                    throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, level));
            }
        }
    }
}
