using PrescriberSystem.Models;

namespace PrescriberSystem.DiagnosticHandlers;

public abstract class DiagnosticHandler : IDiagnosticHandler
{
    public DiagnosticHandler? Next { get; set; }

    protected DiagnosticHandler() { }

    public async Task<Prescription> Diagnosis(DiagnosticRequest request)
    {
        if (IsMatch(request))
        {
            return await DoDiagnosisAsync();
        }

        if (Next != null)
        {
            return await Next.Diagnosis(request);
        }

        throw new InvalidOperationException("No handler found for the given symptoms.");
    }

    protected abstract Task<Prescription> DoDiagnosisAsync();

    protected abstract bool IsMatch(DiagnosticRequest request);
}
