![qpdf_c#](https://user-images.githubusercontent.com/6692947/165035710-ffc38a67-58bf-4ccf-a1f7-aa553d5e9af0.png)

What is QPdfNet
=========
A C# wrapper for QPDF that exposes all the functionality that is available through the QPDF command-line tool

It supports linearization, encryption, and numerous other features. It can also be used for splitting and merging files, creating PDF files (but you have to supply all the content yourself), and inspecting files for study or analysis. QPDF does not render PDFs or perform text extraction, and it does not contain higher-level interfaces for working with page contents. It is a low-level tool for working with the structure of PDF files and can be a valuable tool for anyone who wants to do programmatic or command-line-based manipulation of PDF files.

The QPDF Manual is hosted online at https://qpdf.readthedocs.io. The project website is https://qpdf.sourceforge.io. The source code repository is hosted at GitHub: https://github.com/qpdf/qpdf.

## License Information

QPdfNet is Copyright (C) 2021-2022 Magic-Sessions and is licensed under the MIT license:

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.

## Microsoft Visual C++ runtimes

The DLL qpdf29.dll is compiled with Visual Studio 2022 you need these C++ runtimes for it on your computer

- X86: https://aka.ms/vs/17/release/vc_redist.x86.exe
- X64: https://aka.ms/vs/17/release/vc_redist.x64.exe

How to use
==========

## Getting the amount of pages

```c#
var job = new Job();
var result = job.InputFile(Path.Combine("TestFiles", "20_pages.pdf"))
    .ShowNPages()
    .Run(out var output);

Assert.AreEqual(ExitCode.Success, result);
Assert.AreEqual(output, "20");
```

## Encrypting a PDF file

```c#
var outputFile = Path.Combine(_testFolder, "output_encryption_256_bit.pdf");

var job = new Job();
var result = job.InputFile(Path.Combine("TestFiles", "test.pdf"))
    .OutputFile(outputFile)
    .Encrypt("user", "owner", new Encryption256Bit(true, true, true, true, true, true, Modify.None, Print.None))
    .Linearize()
    .Run(out _);

Assert.AreEqual(ExitCode.Success, result);
```

## Checking if a file is encrypted

```c#
var job = new Job();
var result = job.InputFile(Path.Combine("TestFiles", "encryption_256_bit.pdf"))
    .IsEncrypted()
    .RunIsEncrypted(out _);

Assert.AreEqual(ExitCodeIsEncrypted.Encrypted, result);
```

See the test project for more examples https://github.com/Sicos1977/QPdfNet/blob/main/QpdfNetTest/QpdfTests.cs

Logging
=======

QPdfNet uses the Microsoft ILogger interface (https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-5.0). You can use any logging library that uses this interface.

QPdfNet has some build in loggers that can be found in the ```QPdfNet.Logger``` namespace. 

For example

```c#
var logger = !string.IsNullOrWhiteSpace(<some logfile>)
                ? new QPdfNet.Loggers.Stream(File.OpenWrite(<some logfile>))
                : new QPdfNet.Loggers.Console();
                
var job = new Job(logger);                
```

Installing via NuGet
====================

The easiest way to install MSGReader is via NuGet.

In Visual Studio's Package Manager Console, simply enter the following command:

    Install-Package QPdfNet

Core Team
=========
    Sicos1977 (Kees van Spelde)

Support
=======
If you like my work then please consider a donation as a thank you.

<a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=NS92EXB2RDPYA" target="_blank"><img src="https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif" /></a>
