//
// Copyright 2011 Patrik Svensson
//
// This file is part of BlackBox.
//
// BlackBox is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BlackBox is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License
// along with BlackBox. If not, see <http://www.gnu.org/licenses/>.
//

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("BlackBox")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Patrik Svensson")]
[assembly: AssemblyProduct("BlackBox")]
[assembly: AssemblyCopyright("Copyright © Patrik Svensson 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7da169c1-3bd3-4951-8e63-0ec37eeaf8d4")]

// We want BlackBox to be CLS compliant.
[assembly: CLSCompliant(true)]

// We want internal members to be testable.
[assembly: InternalsVisibleTo("BlackBox.UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010059FF90E61DB14EE0BF9ACB8AB91B5743BA5A40809F7F43C0648A0F073A689C4D02D905E713A433FCEF58525CE6E3C4C44E2C1D5E092B7EF335C24C6C3FF6BECF6409301F0A6FDC3449B803B1C2354DB94052B49F944F0A523EAF95D6C55843F5849EA589138472F6D5BB889ED5FE2B270A45DD587DB9FF1A00432B1E51F31CB1")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
