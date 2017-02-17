// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Messaging;

	public static class MessageQueueHelper
	{
		/// <summary>
		/// Gets the fullname of the queue (".\private$\nameQueue").
		/// </summary>
		/// 
		/// <param name="queueName">
		/// Name of the queue (".\private$\nameQueue" or "nameQueue").
		/// </param>
		/// 
		/// <returns>
		/// The fullname of the queue.
		/// </returns>
		public static string GetQueueFullName(string queueName)
		{
			if (string.IsNullOrEmpty(queueName))
			{
				ThrowException.ThrowArgumentException("queueName");
			}

			if (!queueName.ToLowerInvariant().Contains(@"\private$\"))
			{
				queueName = string.Format(@".\private$\{0}", queueName);
			}

			return queueName;
		}

		/// <summary>
		/// Creates the new queue if not exists.
		/// </summary>
		/// 
		/// <param name="queueName">
		/// Name of the queue (".\private$\mailQueue" or "nameQueue").
		/// </param>
		/// 
		/// <param name="isTransactional">
		/// Value indicating whether the queue is transactional.
		/// </param>
		/// 
		/// <returns>
		/// The MessageQueue instance.
		/// </returns>
		public static MessageQueue CreateQueue(string queueName, bool isTransactional = false)
		{
			if (string.IsNullOrEmpty(queueName))
			{
				ThrowException.ThrowArgumentException("queueName");
			}

			queueName = GetQueueFullName(queueName);

			if (MessageQueue.Exists(queueName))
			{
				return new MessageQueue(queueName);
			}

			return MessageQueue.Create(queueName, isTransactional);
		}

		public static T ReceiveMessage<T>(this MessageQueue messageQueue) where T : class, new()
		{
			T value = default(T);

			Message message = messageQueue.Receive();

			if (message.Body is T)
			{
				value = (T)message.Body;
			}

			return value;
		}

		public static T PeekMessage<T>(this MessageQueue messageQueue) where T : class, new()
		{
			T value = default(T);

			Message message = messageQueue.Peek();

			if (message.Body is T)
			{
				value = (T)message.Body;
			}

			return value;
		}

		public static void SendMessage(this MessageQueue messageQueue, object body, string messageLabel = null)
		{
			Message message = new Message(body);

			message.Label = messageLabel;
			message.Recoverable = true;

			messageQueue.Send(message);
		}
	}
}
