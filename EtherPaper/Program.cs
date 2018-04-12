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
            string paperWalletPath = args.Length > 2 ? args[2] : ""; // save location for the resulting private key file (optional argument) defaults to current path
            paperWalletPath = Path.Combine(paperWalletPath, "paperwallet.txt"); // adds filename to the provided path
            PaperWallet paperWallet;

            // gather keystore file contents as JSON
            string keyStoreEncryptedJson = await LoadJson(keyStorePath);

            // retireve decrypted details from keystore
            paperWallet = ExtractDetailsFromKeyStore(keyStoreEncryptedJson, password);

            // save private key to a new file
            //TODO: needs file creation safety checks
            using (StreamWriter fWriter = File.CreateText(paperWalletPath))
            {
                await fWriter.WriteLineAsync(paperWallet.Address);
                await fWriter.WriteLineAsync(paperWallet.PrivateKey);
            }
        }

        private static PaperWallet ExtractDetailsFromKeyStore(string keyStoreEncryptedJson, string password)
        {
            var account = Account.LoadFromKeyStore(keyStoreEncryptedJson, password);

            PaperWallet pw = new PaperWallet()
            {
                PrivateKey = account.PrivateKey,
                Address = account.Address
            };

            return pw;
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
