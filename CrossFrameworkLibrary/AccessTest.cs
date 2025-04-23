using System.Runtime.InteropServices;
using System.Runtime.Remoting; // only for .NET Framework

namespace CrossFrameworkLibrary
{
    public class AccessTest
    {
        [DllExport("GetPublicMessage", CallingConvention.Cdecl)]
        public static string GetPublicMessage()
        {
            return "This is a public message!";
        }

        [DllExport("GetInternalMessage", CallingConvention.Cdecl)]
        internal static string GetInternalMessage()
        {
            return "This is a internal message, only accessible through reflection or DllImport!";
        }

        [DllExport("RemotingTest", CallingConvention.Cdecl)]
        public static string RemotingTest()
        {
            try
            {
                return typeof(RemotingConfiguration).ToString();
            }
            catch
            {
                return "error";
            }
        }
    }
}