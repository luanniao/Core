using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace LuanNiao.Core.NetTools
{
    /// <summary>
    /// sometools for ip v4
    /// </summary>
    public static class IPV4Tools
    {
        /// <summary>
        /// get this pc's all ipv4's boardcast ip list
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetBoardcastIPList()
        {
            var res = new List<IPAddress>();
            NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
            {
                IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                foreach (var item in IPInterfaceProperties.UnicastAddresses)
                {
                    if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        res.Add(GetBroadcastAddress(item.Address, item.IPv4Mask));
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// get the all ip v4 address which in this pc
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetAllIPList()
        {
            var res = new List<IPAddress>();
            NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
            {
                IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                foreach (var item in IPInterfaceProperties.UnicastAddresses)
                {
                    if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        res.Add(item.Address);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// get the boardcast ip address
        /// </summary>
        /// <param name="address">source ip</param>
        /// <param name="mask">this ip's mask info</param>
        /// <returns></returns>
        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
        {
            uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint broadCastIpAddress = ipAddress | ~ipMaskV4;
            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }

        /// <summary>
        /// decide some ipaddress isn't the local ipaddress
        /// </summary>
        /// <param name="ip">the ip that you will decide</param>
        /// <returns></returns>
        public static bool IsLocalIP(this IPAddress ip)
        {
            return GetAllIPList().Contains(ip);
        }

    }
}
