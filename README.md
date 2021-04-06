# WkHtmlToPdf-DotNet [![NuGet Version](http://img.shields.io/nuget/v/Haukcode.WkHtmlToPdfDotNet.svg?style=flat)](https://www.nuget.org/packages/Haukcode.WkHtmlToPdfDotNet/)

.NET Core P/Invoke wrapper for wkhtmltopdf library that uses Webkit engine to convert HTML pages to PDF.

Has been successfully tested on Windows, Linux, MacOSX and docker. One of the examples is using this in docker.


### Install 

Library can be installed through Nuget. Run command below from the package manager console:

```
PM> Install-Package Haukcode.WkHtmlToPdfDotNet
```
*Note that with this NuGet package you don't need to manually add the native binaries to your project.*


### Fork
This library is forked from DinkToPdf. The main changes are to include the required native binaries in the package so they don't have to be manually installed, and renamed to a more appropriate project name. The license has also been corrected to match the license for the wkhtmltopdf parent project.


### Building
Download the binaries (of wkhtmltopdf) from Github Releases (currently version 0.12.5) and put them under `src\WkHtmlToPdf-DotNet` in the `runtimes` folder (with a sub folder for each platform). Note that these binaries are only needed when you build this project to generate the NuGet package, you will **not** need the binaries when **using** the NuGet package, they will be automatically added.


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
        Out = @"C:\WkHtmlToPdf-DotNet\src\TestThreadSafe\test.pdf",
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
Converter must be registered as singleton.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add converter to DI
    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
}
```

### Docker 
If you are using the linux version of docker container for net core provided from microsoft, you need to add this line to the dockerfile 
```
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
```
