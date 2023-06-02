using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BluetoothConnectionLibrary.Utils
{
    public class MessagingInterface
    {
        public delegate void MessageReceived(string Message);
        public event MessageReceived OnMessageReceived;

        Stream inputStream, outputStream;
        int bufferSize = 2048;
        string beginMessageToken = "<beginMessage>";
        string endMessageToken = "<endMessage>";

        public MessagingInterface(Stream inputStream, Stream outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                try
                {
                    ListenForMessages();
                }
                catch (Exception) { }
            }).Start();

        }


        void ListenForMessages()
        {
            StreamReader inputReader = new StreamReader(inputStream);
            string receivedText = "";
            while (true)
            {
                receivedText += Encoding.UTF8.GetString(new byte[] { (byte)inputReader.Read() });
                while (receivedText.Contains(endMessageToken))
                {
                    OnMessageReceived.Invoke(receivedText.Substring(
                        receivedText.IndexOf(beginMessageToken) + beginMessageToken.Length, 
                        receivedText.IndexOf(endMessageToken) - receivedText.IndexOf(beginMessageToken) - beginMessageToken.Length));
                    receivedText = receivedText.Substring(receivedText.IndexOf(endMessageToken) + endMessageToken.Length);
                }
            }
        }

        public void SendMessage(string message)
        {
            StreamWriter outputWriter = new StreamWriter(outputStream);
            outputWriter.Write(beginMessageToken +  message + endMessageToken);
            outputWriter.Flush();
        }
    }
}
