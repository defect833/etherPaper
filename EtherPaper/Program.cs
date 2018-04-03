using Nethereum.Web3.Accounts;
using System.IO;
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
            string privateKeySavePath = "private.txt"; // save location for the resulting private key file (can be overridden by optional argument)
            string privateKey; // unencrypted private key

            // optional arguments
            if (args.Length > 2)
                privateKeySavePath = args[2];

            string keyStoreEncryptedJson = await LoadJson(keyStorePath);

            // decrypt private key from keystore
            privateKey = ExtractPrivateKeyFromKeyStore(keyStoreEncryptedJson, password);

            // save private key to a new file
            //TODO: needs file creation safety checks
            using (StreamWriter fWriter = File.CreateText(privateKeySavePath))
            {
                await fWriter.WriteLineAsync(privateKey);
            }
        }

        private static string ExtractPrivateKeyFromKeyStore(string keyStoreEncryptedJson, string password)
        {
            var account = Account.LoadFromKeyStore(keyStoreEncryptedJson, password);

            return account.PrivateKey;
        }

        private static async Task<string> LoadJson(string path)
        {
            string keyStoreEncryptedJson = string.Empty;

            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    keyStoreEncryptedJson = await r.ReadToEndAsync();
                }
            }

            return keyStoreEncryptedJson;
        }
    }
}
