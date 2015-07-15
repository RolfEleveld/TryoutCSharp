using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;

namespace Powershell.Extensions.For.Euler
{
    /// <summary>
    /// Install this assembly with InstallUtil PowershellExtensionsForEuler.dll in a visual studio cmd from th output folder
    /// List installed snap-ins with Get-PSSnapIn -registered
    /// Add the snapin with Add-PSSnapIn yoursnapinname
    /// </summary>
    [Cmdlet( VerbsCommon.Get, "NextFibonacciSequence", SupportsShouldProcess = true )]
    public class GetNextFibonacciSequence : Cmdlet
    {
        static readonly Utility.Euler euler = new Utility.Euler();
        private static int count;

        #region Parameters
        /*
        [Parameter(Position = 0,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Help Text")]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            
        }
 */
        #endregion

        protected override void ProcessRecord()
        {
            try
            {
                WriteObject( euler.FibonacciSequence().Take( count++ ), true );
            }
            catch ( Exception ex)
            {
                Debug.Write(ex.Message);
                Debug.Write(ex.StackTrace);
                Debug.Write(ex.InnerException);
            }
        }
    }
}