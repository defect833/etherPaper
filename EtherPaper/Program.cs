using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EtherPaper
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string keyStorePath = args[0]; // path to the Ethereum keystore file
            string password = args[1]; // password associated with Ethereum keystore file
            //string privateKeySavePath = args[2] ?? "private.txt"; // optional save path for private key file
            string privateKeySavePath = "private.txt"; //TODO: temporary until optional arguments can be implemented
            string privateKey; // unencrypted private key

            // decrypt private key from keystore
            privateKey = await ExtractPrivateKeyFromKeyStore(keyStorePath, password);

            // save private key to a new file
            //TODO: needs file creation safety checks
            using (StreamWriter fWriter = File.CreateText(privateKeySavePath))
            {
                await fWriter.WriteLineAsync(privateKey);
            }
        }


        /// <summary>
        /// Using a provided password, will decrypt and provide a private key from an Ethereum KeyStore file
        /// </summary>
        /// <param name="path">path to the keystore file</param>
        /// <param name="password">password to decrypt the encrypted private key</param>
        /// <returns>unencrypted private key</returns>
        private static async Task<string> ExtractPrivateKeyFromKeyStore(string path, string password)
        {

            if (File.Exists(path))
            {
                using (FileStream fStream = File.OpenRead(path))
                {
                    // read the byte contents of the file
                    byte[] byteResult = new byte[fStream.Length];
                    await fStream.ReadAsync(byteResult, 0, (int)fStream.Length);

                    // convert contents to an ASCII formatted string
                    string strResult = System.Text.Encoding.ASCII.GetString(byteResult);

                    // parse the contents as JSON
                    var json = JObject.Parse(strResult);
                    foreach (var property in json.Properties())
                    {
                        Console.WriteLine("{0} - {1}", property.Name, property.Value);
                    }

                    return strResult; //TODO: placeholder returning keystore file contents as string, this should be the decrypted private key
                }
            }

            return null;
        }
    }
}
