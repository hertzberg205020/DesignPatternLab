using PrescriberSystem.DiagnosticHandlers;
using PrescriberSystem.Models;
using PrescriberSystem.Services;
using PrescriberSystem.Utils;
using PrescriberSystem.Validators;

namespace PrescriberSystem;

public class PrescriberSystemFacade : IDisposable, IAsyncDisposable
{
    private readonly Prescriber _prescriber;

    private readonly PatientDatabase _patientDatabase;

    private bool _disposed = false;

    private PrescriberSystemFacade(PatientDatabase patientDatabase, Prescriber prescriber)
    {
        _patientDatabase = patientDatabase;
        _prescriber = prescriber;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _prescriber.Dispose();
                _patientDatabase.Dispose();
            }

            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (!_disposed)
        {
            // Dispose _prescriber asynchronously
            await _prescriber.DisposeAsync().ConfigureAwait(false);

            // Dispose _patientDatabase asynchronously
            await _patientDatabase.DisposeAsync().ConfigureAwait(false);

            _disposed = true;
        }
    }

    ~PrescriberSystemFacade()
    {
        Dispose(false);
    }

    public static async Task<PrescriberSystemFacade> CreateAsync(
        string patientFilePath,
        string supportDiseaseFilePath
    )
    {
        var patientDatabase = new PatientDatabase(patientFilePath);
        var prescriber = InitializePrescriber(supportDiseaseFilePath);

        var facade = new PrescriberSystemFacade(patientDatabase, prescriber);
        await facade.InitializeAsync();
        return facade;
    }

    private async Task InitializeAsync()
    {
        try
        {
            await _patientDatabase.LoadPatientsAsync();
        }
        catch (Exception ex)
        {
            // Log or handle the exception appropriately
            Console.WriteLine($"Failed to load patients: {ex.Message}");
        }
    }

    private static Prescriber InitializePrescriber(string supportDiseaseFilePath)
    {
        var supportedDisease = TxtReader.ReadTxtFile(supportDiseaseFilePath);
        var diagnosticHandlers = CreateDiagnosticHandlers(supportedDisease);
        return new Prescriber(diagnosticHandlers.FirstOrDefault());
    }

    private static List<DiagnosticHandler> CreateDiagnosticHandlers(
        IEnumerable<string> supportedDiseases
    )
    {
        var handlerMapping = new Dictionary<string, Func<DiagnosticHandler>>
        {
            { "COVID-19", () => new Covid19DiagnosticHandler() },
            { "Attractive", () => new AttractiveHandler() },
            { "SleepApneaSyndrome", () => new SleepApneaSyndromeHandler() }
        };

        var handlers = supportedDiseases
            .Where(handlerMapping.ContainsKey)
            .Select(disease => handlerMapping[disease]())
            .ToList();

        for (var i = 0; i < handlers.Count - 1; i++)
        {
            handlers[i].Next = handlers[i + 1];
        }

        return handlers;
    }

    public async Task<Prescription> PrescriptionDemandAsync(
        string patientId,
        HashSet<string> symptoms
    )
    {
        var patient = _patientDatabase.FindPatient(patientId);

        if (patient == null)
        {
            throw new ArgumentException(
                $"Patient with ID {patientId} does not exist.",
                nameof(patientId)
            );
        }
        var res = await _prescriber.PrescriptionDemandAsync(patient, symptoms);
        await _patientDatabase.InsertPatientCaseAsync(
            new PatientCase()
            {
                PatientId = patientId,
                Symptoms = symptoms,
                Prescription = res
            }
        );
        return res;
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> CreateNewPatientAsync(Patient patient)
    {
        var existingPatient = _patientDatabase.FindPatient(patient.Id);

        if (existingPatient != null)
        {
            return (false, "Patient already exists.");
        }

        try
        {
            await ValidatePatient(patient);
        }
        catch (ArgumentException e)
        {
            return (false, e.Message);
        }

        var result = await _patientDatabase.CreateNewPatientAsync(patient);
        return (result, result ? string.Empty : "Failed to create new patient.");
    }

    private static async Task ValidatePatient(Patient patient)
    {
        var validator = new PatientValidator();

        var validationResult = await validator.ValidateAsync(patient);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(
                ", ",
                validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            );
            throw new ArgumentException($"Invalid request: {errors}");
        }
    }

    public Patient? FindPatient(string patientId)
    {
        return _patientDatabase.FindPatient(patientId);
    }

    public async Task<bool> InsertPatientCaseAsync(PatientCase patientCase)
    {
        return await _patientDatabase.InsertPatientCaseAsync(patientCase);
    }

    public static async Task SavePrescriptionAsync(
        Prescription prescription,
        string baseDirectory,
        string fileName,
        ExportFormat format
    )
    {
        var isDirectoryExists = Directory.Exists(baseDirectory);

        if (!isDirectoryExists)
        {
            Directory.CreateDirectory(baseDirectory);
        }

        await DtoSerializer.SerializeAsync(prescription, baseDirectory, fileName, format);
    }
}
