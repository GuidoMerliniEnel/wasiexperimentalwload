using System;
using System.Runtime.InteropServices;

namespace wasiapplication;
public static class Class1
{
    [UnmanagedCallersOnly(EntryPoint = "MyAdd")]
    public static int MyAdd(int a, int b)
    {
        return a + b;
    }

    [UnmanagedCallersOnly(EntryPoint = "HelloWorld")]
    public static IntPtr HelloWorld()
    {
        string returnablestring = "Hello, World!";
        return StringUtils.returnpointerstring(returnablestring.Length, returnablestring);
    }

    [UnmanagedCallersOnly(EntryPoint = "AllocateMemory")]
    public static IntPtr AllocateMemory(int size)
    {
        return Marshal.AllocHGlobal(size);
    }

    [UnmanagedCallersOnly(EntryPoint = "HelloString")]
    public static IntPtr HelloString(IntPtr input, int size)
    {
        string? inputstring = StringUtils.returnstringfrompointer(size, input);

        //GM: here you fake working on the string
        string returnablestring = "Hello, World! your name is " + inputstring;

        return StringUtils.returnpointerstring(returnablestring.Length, returnablestring);
    }

}