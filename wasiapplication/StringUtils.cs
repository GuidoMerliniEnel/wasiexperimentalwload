using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace wasiapplication;

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

    public static unsafe string returnstringfrompointer(int len, nint ptr)
    {
        var bastrin = new Span<byte>((byte*)ptr.ToPointer(), len);
        //trim to first 0
        int i = 0;
        while (bastrin[i] != 0)
        {
            i++;
        }
        bastrin = bastrin.Slice(0, i);
        var str = Encoding.ASCII.GetString(bastrin);
        return str;
    }

}