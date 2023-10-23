using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace KitchenChaos {
    public static class IPAddress {
        public static string Public() {
            var externalIpString = new WebClient().DownloadString("https://ipv4.icanhazip.com")
                .Replace(@"\r\n", "")
                .Replace(@"\n", "")
                .Trim();
            var externalIp = System.Net.IPAddress.Parse(externalIpString);
            return externalIp.ToString();
        }

        public static string Local() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static bool TryGetValidAddressAndPort(string test, out (string address, string port) data) {
            data = default;
            const string pattern = @"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}):(\d{1,5})";
            var valid = Regex.IsMatch(test, pattern);
            if (valid) {
                var match = Regex.Match(test, pattern);
                data = (match.Groups[1].Value, match.Groups[2].Value);
            }
            return valid;
        }
    }
}
