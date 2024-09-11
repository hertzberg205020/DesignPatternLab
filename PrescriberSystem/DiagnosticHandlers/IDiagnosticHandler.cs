using PrescriberSystem.Models;

namespace PrescriberSystem.DiagnosticHandlers;

public interface IDiagnosticHandler
{
    DiagnosticHandler? Next { get; set; }

    Task<Prescription> Diagnosis(DiagnosticRequest request);
}
