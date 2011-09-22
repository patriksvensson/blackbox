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
using System.Globalization;
using System.Messaging;
using BlackBox.Formatting;

namespace BlackBox
{
	/// <summary>
	/// Log sink that write messages to a MSMQ queue.
	/// </summary>
	[LogSinkType("msmq")]
	public sealed class MessageQueueSink : FormatLogSink
	{
		private MessageQueue _messageQueue;
		private FormatPattern _labelPattern;

		#region Properties

		/// <summary>
		/// Gets or sets the name of the queue to write to.
		/// </summary>
		/// <value>The queue.</value>
		public string Queue { get; set; }

		/// <summary>
		/// Gets or sets the label of the MSMQ message.
		/// This can be a format pattern.
		/// </summary>
		/// <value>The label.</value>
		public string Label { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether MSMQ messages are recoverable.
		/// </summary>
		/// <value><c>true</c> if MSMQ messages are recoverable; otherwise, <c>false</c>.</value>
		public bool Recoverable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the MSMQ queue should be created if it do not exist.
		/// </summary>
		/// <value><c>true</c> if the MSMQ queue should be created if it do not exist; otherwise, <c>false</c>.</value>
		public bool CreateIfNotExists { get; set; }

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageQueueSink"/> class.
		/// </summary>
		public MessageQueueSink()
			: base()
		{
			this.Format = "$(time(format='HH:mm:ss.fff')): $(message())";
			this.Label = "$(level(numeric=false))";
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Initializes the log sink.
		/// </summary>
		/// <param name="context"></param>
		protected internal override void Initialize(InitializationContext context)
		{
			#region Sanity Check
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (string.IsNullOrEmpty(this.Queue))
			{
				throw new BlackBoxException("The message queue has not been set.");
			}
			#endregion

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
			_labelPattern = context.FormatPatternFactory.Create(this.Label);

			// Call the base class so the format message sink gets properly initialized.
			base.Initialize(context);
		}

		#endregion

		/// <summary>
		/// Performs the writing of the specified entry to the MSMQ queue.
		/// </summary>
		/// <param name="entry">The entry.</param>
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
			catch (MessageQueueException ex)
			{
				// There was an error sending the message.
				// Write an error message to the internal log.
				string message = string.Format(CultureInfo.InvariantCulture, "An exception occured while sending MSMQ message. {0}", ex.Message);
				this.InternalLogger.Write(LogLevel.Error, message);
			}
		}
	}
}
