# rd.xml-UWP-to-NativeAOT
Converts UWP rd.xml format to NativeAOT rd.xml

NativeAOT only supports a subset of the rd.xml specification.
 * [UWP Specification](https://docs.microsoft.com/en-us/windows/uwp/dotnet-native/runtime-directives-rd-xml-configuration-file-reference)
 * [NativeAOT Specification](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/rd-xml-format.md)
 
Most users can apply ```Dynamic="Required All"``` to their whole assembly but in some cases the assembly will be too large causing NativeAOT to exceed its blob size. This tool allows users to specify namespaces, subtypes, etc. per the original UWP specification and it will output a rd.xml suitable for NativeAOT.
 
## Usage

Download the binary release from [Releases](https://github.com/tomcashman/rd.xml-UWP-to-NativeAOT/releases) and unzip the archive.

Run ```RdXmlConverter.exe```

Command line args:

```
  --rdxml         Required. Input UWP rd.xml file

  -o, --output    Required. Output NativeAOT rd.xml file

  -i, --input     Required. Input directory containing DLLs

  --help          Display this help screen.

  --version       Display version information.
```
