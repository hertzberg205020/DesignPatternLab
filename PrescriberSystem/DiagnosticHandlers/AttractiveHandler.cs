using PrescriberSystem.Models;

namespace PrescriberSystem.DiagnosticHandlers;

public class AttractiveHandler : DiagnosticHandler
{
    private static readonly HashSet<string> Symptoms = new HashSet<string>(
        new[] { "sneeze" },
        StringComparer.OrdinalIgnoreCase
    );

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

        return request.Symptoms.SetEquals(Symptoms) && request.Patient?.Gender == 'F';
    }

    protected override async Task<Prescription> DoDiagnosisAsync()
    {
        await Task.Delay(3000);

        return new Prescription()
        {
            Name = "青春抑制劑",
            PotentialDisease = "Attractive",
            Medicines = ["假鬢角", "臭味"],
            Usage = "把假鬢角黏在臉的兩側，讓自己異性緣差一點，自然就不會有人想妳了。"
        };
    }
}
