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
    private bool _disposed;

    public Prescriber(IDiagnosticHandler? diagnosticHandler)
    {
        _diagnosisQueue = new ConcurrentQueue<DiagnosticRequest>();
        _diagnosticHandler = diagnosticHandler;
        _globalCts = new CancellationTokenSource();
        _processingTask = Task.Run(() => ProcessDiagnosisAsync(_globalCts.Token));
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(Prescriber));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    private async ValueTask DisposeAsyncCore()
    {
        _globalCts.Cancel();

        try
        {
            await _processingTask.WaitAsync(TimeSpan.FromSeconds(5));
        }
        catch (OperationCanceledException)
        {
            // Cancellation is expected, ignore
        }
        catch (TimeoutException)
        {
            // Log timeout
        }

        _globalCts.Dispose();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            DisposeAsyncCore().AsTask().GetAwaiter().GetResult();
        }

        _disposed = true;
    }

    ~Prescriber()
    {
        Dispose(disposing: false);
    }

    public Task<Prescription> PrescriptionDemandAsync(Patient patient, HashSet<string> symptoms)
    {
        ThrowIfDisposed();

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
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        _diagnosisQueue.Enqueue(request);
        return request.CompletionSource.Task;
    }

    private async Task ProcessDiagnosisAsync(CancellationToken cancellationToken)
    {
        try
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
                    catch (OperationCanceledException)
                    {
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
        catch (Exception ex)
        {
            // 處理異常的全局處理邏輯
        }
    }
}
