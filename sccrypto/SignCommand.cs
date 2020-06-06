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
    [Command("sign",Description ="Sign a data file")]
    public class SignCommand : ICommand
    {
        [CommandParameter(0, Name = "Data File Name", Description = "The name of the file that need to be signed")]
        public string FileName { get; set; }

        [CommandParameter(1, Name = "Signature File Name", Description = "The name of the file the signature will be saved to.")]
        public string SigFileName { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            CspParameters csp = new CspParameters(1, "Microsoft Base Smart Card Crypto Provider");
            csp.Flags = CspProviderFlags.UseDefaultKeyContainer;



            // Initialize an RSACryptoServiceProvider object using
            // the CspParameters object.

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048, csp))
            {
                // Create some data to sign.
                using (var data = File.OpenRead(FileName))
                {
                     // Sign the data using the Smart Card CryptoGraphic Provider.
                    byte[] sig = rsa.SignData(data, HashAlgorithmName.SHA256,RSASignaturePadding.Pkcs1);
                    
                    string signature = System.Convert.ToBase64String(sig);
                    Console.WriteLine("Signature	: " + signature);

                    if (File.Exists(SigFileName))
                    {
                        console.Output.WriteLine("Deleting signature");
                        File.Delete(SigFileName);
                    }
                    console.Output.WriteLine("Writing signature");
                    File.AppendAllText(SigFileName, signature);
                }

                return default;
            }
        }
    }
}
