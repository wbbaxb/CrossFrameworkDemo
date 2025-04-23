using System;
using System.Runtime.InteropServices;

namespace CrossFrameworkDemo
{
    public static class NativeMethods
    {
        [DllImport("CrossFrameworkLibrary.dll", EntryPoint = "GetPublicMessage", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetPublicMessage();

        [DllImport("CrossFrameworkLibrary.dll", EntryPoint = "GetInternalMessage", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetInternalMessage();

        [DllImport("CrossFrameworkLibrary.dll", EntryPoint = "RemotingTest", CallingConvention = CallingConvention.Cdecl)]
        public static extern string RemotingTest();
    }
}