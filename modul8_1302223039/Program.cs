using System.Net.Http.Headers;
using System.Text.Json;
using System.Transactions;

internal class Program
{
    private static void Main(string[] args)
    {
        long totalTransfer;
        long uangTransfer;
        BankPublic defaultConfig = new BankPublic();

        if (defaultConfig.bankConfig.lang == "en" )
        {
            Console.Write("Please insert the amount of money to transfer: ");
        } 
        
        else if (defaultConfig.bankConfig.lang == "id")
        {
            Console.Write("Masukkan jumlah uang yang akan di-transfer: ");
        }

        // Menampilkan Total Biaya Transfer
        uangTransfer = Convert.ToInt64(Console.ReadLine());

        if (uangTransfer > defaultConfig.bankConfig.transfer.threshold)
        {
            totalTransfer = defaultConfig.bankConfig.transfer.high_fee + uangTransfer;
        }

        else
        {
            totalTransfer = defaultConfig.bankConfig.transfer.low_fee + uangTransfer;
        }


        if (defaultConfig.bankConfig.lang == "en")
        {
           
            Console.WriteLine("Transfer fee = " + defaultConfig.bankConfig.transfer.low_fee);
            Console.WriteLine("Total amount = " + totalTransfer);

        }

        else if (defaultConfig.bankConfig.lang == "id")
        {
            
            Console.WriteLine("Biaya tranfer = " + defaultConfig.bankConfig.transfer.low_fee);
            Console.WriteLine("Total biaya = " + totalTransfer);
        }

        // Pemilihan metode

        if (defaultConfig.bankConfig.lang == "en")
        {
            Console.WriteLine("\nSelect transfer method: ");
        }

        else if (defaultConfig.bankConfig.lang == "id")
        {
            Console.WriteLine("\nPilih metode transfer: ");
        }

        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine((i+1)+"."+defaultConfig.bankConfig.methods[i]);
        }

        if (defaultConfig.bankConfig.lang == "en")
        {
            Console.WriteLine("Please type " + defaultConfig.bankConfig.confirmation.en + "to confirm the transaction");
        }

        else if (defaultConfig.bankConfig.lang == "id")
        {
            Console.WriteLine("Ketik " + defaultConfig.bankConfig.confirmation.id + "untuk mengkonfirmasi transaks");
        }


    }
}

public class BankPublicConfig
{
    public string lang { get; set; }
    public Transfer transfer { get; set; }
    public List<string> methods { get; set; }
    public Confirmation confirmation { get; set; }

    public BankPublicConfig() { }

    public BankPublicConfig(string lang, Transfer transfer, List<string> methods, Confirmation confirmation)
    {
        this.lang = lang;  
        this.transfer = transfer;
        this.methods = methods;
        this.confirmation = confirmation;

    }

}

public class Transfer
{
    public long threshold { get; set; }

    public int low_fee { get; set; }

    public int high_fee { get; set;}

    public Transfer(long threshold, int low_fee, int high_fee)
    {
        this.threshold = threshold;
        this.low_fee = low_fee;
        this.high_fee = high_fee;
    }
}

public class Confirmation
{
    public string en { get; set; }
    
    public string id { get; set; }

    public Confirmation(string en, string id)
    {
        this.en = en;
        this.id = id;
    }
}

public class BankPublic
{
    public BankPublicConfig bankConfig;
    public const string filePath = @"bank_transfer_config.json";

    public BankPublic()
    {
        try
        {
            ReadConfig();
        }

        catch (Exception)
        { 
            SetDefault();
            WriteNewConfig();
        }

        
    }

    private BankPublicConfig ReadConfig()
    {
        string jsonData = File.ReadAllText(filePath);
        bankConfig = JsonSerializer.Deserialize<BankPublicConfig>(jsonData);
        return bankConfig;
    }

    private void SetDefault()
    {
        bankConfig = new BankPublicConfig("en",new Transfer(25000000, 6500, 15000), 
            ["RTO (real-time)","SKN","RTGS","BI FAST"],new Confirmation("Yes","Ya") );
    }

    private void WriteNewConfig()
    {
        JsonSerializerOptions opts = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        string JsonString = JsonSerializer.Serialize(bankConfig, opts);
        File.WriteAllText(filePath, JsonString);
    }
}