# DinkToPdf
.NET Core P/Invoke wrapper for wkhtmltopdf library that uses Webkit engine to convert HTML pages to PDF.

### Install 

Library can be installed through Nuget. Run command bellow from the package manager console:

```
PM> Install-Package DinkToPdf
```

Copy all native libraries 32 & 64 to a directory of your project. From there .NET Core loads native library when native method is called with P/Invoke. You can find latest version of native library [here](https://github.com/rdvojmoc/DinkToPdf/tree/master/v0.12.4). Select appropriate library for your OS and platform (64 or 32 bit).

Make sure your .csproj is keeping the libraries onto the OutputDirectory (dotnet publish).
It will make sure for Azure App Service Deployment that your App Service has all the required libs.

Example:
```xml
<ItemGroup> 
      <None Include="libs\\32 bit\\libwkhtmltox.dll">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
      <None Include="libs\\32 bit\\libwkhtmltox.dylib">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
      <None Include="libs\\32 bit\\libwkhtmltox.so">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>

      <None Include="libs\\64 bit\\libwkhtmltox.dll">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
      <None Include="libs\\64 bit\\libwkhtmltox.dylib">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
      <None Include="libs\\64 bit\\libwkhtmltox.so">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
  </ItemGroup>
```


### IMPORTANT
Library was NOT tested with IIS. Library was tested in console applications and with Kestrel web server both for Web Application and Web API . 

### 

### Basic converter
Use this converter in single threaded applications.

Create converter:
```csharp
var converter = new BasicConverter(new PdfTools());
```

### Synchronized converter
Use this converter in multi threaded applications and web servers. Conversion tasks are saved to blocking collection and executed on a single thread.

```csharp
var converter = new SynchronizedConverter(new PdfTools());
```

### Define document to convert
```csharp
var doc = new HtmlToPdfDocument()
{
    GlobalSettings = {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Landscape,
        PaperSize = PaperKind.A4Plus,
    },
    Objects = {
        new ObjectSettings() {
            PagesCount = true,
            HtmlContent = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Ut                               odio viverra, molestie lectus nec, venenatis turpis.",
            WebSettings = { DefaultEncoding = "utf-8" },
            HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
        }
    }
};

```

### Convert
If Out property is empty string (defined in GlobalSettings) result is saved in byte array. 
```csharp
byte[] pdf = converter.Convert(doc);
```

If Out property is defined in document then file is saved to disk:
```csharp
var doc = new HtmlToPdfDocument()
{
    GlobalSettings = {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
        Margins = new MarginSettings() { Top = 10 },
        Out = @"C:\DinkToPdf\src\DinkToPdf.TestThreadSafe\test.pdf",
    },
    Objects = {
        new ObjectSettings()
        {
            Page = "http://google.com/",
        },
    }
};
```
```csharp
converter.Convert(doc);
```

### Dependency injection

Create a custom assembly loader in your Startup class.
```csharp

internal class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        return LoadUnmanagedDll(absolutePath);
    }
    protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
    {
        return LoadUnmanagedDllFromPath(unmanagedDllName);
    }
    protected override Assembly Load(AssemblyName assemblyName)
    {
        throw new NotImplementedException();
    }
}
public void ConfigureServices(IServiceCollection services)
{
    CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
    var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
    
    // Check the platform and load the appropriate Library
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"libs\\{architectureFolder}\\libwkhtmltox.dylib");
        context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"libs\\{architectureFolder}\\libwkhtmltox.so");
        context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
    }
    else // RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    {
        var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"libs\\{architectureFolder}\\libwkhtmltox.dll");
        context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
    }

    // Add converter to DI
    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
}
```

Converter must be registered as singleton.


