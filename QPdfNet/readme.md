![qpdf_c#](https://user-images.githubusercontent.com/6692947/165035710-ffc38a67-58bf-4ccf-a1f7-aa553d5e9af0.png)

What is QPdfNet
=========
A C# wrapper for QPDF that exposes all the functionality that is available through the QPDF command-line tool

It supports linearization, encryption, and numerous other features. It can also be used for splitting and merging files, creating PDF files (but you have to supply all the content yourself), and inspecting files for study or analysis. QPDF does not render PDFs or perform text extraction, and it does not contain higher-level interfaces for working with page contents. It is a low-level tool for working with the structure of PDF files and can be a valuable tool for anyone who wants to do programmatic or command-line-based manipulation of PDF files.

The QPDF Manual is hosted online at https://qpdf.readthedocs.io. The project website is https://qpdf.sourceforge.io. The source code repository is hosted at GitHub: https://github.com/qpdf/qpdf.

## License Information

QPdfNet is Copyright (C) 2021-2026 Magic-Sessions and is licensed under the MIT license:

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

The DLL qpdf30.dll is compiled with Visual Studio 2026 you need these C++ runtimes for it on your computer

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

Job class overview
==================

The `Job` class exposes (almost) all functionality of the QPDF command-line tool through a fluent API. Every method returns the `Job` instance so calls can be chained. After configuring a job you call one of the `Run` methods to execute it. Below is an overview of all available methods grouped by category.

## Input / Output

| Method | Description |
| --- | --- |
| `InputFile(string fileName)` | The input PDF file |
| `OutputFile(string fileName)` | The output PDF file |
| `Empty()` | Use an empty (zero-page) dummy file instead of an input file |
| `ReplaceInput()` | Overwrite the input file with the output |

## General options

| Method | Description |
| --- | --- |
| `WarningExit0()` | Exit with code 0 instead of 3 when only warnings occur |
| `Password(string password)` | Password for an encrypted input file |
| `PasswordFile(string file)` | Read the password from the first line of a file |
| `Verbose()` | Increase verbosity of the output |
| `Progress()` | Indicate progress while writing the output file |
| `NoWarn()` | Suppress writing of warnings |
| `DeterministicId()` | Generate a deterministic document ID |
| `AllowWeakCrypto()` | Allow creation of files with weak (RC4) cryptography |
| `KeepFilesOpen(bool keepOpen)` | Control whether files are kept open when merging |
| `KeepFilesOpenThreshold(int count = 200)` | Threshold that triggers keep-files-open behaviour |

## Advanced control

| Method | Description |
| --- | --- |
| `PasswordIsHexKey()` | Interpret the password as a hex-encoded encryption key |
| `SuppressPasswordRecovery()` | Do not try different password encodings |
| `PasswordMode(PasswordMode mode)` | Fine-tune how passwords are encoded |
| `SuppressRecovery()` | Do not attempt recovery of damaged files |
| `IgnoreXrefStreams()` | Ignore cross-reference streams |

## Transformation

| Method | Description |
| --- | --- |
| `Linearize()` | Create a linearized (web-optimized) file |
| `Encrypt(string? userPassword, string ownerPassword, IEncryption options)` | Encrypt the output file |
| `Decrypt()` | Remove encryption from the output file |
| `RemoveRestrictions()` | Remove security restrictions from signed files |
| `CopyEncryption(string file)` | Copy encryption parameters from another file |
| `EncryptionFilePassword(string password)` | Password for the file used by `CopyEncryption` |
| `Qdf()` | Create a QDF file for editing in a text editor |
| `NoOriginalObjectIds()` | Omit original object ID comments in QDF mode |
| `CompressStreams(bool compress)` | Compress uncompressed streams |
| `DecodeLevel(DecodeLevel decodeLevel)` | Control which streams are uncompressed |
| `StreamData(StreamData streamData)` | Control stream compression |
| `RecompressFlate()` | Uncompress and recompress flate streams |
| `CompressionLevel(int level)` | Flate compression level (1-9) |
| `JpegQualityLevel(int n = 75)` | JPEG quality used by `OptimizeImages` |
| `NormalizeContent(bool normalize)` | Normalize newlines in content streams |
| `ObjectStreams(ObjectStreams objectStreams)` | Control use of object streams |
| `PreserveUnreferenced()` | Preserve unreferenced objects |
| `RemoveUnreferencedResources(AutoYesNo value)` | Remove unreferenced page resources |
| `PreserveUnreferencedResources()` | Keep unreferenced page resources |
| `NewlineBeforeEndstream()` | Force a newline before `endstream` (PDF/A) |
| `CoalesceContents()` | Combine a page's content streams into one |
| `ExternalizeInlineImages()` | Convert inline images to regular images |
| `IiMinBytes(int size = 1024)` | Minimum size for externalizing inline images |
| `MinVersion(string version)` | Set the minimum PDF version |
| `ForceVersion(string version)` | Force the output PDF version |

## Modification

| Method | Description |
| --- | --- |
| `Pages(string file, string range, string? password = null)` | Select pages from one or more files |
| `Collate(int n = 1)` / `Collate(List<int> pages)` | Collate instead of concatenate pages |
| `SplitPages(int n = 1)` | Write each group of pages to a separate file |
| `Overlay(string file, string? to, string? from, string? repeat)` | Overlay pages from another file |
| `Underlay(string file, string? to, string? from, string? repeat)` | Underlay pages from another file |
| `FlattenRotation()` | Remove `/Rotate` and bake rotation into content |
| `FlattenAnnotations(FlattenAnnotations parameter)` | Push annotations into the page content |
| `Rotate(Rotation angle, string? pageRange = null)` | Rotate pages |
| `GenerateAppearances()` | Generate appearance streams for form fields |
| `OptimizeImages()` | Recompress images using DCT (JPEG) |
| `OiMinWidth(int width = 128)` | Minimum width for `OptimizeImages` |
| `OiMinHeight(int height = 128)` | Minimum height for `OptimizeImages` |
| `OiMinArea(int pixels = 16384)` | Minimum area for `OptimizeImages` |
| `KeepInlineImages()` | Exclude inline images from optimization |
| `RemoveAcroform()` | Remove the interactive form dictionary |
| `RemoveInfo()` | Remove the `/Info` dictionary (except `/ModDate`) |
| `RemoveMetadata()` | Remove the `/Metadata` dictionary |
| `RemoveStructure()` | Remove the structure tree |
| `RemovePageLabels()` | Remove page labels (explicit page numbers) |
| `SetPageLabels(string pageLabels)` | Set page labels for the whole document |

## Attachments

| Method | Description |
| --- | --- |
| `AddAttachment(...)` | Add (embed) a file as an attachment |
| `CopyAttachmentsFrom(string file, string? prefix = null)` | Copy attachments from another file |
| `RemoveAttachment(string key)` | Remove an embedded file by key |

## Inspection

| Method | Description |
| --- | --- |
| `IsEncrypted()` | Test whether a file is encrypted (use `RunIsEncrypted`) |
| `RequiresPassword()` | Test a file's password (use `RunRequiresPassword`) |
| `Check()` | Check the structure of the PDF file |
| `ShowEncryption()` | Show document encryption parameters |
| `ShowEncryptionKey()` | Show the encryption key |
| `CheckLinearization()` | Check the linearization hint tables |
| `ShowLinearization()` | Show the linearization hint tables |
| `ShowXref()` | Show the cross-reference data |
| `ShowObject(string obj)` | Show the contents of an object |
| `RawStreamData()` | Show raw (compressed) stream data |
| `FilteredStreamData()` | Show filtered (uncompressed) stream data |
| `ShowNPages()` | Show the number of pages |
| `ShowPages()` | Show page dictionary information |
| `WithImages()` | Include image details with `ShowPages` |
| `ListAttachments()` | List embedded files |
| `ShowAttachment(string key)` | Export an embedded file |

## JSON

| Method | Description |
| --- | --- |
| `Json(JsonVersion version = JsonVersion.Version2)` | Generate a JSON representation of the file |
| `JsonKey(string key)` | Limit which top-level keys are in the JSON output |
| `JsonObject(string obj)` | Limit which objects are in the JSON output |
| `JsonStreamData(JsonStreamData streamData)` | Control how streams are handled in JSON output |
| `JsonStreamPrefix(string prefix)` | Prefix for JSON stream data files |
| `JsonOutput(JsonVersion version = JsonVersion.Version2)` | Apply defaults for JSON serialization |
| `JsonInput()` | Treat the input file as qpdf JSON |
| `UpdateFromJson(string jsonFile)` | Update a PDF from a qpdf JSON file |

## Testing / debugging

| Method | Description |
| --- | --- |
| `StaticId()` | Use a fixed document ID (testing only) |
| `StaticAesIv()` | Use a static AES initialization vector (testing only) |
| `LinearizePass1(string fileName)` | Save the first linearization pass to a file |
| `ReportMemoryUsage()` | Report the maximum amount of memory used |

## Running the job

| Method | Description |
| --- | --- |
| `Run(out string? output)` | Run the job and return any output |
| `Run(out string? output, out byte[]? data)` | Run the job and also return binary data |
| `RunIsEncrypted(out string? output)` | Run a job configured with `IsEncrypted()` |
| `RunRequiresPassword(out string? output)` | Run a job configured with `RequiresPassword()` |

Logging
=======

QPdfNet uses the Microsoft ILogger interface (https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-5.0). You can use any logging library that uses this interface.

The following 3 loglevels are used:

- LogInformatie: Logs information about the usage of QPdfNet
- LogError: Logs any errors that occurs
- LogDebug: Logs debug information

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

The easiest way to install QPdfNet is via NuGet.

In Visual Studio's Package Manager Console, simply enter the following command:

    Install-Package QPdfNet

Core Team
=========
    Sicos1977 (Kees van Spelde)