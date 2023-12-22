// See https://aka.ms/new-console-template for more information

using Wasmtime;
using System.Text;
using System.Runtime.InteropServices;

//DECISIONE GUIDO: passano massimo 500kb (da rest api di fonte su console)
int maxmemory = 500 * 1024;

#region  instantiation
var wasmfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wasiapplication.wasm");
var wasi = new WasiConfiguration();

using var engine = new Engine();
using var module = Module.FromFile(engine, wasmfile);
using var linker = new Linker(engine);
using var store = new Store(engine);

linker.DefineWasi();
store.SetWasiConfiguration(wasi);

var instance = linker.Instantiate(store, module);

#endregion  instantiation

#region add
// var add = instance.GetFunction<int, int, int>("MyAdd");
// if (add is null)
// {
//     Console.WriteLine("error: MyAdd export is missing");
//     return;
// }

// Console.WriteLine(add(1, 2));
#endregion add

#region hello world
// var helloworld = instance.GetFunction<int>("HelloWorld");
// if (helloworld is null)
// {
//     Console.WriteLine("error: HelloWorld export is missing");
//     return;
// }

// var ptr = helloworld();
// var mem = instance.GetMemory("memory");
// var bastrin = mem!.GetSpan(ptr, maxmemory);
// //trim to first 0
// int i = 0;
// while (bastrin[i] != 0)
// {
//     i++;
// }
// bastrin = bastrin.Slice(0, i);
// var str = Encoding.ASCII.GetString(bastrin);

// Console.WriteLine(str);

#endregion hello world

#region hello string
var hellostring = instance.GetFunction<int, int, int>("HelloString");
if (hellostring is null)
{
    Console.WriteLine("error: HelloString export is missing");
    return;
}

String myname = "guidoneee";
int startinginmemory = 0;
var allocate = instance.GetFunction<int, int>("AllocateMemory");
if (allocate is null)
{
    Console.WriteLine("error: AllocateMemory export is missing");
    return;
}
startinginmemory = allocate(maxmemory);
Console.WriteLine($"Allocata memoria all'indirizzo {startinginmemory}");

int byteswritten = instance.GetMemory("memory")!.WriteString(startinginmemory, myname);

#region verifica che hai scritto bene su memoria

var hellostring_mem = instance.GetMemory("memory");
var hellostring_bastrin = hellostring_mem!.GetSpan(startinginmemory, maxmemory);
int hellostring_i = 0;
while (hellostring_bastrin[hellostring_i] != 0)
{
    hellostring_i++;
}
hellostring_bastrin = hellostring_bastrin.Slice(0, hellostring_i);
var hellostring_str = Encoding.ASCII.GetString(hellostring_bastrin);

Console.WriteLine($"Read from WASM Memory:  {hellostring_str}");

#endregion verifica che hai scritto bene su memoria

int ret = hellostring(startinginmemory, maxmemory);

var mem = instance.GetMemory("memory");
var bastrin = mem!.GetSpan(ret, maxmemory);
//trim to first 0
int i = 0;
while (bastrin[i] != 0)
{
    i++;
}
bastrin = bastrin.Slice(0, i);
var str = Encoding.ASCII.GetString(bastrin);

Console.WriteLine(str);

#endregion hello string

Console.ReadLine();



