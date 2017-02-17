// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public interface ITransmitter<T>
	{
		string Name { get; }

		void Broadcast(T message);

		void Register(IReceiver<T> receiver);
		void Unregister(IReceiver<T> receiver);
	}
}

/*
-----------------------------------------------------
Implementation Example
-----------------------------------------------------

namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            Radio radio1 = new Radio("radio1");
            Radio radio2 = new Radio("radio2");

            RadioListener listener1 = new RadioListener("listener1");
            RadioListener listener2 = new RadioListener("listener2");

            listener1.Subscribe(radio1);
            listener1.Subscribe(radio2);

            radio1.Broadcast(new Message { Content = "radio1: test 1" }); // received by listener1
            radio2.Broadcast(new Message { Content = "radio2: test A" }); // received by listener1

            listener1.Unsubscribe(radio1);
            listener2.Subscribe(radio2);

            radio1.Broadcast(new Message { Content = "radio1: test 2" }); // lost...

            listener1.Subscribe(radio1);

            radio1.Broadcast(new Message { Content = "radio1: test 3" }); // received by listener1

            radio2.Broadcast(new Message { Content = "radio2: test B" }); // received by listener1 & listener2

            Console.ReadLine();
        }
    }

    public class Radio : ITransmitter<Message>
    {
        private readonly IList<IReceiver<Message>> _receivers = null;

        public string Name
        {
            get;
            private set;
        }

        public Radio(string name)
        {
            if (string.IsNullOrEmpty(name))
                Throw.ThrowArgumentException("name");

            this.Name = name;
            _receivers = new List<IReceiver<Message>>();
        }

        public void Broadcast(Message message)
        {
            foreach (IReceiver<Message> receiver in _receivers)
            {
                receiver.Receive(message);
            }
        }

        public void Register(IReceiver<Message> receiver)
        {
            if (receiver != null)
                _receivers.Add(receiver);
        }

        public void Unregister(IReceiver<Message> receiver)
        {
            if (receiver != null)
            {
                if (_receivers.Contains(receiver))
                    _receivers.Remove(receiver);
            }
        }
    }

    public class RadioListener : IReceiver<Message>
    {
        public string Name
        {
            get;
            private set;
        }

        public RadioListener(string name)
        {
            if (string.IsNullOrEmpty(name))
                Throw.ThrowArgumentException("name");

            this.Name = name;
        }

        public void Receive(Message message)
        {
            Console.WriteLine("{0} -> {1}", this.Name, message.Content);

            Debug.WriteLine("Receiver {0}: {1}", this.Name, message.Content);
        }

        public void Subscribe(ITransmitter<Message> transmitter)
        {
            transmitter.Register(this);

            Debug.WriteLine("Receiver {0} -----> Transmitter {1}...", this.Name, transmitter.Name);
        }

        public void Unsubscribe(ITransmitter<Message> transmitter)
        {
            transmitter.Unregister(this);
            Debug.WriteLine("Receiver {0} --x--> Transmitter {1}...", this.Name, transmitter.Name);
        }
    }

    public class Message
    {
        public string Content
        {
            get;
            set;
        }
    }
}

*/