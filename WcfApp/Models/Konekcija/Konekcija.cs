using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Konekcije
{
    [ExcludeFromCodeCoverage]
    public class Konekcija
    {
        public static short ServerPort { get; set; } = 12341;
        public static short ServerWritePort { get; set; } = 12351;
        public static short ProxyServerPort { get; set; } = 12361;

        public static string UriServer { get; set; } = $"net.tcp://localhost:{ServerPort}/MerenjeService";
        public static string UriServerWrite { get; set; } = $"net.tcp://localhost:{ServerWritePort}/MerenjeService";
        public static string UriProxyServer { get; set; } = $"net.tcp://localhost:{ProxyServerPort}/MerenjeService";
    }
}
