using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;

namespace ViteFest;

internal interface IViteState : IDisposable
{
    void Initialize();
    bool TryGet(string key, [NotNullWhen(true)] out ViteResource? resource);
}

internal sealed class ViteState : IViteState
{
    private readonly IViteEnvironment _environment;
    private readonly IViteManifestReader _manifestReader;
    private readonly IViteResourceMapper _resourceMapper;

    private Timer? _debouncer;
    private Dictionary<string, ViteResource>? _state;
    private FileSystemWatcher? _watcher;

    public ViteState(
        IViteEnvironment environment,
        IViteManifestReader manifestReader,
        IViteResourceMapper resourceMapper
    )
    {
        _environment = environment;
        _manifestReader = manifestReader;
        _resourceMapper = resourceMapper;
    }

    public void Initialize()
    {
        if (_state != null)
        {
            throw new InvalidOperationException(
                "The registry can't be initialized more than once."
            );
        }

        Load();

        if (_environment.Watch)
        {
            _debouncer = new Timer(_ => Load());
            _watcher = new FileSystemWatcher(
                Path.GetDirectoryName(_environment.ManifestFile)!,
                Path.GetFileName(_environment.ManifestFile)
            )
            {
                NotifyFilter = NotifyFilters.LastWrite
            };

            _watcher.Changed += (_, _) =>
                _debouncer.Change(TimeSpan.FromMilliseconds(200), Timeout.InfiniteTimeSpan);

            _watcher.EnableRaisingEvents = true;
        }
    }

    public bool TryGet(string key, [NotNullWhen(true)] out ViteResource? resource)
    {
        if (_state == null)
        {
            throw new InvalidOperationException("The registry must be initialized before use.");
        }

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        var normalizedKey = key.TrimStart('/');
        return _state.TryGetValue(normalizedKey, out resource);
    }

    public void Dispose()
    {
        _watcher?.Dispose();
        _debouncer?.Dispose();
    }

    private void Load()
    {
        var chunks = _manifestReader.ReadManifest(_environment.ManifestFile);
        var resources = _resourceMapper.Map(chunks);
        _state = resources.ToDictionary(x => x.Key, x => x, StringComparer.OrdinalIgnoreCase);
    }
}
