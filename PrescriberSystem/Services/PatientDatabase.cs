using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using PrescriberSystem.Models;

namespace PrescriberSystem.Services;

public sealed class PatientDatabase
{
    private readonly string _patientFilePath;
    private readonly ConcurrentDictionary<string, Patient> _patients =
        new ConcurrentDictionary<string, Patient>();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public PatientDatabase(string patientFilePath)
    {
        // _patientFilePath = Path.Combine(
        //     Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //     "patients.json"
        // );
        _patientFilePath = patientFilePath;
    }

    public async Task LoadPatientsAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (File.Exists(_patientFilePath))
            {
                await using var fs = new FileStream(
                    _patientFilePath,
                    FileMode.Open,
                    FileAccess.Read
                );
                var patients = await JsonSerializer.DeserializeAsync<List<Patient>>(fs);
                if (patients != null)
                {
                    foreach (var patient in patients)
                    {
                        _patients[patient.Id] = patient;
                    }
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> CreateNewPatientAsync(Patient patient)
    {
        if (_patients.ContainsKey(patient.Id))
        {
            Console.WriteLine($"Patient with ID {patient.Id} already exists. Skipping insertion.");
            return false;
        }

        return await InsertOrUpdatePatientAsync(patient);
    }

    public async Task<bool> InsertPatientCaseAsync(PatientCase patientCase)
    {
        if (!_patients.ContainsKey(patientCase.PatientId))
        {
            Console.WriteLine(
                $"Patient with ID {patientCase.PatientId} does not exist. Skipping insertion."
            );
            return false;
        }

        var patient = _patients[patientCase.PatientId];
        patient.PatientCases.Add(patientCase);
        return await InsertOrUpdatePatientAsync(patient);
    }

    public Patient? FindPatient(string id)
    {
        return _patients.GetValueOrDefault(id);
    }

    private async Task<bool> InsertOrUpdatePatientAsync(Patient patient)
    {
        await _semaphore.WaitAsync();
        try
        {
            _patients[patient.Id] = patient;

            // 將更新後的病人列表寫入文件
            var patients = _patients.Values.ToList();
            await using var fs = new FileStream(
                _patientFilePath,
                FileMode.Create,
                FileAccess.Write
            );
            await JsonSerializer.SerializeAsync(
                fs,
                patients,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }
            );

            return true; // 表示成功執行了插入操作
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
