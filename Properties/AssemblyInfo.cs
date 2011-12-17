using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Fotoideen")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("wp7dev.de")]
[assembly: AssemblyProduct("Fotoideen")]
[assembly: AssemblyCopyright("Copyright © wp7dev.de 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("19787d82-09ab-48c1-957d-156f2601ffb8")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: NeutralResourcesLanguageAttribute("de")]

[assembly: Obfuscation(Feature = "encrypt symbol names with password altima", Exclude = false)]
[assembly: Obfuscation(Feature = "Apply to type Fotoideen.Model.*: all", Exclude = true, ApplyToMembers = true)]
[assembly: Obfuscation(Feature = "Apply to type Fotoideen.Resources.*: all", Exclude = true, ApplyToMembers = true)]