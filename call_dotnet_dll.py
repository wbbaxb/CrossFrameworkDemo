import ctypes
import os
import sys


def main():
    try:
        script_dir = os.path.dirname(os.path.abspath(__file__))

        dll_path = os.path.join(
            script_dir, "builds", "Debug", "net8.0-windows", "CrossFrameworkLibrary.dll")

        if not dll_path:
            return 1

        dotnet_lib = ctypes.cdll.LoadLibrary(dll_path)

        # 定义返回类型为字符指针(char*)
        dotnet_lib.GetPublicMessage.restype = ctypes.c_char_p
        public_message = dotnet_lib.GetPublicMessage() 
        print(f"type of public_message: {type(public_message)}") # <class 'bytes'>
        print(f"GetPublicMessage result: {public_message.decode('utf-8')}")

        dotnet_lib.GetInternalMessage.restype = ctypes.c_char_p
        internal_message = dotnet_lib.GetInternalMessage()
        print(f"GetInternalMessage result: {internal_message.decode('utf-8')}")

        dotnet_lib.RemotingTest.restype = ctypes.c_char_p
        result = dotnet_lib.RemotingTest()
        print(f"RemotingTest result: {result.decode('utf-8')}")

        return 0

    except Exception as ex:
        print(f"Error occurred: {ex}")
        return 1


if __name__ == "__main__":
    sys.exit(main())
