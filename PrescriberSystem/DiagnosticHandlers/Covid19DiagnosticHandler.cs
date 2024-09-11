using PrescriberSystem.Models;

namespace PrescriberSystem.DiagnosticHandlers;

public sealed class Covid19DiagnosticHandler : DiagnosticHandler
{
    // 使用忽略大小寫的比較器
    private static readonly HashSet<string> Symptoms = new HashSet<string>(
        new[] { "sneeze", "headache", "cough" },
        StringComparer.OrdinalIgnoreCase
    );

    protected override async Task<Prescription> DoDiagnosisAsync()
    {
        await Task.Delay(1000);
        return new Prescription()
        {
            Name = "清冠一號",
            PotentialDisease = "COVID-19",
            Medicines = ["清冠一號"],
            Usage = "將相關藥材裝入茶包裡，使用500 mL 溫、熱水沖泡悶煮 1~3 分鐘後即可飲用。"
        };
    }

    protected override bool IsMatch(DiagnosticRequest request)
    {
        if (request.Symptoms.Count != Symptoms.Count)
        {
            return false;
        }

        var symptomsIgnoreCase = new HashSet<string>(
            request.Symptoms,
            StringComparer.OrdinalIgnoreCase
        );
        return Symptoms.SetEquals(symptomsIgnoreCase);
    }
}
