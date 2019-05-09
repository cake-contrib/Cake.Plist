using System.Runtime.InteropServices;
using Cake.Core;
using Xunit;

namespace Cake.Plist.Tests
{
    public sealed class WindowsFact : FactAttribute
    {
        private static readonly PlatformFamily _family;

        static WindowsFact()
        {
            _family = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? PlatformFamily.Windows : PlatformFamily.Unknown;
        }

        public WindowsFact(string reason = null)
        {
            if (_family != PlatformFamily.Windows)
            {
                Skip = reason ?? "Windows test.";
            }
        }
    }
}
