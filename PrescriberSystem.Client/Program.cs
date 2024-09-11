using PrescriberSystem.Models;

namespace PrescriberSystem.Client;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // 取得專案的絕對路徑
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var supportedDiseaseFilePath = Path.Combine(root, "Config", "support-disease.txt");
            var patientFilePath = Path.Combine(root, "Data", "patients.json");

            // 檢查文件是否存在
            if (!File.Exists(supportedDiseaseFilePath))
            {
                throw new FileNotFoundException("支援疾病文件不存在", supportedDiseaseFilePath);
            }
            if (!File.Exists(patientFilePath))
            {
                throw new FileNotFoundException("病人數據文件不存在", patientFilePath);
            }

            await using var facade = await PrescriberSystemFacade.CreateAsync(
                patientFilePath,
                supportedDiseaseFilePath
            );
            await TestDiagnosisAsync(facade);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"發生錯誤: {ex.Message}");
            // 可以在這裡添加更詳細的日誌記錄
        }
    }

    private static async Task TestDiagnosisAsync(PrescriberSystemFacade facade)
    {
        var patientId = "A123456789";

        var res = await facade.PrescriptionDemandAsync(patientId, ["sneeze", "headache", "cough"]);
        Console.WriteLine(res.Name == "清冠一號");
    }

    private static void TestDbQuery(PrescriberSystemFacade facade)
    {
        var patient = facade.FindPatient("A123456789");
        Console.WriteLine(patient?.Name);
    }

    private static async Task TestAddNewPatient(PrescriberSystemFacade facade)
    {
        var patient = new Patient()
        {
            Id = "G334455667",
            Name = "EmmaTaylor",
            Gender = 'F',
            Age = 28,
            Height = 162.7f,
            Weight = 58.3f,
            PatientCases = []
        };

        await facade.CreateNewPatientAsync(patient);

        var newPatient = facade.FindPatient(patient.Id);
        Console.WriteLine(newPatient?.Name == patient.Name);
    }

    private static async Task TestSavePrescriptionAsync()
    {
        var root = AppDomain.CurrentDomain.BaseDirectory;
        var prescription = new Prescription()
        {
            Name = "清冠一號",
            PotentialDisease = "COVID-19",
            Medicines = ["清冠一號"],
            Usage = "將相關藥材裝入茶包裡，使用500 mL 溫、熱水沖泡悶煮 1~3 分鐘後即可飲用。"
        };

        Console.WriteLine(root);
        await PrescriberSystemFacade.SavePrescriptionAsync(
            prescription,
            root,
            "prescription",
            ExportFormat.Json
        );
    }
}
