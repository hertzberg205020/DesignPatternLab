namespace PrescriberSystem.Models;

public class DiagnosticRequest
{
    public Patient Patient { get; }
    public HashSet<string> Symptoms { get; }

    public TaskCompletionSource<Prescription> CompletionSource { get; }

    public DiagnosticRequest(Patient patient, HashSet<string> symptoms)
    {
        Patient = patient;
        Symptoms = symptoms;
        CompletionSource = new TaskCompletionSource<Prescription>(
            TaskCreationOptions.RunContinuationsAsynchronously
        );
    }
}
