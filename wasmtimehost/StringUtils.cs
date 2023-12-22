using System.Runtime.InteropServices;

namespace wasmtimehost;

public static class StringUtils
{
    public static nint returnpointerstring(int len, string returnablestring)
    {
        var ptr = Marshal.AllocHGlobal(len);
        for (int i = 0; i < returnablestring.Length; i++)
        {
            Marshal.WriteByte(ptr + i, (byte)returnablestring[i]);
        }
        return ptr;
    }

}