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
    [Command("verify", Description ="verify the signature of a data file")]
    public class VerifyCommand : ICommand
    {
        [CommandParameter(0, Name = "Data File Name", Description = "The name of the file that need to be signed")]
        public string FileName { get; set; }

        [CommandParameter(1, Name = "Signature File Name", Description = "The name of the file the signature read from.")]
        public string SigFileName { get; set; }

        [CommandParameter(2, Name = "Public key File Name", Description = "The name of the file the key will be read from.")]
        public string KeyFileName { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            CspParameters csp = new CspParameters(1, "Microsoft Base Smart Card Crypto Provider");
            csp.Flags = CspProviderFlags.UseDefaultKeyContainer;



            // Initialize an RSACryptoServiceProvider object using
            // the CspParameters object.

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048, csp))
            {
                if (!File.Exists(KeyFileName))
                {
                    console.Output.WriteLine("Public key file {0} doesn't exist. Exiting", KeyFileName);
                    return default;
                }

                rsa.FromXmlString(File.ReadAllText(KeyFileName));
                
                if (!File.Exists(SigFileName))
                {
                    console.Output.WriteLine("Signature file doesn't exist. Exiting");
                    return default;
                }

                if (!File.Exists(FileName))
                {
                    console.Output.WriteLine("Data file {0} doesn't exist. Exiting", FileName);
                    return default;
                }

                using (var data = File.OpenRead(FileName))
                {
                                        string signature = File.ReadAllText(SigFileName);
                    byte[] sig = System.Convert.FromBase64String(signature);

                    // Verify the data using the Smart Card CryptoGraphic Provider.
                    bool verified = rsa.VerifyData(data, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    Console.WriteLine("Verified		: " + verified);
                }
                return default;
            }
        }
    }
}
