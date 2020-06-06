using CliFx;
using CliFx.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sccrypto
{
    [Command("publickey export",Description ="Export the public key to an xml file")]
    public class PublicKeyCommand : ICommand
    {
        [CommandParameter(0,Name ="Public key File Name", Description = "The name of the file the key will be exported to.")]
        public string FileName { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            // To idendify the Smart Card CryptoGraphic Providers on your
            // computer, use the Microsoft Registry Editor (Regedit.exe).
            // The available Smart Card CryptoGraphic Providers are listed
            // in HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider.

            // Create a new CspParameters object that identifies a
            // Smart Card CryptoGraphic Provider.
            // The 1st parameter comes from HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider Types.
            // The 2nd parameter comes from HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider.
            CspParameters csp = new CspParameters(1, "Microsoft Base Smart Card Crypto Provider");
            csp.Flags = CspProviderFlags.UseDefaultKeyContainer;



            // Initialize an RSACryptoServiceProvider object using
            // the CspParameters object.

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048, csp))
            {

                string xmlInfo = rsa.ToXmlString(false);
                if (File.Exists(FileName))
                {
                    console.Output.WriteLine("Deleting file: " + FileName);
                    File.Delete(FileName);
                }
                console.Output.WriteLine("Writing public key to: " + FileName);
                File.AppendAllText(FileName, xmlInfo);

                return default;
            }           
        }
    }
}
