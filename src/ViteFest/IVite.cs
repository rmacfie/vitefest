using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ViteFest;

public interface IVite : IDisposable
{
    string? GetUrl(string key);

    IReadOnlyCollection<string> GetImports(string key);

    IReadOnlyCollection<string> GetDynamicImports(string key);

    IReadOnlyCollection<string> GetAssetUrls(string key);

    IReadOnlyCollection<string> GetCssUrls(string key);

    bool TryGetUrl(string key, [NotNullWhen(true)] out string? path);

    bool TryGetImports(string key, [NotNullWhen(true)] out IReadOnlyCollection<string>? imports);

    bool TryGetDynamicImports(
        string key,
        [NotNullWhen(true)] out IReadOnlyCollection<string>? dynamicImports
    );

    bool TryGetAssetUrls(
        string key,
        [NotNullWhen(true)] out IReadOnlyCollection<string>? assetPaths
    );

    bool TryGetCssUrls(string key, [NotNullWhen(true)] out IReadOnlyCollection<string>? cssPaths);
}

public sealed class Vite : IVite
{
    private readonly IViteState _state;

    internal Vite(IViteState state)
    {
        _state = state;
    }

    public string? GetUrl(string key)
    {
        return TryGetUrl(key, out var path) ? path : default;
    }

    public IReadOnlyCollection<string> GetImports(string key)
    {
        return TryGetImports(key, out var imports) ? imports : Array.Empty<string>();
    }

    public IReadOnlyCollection<string> GetDynamicImports(string key)
    {
        return TryGetDynamicImports(key, out var imports) ? imports : Array.Empty<string>();
    }

    public IReadOnlyCollection<string> GetAssetUrls(string key)
    {
        return TryGetAssetUrls(key, out var paths) ? paths : Array.Empty<string>();
    }

    public IReadOnlyCollection<string> GetCssUrls(string key)
    {
        return TryGetCssUrls(key, out var paths) ? paths : Array.Empty<string>();
    }

    public bool TryGetUrl(string key, [NotNullWhen(true)] out string? path)
    {
        if (!_state.TryGet(key, out var chunk))
        {
            path = default;
            return false;
        }

        path = chunk.Url;
        return true;
    }

    public bool TryGetImports(
        string key,
        [NotNullWhen(true)] out IReadOnlyCollection<string>? imports
    )
    {
        if (!_state.TryGet(key, out var chunk))
        {
            imports = default;
            return false;
        }

        imports = chunk.Imports;
        return true;
    }

    public bool TryGetDynamicImports(
        string key,
        [NotNullWhen(true)] out IReadOnlyCollection<string>? dynamicImports
    )
    {
        if (!_state.TryGet(key, out var chunk))
        {
            dynamicImports = default;
            return false;
        }

        dynamicImports = chunk.DynamicImports;
        return true;
    }

    public bool TryGetAssetUrls(
        string key,
        [NotNullWhen(true)] out IReadOnlyCollection<string>? assetPaths
    )
    {
        if (!_state.TryGet(key, out var chunk))
        {
            assetPaths = default;
            return false;
        }

        assetPaths = chunk.AssetUrls;
        return true;
    }

    public bool TryGetCssUrls(
        string key,
        [NotNullWhen(true)] out IReadOnlyCollection<string>? cssPaths
    )
    {
        if (!_state.TryGet(key, out var chunk))
        {
            cssPaths = default;
            return false;
        }

        cssPaths = chunk.CssUrls;
        return true;
    }

    public void Dispose()
    {
        _state.Dispose();
    }

    public static IVite Create(Action<ViteOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new ViteOptions();
        configure(options);
        return Create(options);
    }

    public static IVite Create(ViteOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        options.Validate();

        var env = new ViteEnvironment(options);
        var manifestReader = new ViteManifestReader();
        var resourceMapper = new ViteResourceMapper(env);
        var state = new ViteState(env, manifestReader, resourceMapper);

        state.Initialize();

        return new Vite(state);
    }
}
