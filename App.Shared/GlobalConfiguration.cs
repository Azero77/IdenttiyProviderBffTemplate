using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared
{
    public static class GlobalConfiguration
    {
        public static string IdentityUri { get; } = "https://localhost:5001";
        public static string BFFUri { get; } = "https://localhost:5002";
        public static string ClientUri { get; } = "https://localhost:5003";
        public static string ApiUri { get; } = "https://localhost:5004";
    }
}
