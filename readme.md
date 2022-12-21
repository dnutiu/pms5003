# Introduction

C# Library for interfacing with the PMS5003 Particulate Matter Sensor. 

For wiring the Sensor consult the [datasheet](https://www.aqmd.gov/docs/default-source/aq-spec/resources-page/plantower-pms5003-manual_v2-3.pdf). 

## Example

```csharp
using System;
using System.Threading;

namespace Application
{
    class Example
    {
        static void Main(string[] args)
        {
            var pms = new Pms5003("COM3", -1, -1);
            while (true)
            {
                try
                {
                    var data = pms.ReadData();
                    Console.WriteLine(data);
                    Thread.Sleep(2000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
```