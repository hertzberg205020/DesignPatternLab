using PrescriberSystem.Models;

namespace PrescriberSystem.DiagnosticHandlers;

public class SleepApneaSyndromeHandler : DiagnosticHandler
{
    private static readonly HashSet<string> Symptoms = new HashSet<string>(
        new[] { "snore" },
        StringComparer.OrdinalIgnoreCase
    );

    protected override async Task<Prescription> DoDiagnosisAsync()
    {
        await Task.Delay(3000);

        return new Prescription()
        {
            Name = "打呼抑制劑",
            PotentialDisease = "SleepApneaSyndrome",
            Medicines = ["一捲膠帶"],
            Usage = "睡覺時，撕下兩塊膠帶，將兩塊膠帶交錯黏在關閉的嘴巴上，就不會打呼了。"
        };
    }

    protected override bool IsMatch(DiagnosticRequest request)
    {
        if (request.Symptoms.Count != Symptoms.Count)
        {
            return false;
        }

        if (request.Patient == null)
        {
            return false;
        }

        return Symptoms.SetEquals(request.Symptoms) && CalculateBMI(request.Patient) > 26;
    }

    private static float CalculateBMI(Patient patient)
    {
        // 計算 BMI
        return patient.Weight / (patient.Height * patient.Height / 10000);
    }
}
