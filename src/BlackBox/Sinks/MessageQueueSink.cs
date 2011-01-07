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
using BlackBox.Formatting;
using System.Messaging;
using System.Globalization;

namespace BlackBox
{
	[LogSinkType("msmq")]
	public sealed class MessageQueueSink : FormatLogSink
	{
		private MessageQueue _messageQueue;
		private FormatPattern<ILogEntry> _labelPattern;

		#region Properties

		public string Queue { get; set; }
		public string Label { get; set; }
		public bool Recoverable { get; set; }
		public bool CreateIfNotExists { get; set; }

		#endregion

		#region Construction

		public MessageQueueSink()
			: base()
		{
			this.Format = "$(time:format='HH:mm:ss.fff'): $(message)";
			this.Label = "$(level:numeric=false)";
		}

		#endregion

		#region Initialization

		protected internal override void InitializeSink(IServiceLocator locator)
		{
			// Create the message queue.
			if (!MessageQueue.Exists(this.Queue))
			{
				if (this.CreateIfNotExists)
				{
					try
					{
						// Create the message queue.
						_messageQueue = MessageQueue.Create(this.Queue);
					}
					catch (MessageQueueException exception)
					{
						// The queue could not be created.
						string message = string.Format(CultureInfo.InvariantCulture, "Could not create message queue '{0}'.", this.Queue);
						throw new BlackBoxException(message, exception);
					}
				}
				else
				{
					// The message queue didn't exist and we're not allowed to create it.
					string message = string.Format(CultureInfo.InvariantCulture, "The message queue '{0}' do not exist.", this.Queue);
					throw new BlackBoxException(message);
				}
			}
			else
			{
				// Instanciate the message queue.
				_messageQueue = new MessageQueue(this.Queue, QueueAccessMode.Send);
			}

			// Initialize the label pattern.
			_labelPattern = FormatPattern<ILogEntry>.Create(locator, this.Label);

			// Call the base class so the format message sink gets properly initialized.
			base.InitializeSink(locator);
		}

		#endregion

		protected override void WriteEntry(ILogEntry entry)
		{
			try
			{
				// Create the message.
				using (Message message = new Message())
				{
					message.Recoverable = this.Recoverable;
					message.Label = _labelPattern.Render(entry);
					message.Body = this.FormatEntry(entry);

					// Send the message.
					_messageQueue.Send(message);
				}
			}
			catch (MessageQueueException)
			{
				// There was an error sending the message.
				// TODO: Write to internal logger.
			}
		}
	}
}
