using System.Collections.Concurrent;
using FluentValidation.Results;
using PrescriberSystem.DiagnosticHandlers;
using PrescriberSystem.Models;
using PrescriberSystem.Validators;

namespace PrescriberSystem.Services;

public sealed class Prescriber : IAsyncDisposable, IDisposable
{
    private readonly ConcurrentQueue<DiagnosticRequest> _diagnosisQueue;
    private readonly CancellationTokenSource _globalCts;
    private readonly IDiagnosticHandler? _diagnosticHandler;
    private readonly Task _processingTask;

    public Prescriber(IDiagnosticHandler? diagnosticHandler)
    {
        _diagnosisQueue = new ConcurrentQueue<DiagnosticRequest>();
        _diagnosticHandler = diagnosticHandler;
        _globalCts = new CancellationTokenSource();
        _processingTask = Task.Run(() => ProcessDiagnosisAsync(_globalCts.Token));
    }

    public Task<Prescription> PrescriptionDemandAsync(Patient patient, HashSet<string> symptoms)
    {
        var res = EnqueueDiagnosisRequest(new DiagnosticRequest(patient, symptoms));

        return res;
    }

    public async Task CancelAllOperations()
    {
        await _globalCts.CancelAsync();
        await _processingTask; // 等待處理任務實際結束

        while (_diagnosisQueue.TryDequeue(out var item))
        {
            item.CompletionSource.TrySetCanceled();
        }
    }

    private static ValidationResult ValidateRequest(Patient patient, HashSet<string> symptoms)
    {
        if (patient == null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        var request = new DiagnosticRequest(patient, symptoms);
        var validation = new DiagnosticRequestValidator();
        var result = validation.Validate(request);

        if (!result.IsValid)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            );
            throw new ArgumentException($"Invalid request: {errors}");
        }

        return result;
    }

    private Task<Prescription> EnqueueDiagnosisRequest(DiagnosticRequest request)
    {
        _diagnosisQueue.Enqueue(request);
        return request.CompletionSource.Task;
    }

    private async Task ProcessDiagnosisAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_diagnosisQueue.TryDequeue(out var item))
            {
                try
                {
                    if (_diagnosticHandler == null)
                    {
                        throw new InvalidOperationException("Diagnostic handler is not set.");
                    }
                    var prescription = await _diagnosticHandler.Diagnosis(item);
                    item.CompletionSource.SetResult(prescription);
                }
                catch (OperationCanceledException ex)
                {
                    // tcs.SetException(ex);
                    item.CompletionSource.TrySetCanceled(cancellationToken);
                }
                catch (Exception ex)
                {
                    item.CompletionSource.TrySetException(ex);
                }
            }
            else
            {
                await Task.Delay(100, _globalCts.Token);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _globalCts.CancelAsync();

        try
        {
            await _processingTask.WaitAsync(TimeSpan.FromSeconds(5)); // 設定超時時間為5秒
        }
        catch (OperationCanceledException)
        {
            // 取消操作導致的異常可以被忽略
        }
        catch (TimeoutException)
        {
            // 記錄超時日誌
        }

        _globalCts.Dispose();
    }

    public void Dispose()
    {
        _globalCts.Cancel();
        _processingTask.Wait(); // 等待處理任務實際結束
        _globalCts.Dispose();
    }
}
