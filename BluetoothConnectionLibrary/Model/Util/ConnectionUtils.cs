using System;
using System.Collections.Generic;
using System.Text;

namespace BluetoothConnectionLibrary.Util
{
    public class ConnectionUtils
    {
        public static string ParseMAC(string givenMAC)
        {
            givenMAC = givenMAC.ToUpper();
            if (givenMAC.Length == 17) return givenMAC;
            return givenMAC.Substring(0, 2) + ':' + givenMAC.Substring(2, 2) + ':' + givenMAC.Substring(4, 2) + ':' +
                   givenMAC.Substring(6, 2) + ':' + givenMAC.Substring(8, 2) + ':' + givenMAC.Substring(10, 2);
        }
    }
}
