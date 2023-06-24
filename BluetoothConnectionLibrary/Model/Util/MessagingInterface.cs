using Plugin.BLE.Abstractions.Contracts;
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

        string beginMessageToken = "<beginMessage>";
        string endMessageToken = "<endMessage>";
        ICharacteristic characteristic;

        public MessagingInterface(ICharacteristic characteristic)
        {
            this.characteristic = characteristic;
            characteristic.ValueUpdated += Characteristic_ValueUpdated;
        }

        private void Characteristic_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            ListenForMessages();
        }

        void ListenForMessages()
        {
            string receivedText = "";
            while (true)
            {
                receivedText += Encoding.UTF8.GetString(characteristic.ReadAsync().Result);
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
            characteristic.WriteAsync(Encoding.UTF8.GetBytes(beginMessageToken +  message + endMessageToken));
        }
    }
}
