using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace EulerCommands
{
    [RunInstaller( true )]
    public class EulerCommandsSnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "EulerCommands"; }
        }
        public override string Vendor
        {
            get { return ""; }
        }
        public override string VendorResource
        {
            get { return "EulerCommands,"; }
        }
        public override string Description
        {
            get { return "Registers the CmdLets and Providers in this assembly"; }
        }
        public override string DescriptionResource
        {
            get { return "EulerCommands,Registers the CmdLets and Providers in this assembly"; }
        }
    }
}
