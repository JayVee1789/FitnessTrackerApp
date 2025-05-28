using Microsoft.JSInterop;

public class UpdateService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private DotNetObjectReference<UpdateService>? _objRef;
    public event Action? OnUpdateAvailable;

    public UpdateService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task RegisterAsync()
    {
        _objRef = DotNetObjectReference.Create(this);
        await _js.InvokeVoidAsync("updateService.register", _objRef);
    }

    [JSInvokable("NotifyUpdateAvailable")]
    public void NotifyUpdateAvailable()
    {
        OnUpdateAvailable?.Invoke();
    }

    public async Task ReloadAsync()
    {
        await _js.InvokeVoidAsync("updateService.reloadApp");
    }

    public async ValueTask DisposeAsync()
    {
        if (_objRef is not null)
        {
            _objRef.Dispose();
        }
    }
}
