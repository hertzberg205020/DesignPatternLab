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
    private readonly SemaphoreSlim _semaphore;
    private readonly Task[] _processingTasks;
    private const int MaxConcurrentTasks = 4; // 可以根據需要調整
    private bool _disposed;

    // 預設最大同時執行緒數量
    private const int DefaultMaxConcurrentTasks = 15;

    public Prescriber(
        IDiagnosticHandler? diagnosticHandler,
        int maxConcurrentTasks = DefaultMaxConcurrentTasks
    )
    {
        _diagnosisQueue = new ConcurrentQueue<DiagnosticRequest>();
        _diagnosticHandler = diagnosticHandler;
        _globalCts = new CancellationTokenSource();
        _semaphore = new SemaphoreSlim(maxConcurrentTasks);
        _processingTasks = new Task[maxConcurrentTasks];

        for (var i = 0; i < maxConcurrentTasks; i++)
        {
            _processingTasks[i] = Task.Run(() => ProcessDiagnosisAsync(_globalCts.Token));
        }
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
            await Task.WhenAll(_processingTasks).WaitAsync(TimeSpan.FromSeconds(5));
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
        _semaphore.Dispose();
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
        await Task.WhenAll(_processingTasks);

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
        while (!cancellationToken.IsCancellationRequested)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
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
                    await Task.Delay(100, cancellationToken);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
