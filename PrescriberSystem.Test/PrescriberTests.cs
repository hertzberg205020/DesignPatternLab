using System.Diagnostics;
using NSubstitute;
using PrescriberSystem.DiagnosticHandlers;
using PrescriberSystem.Models;
using PrescriberSystem.Services;

namespace PrescriberSystem.Test;

public class PrescriberTests
{
    private List<DiagnosticHandler>? _handlers;

    private void SetupHandlers()
    {
        _handlers = new List<DiagnosticHandler>
        {
            new SleepApneaSyndromeHandler(),
            new AttractiveHandler(),
            new Covid19DiagnosticHandler()
        };

        for (int i = 0; i < _handlers.Count - 1; i++)
        {
            _handlers[i].Next = _handlers[i + 1];
        }
    }

    [Fact]
    public async Task PrescriptionDemandAsync_ShouldCompleteWithinExpectedTime()
    {
        // Arrange

        var diagnosticHandler = Substitute.For<IDiagnosticHandler>();
        diagnosticHandler
            .Diagnosis(Arg.Any<DiagnosticRequest>())
            .Returns(async _ =>
            {
                await Task.Delay(3000);
                return new Prescription
                {
                    Name = "Test",
                    PotentialDisease = "Test",
                    Medicines = ["Test"],
                    Usage = "Test"
                };
            });

        var patient = new Patient()
        {
            Id = "A123456789",
            Name = "Test",
            Age = 20,
            Gender = 'M',
            Height = 180.3f,
            Weight = 70.5f,
            PatientCases = []
        };

        var symptoms = new HashSet<string> { "Cough", "Fever" };
        const int numberOfClients = 2;
        var tasks = new List<Task<Prescription>>();

        // Act

        // 模擬多個客戶端請求
        await using var prescriber = new Prescriber(diagnosticHandler);

        for (var i = 0; i < numberOfClients; i++)
        {
            tasks.Add(prescriber.PrescriptionDemandAsync(patient, symptoms));
        }

        var stopwatch = Stopwatch.StartNew();

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        var elapsedTimeSpan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
        var flooredSeconds = (int)Math.Floor(elapsedTimeSpan.TotalSeconds);

        // Assert

        Assert.Equal(6, flooredSeconds);
    }

    [Fact]
    public async Task Diagnosis_WithCovid19Symptoms_ShouldGetRightPrescription()
    {
        // Arrange
        SetupHandlers();
        var handler = _handlers?.First();

        var patient = new Patient()
        {
            Id = "A123456789",
            Name = "Test",
            Age = 20,
            Weight = 180.3f,
            Height = 70.5f,
            PatientCases = []
        };

        // Act
        var prescription = await handler.Diagnosis(
            new DiagnosticRequest(patient, ["sneeze", "headache", "cough"])
        );

        // Assert
        Assert.Equal("清冠一號", prescription.Name);
    }

    [Fact]
    public async Task Diagnosis_WithAttractiveSymptoms_ShouldGetRightPrescription()
    {
        // Arrange
        SetupHandlers();
        var handler = _handlers?.First();

        var patient = new Patient()
        {
            Id = "A123456789",
            Name = "Test",
            Gender = 'F',
            Age = 20,
            Weight = 180.3f,
            Height = 70.5f,
            PatientCases = []
        };

        // Act
        var prescription = await handler.Diagnosis(new DiagnosticRequest(patient, ["sneeze"]));

        // Assert
        Assert.Equal("青春抑制劑", prescription.Name);
    }

    // test SleepApneaDiagnosticHandler
    [Fact]
    public async Task Diagnosis_WithSleepApneaSymptoms_ShouldGetRightPrescription()
    {
        // Arrange
        SetupHandlers();
        var handler = _handlers?.First();

        var patient = new Patient()
        {
            Id = "A123456789",
            Name = "Test",
            Gender = 'M',
            Age = 20,
            Height = 175,
            Weight = 120,
            PatientCases = []
        };

        // Act
        var prescription = await handler.Diagnosis(new DiagnosticRequest(patient, ["snore"]));

        // Assert
        Assert.Equal("打呼抑制劑", prescription.Name);
    }
}
