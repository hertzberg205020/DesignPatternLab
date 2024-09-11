namespace PrescriberSystem.Models;

public class PatientCase
{
    public HashSet<string> Symptoms { get; set; } = new HashSet<string>();

    public Prescription? Prescription { get; set; }

    public DateTime CaseTime { get; set; } = DateTime.Now;

    public string? PatientId { get; set; }
}
