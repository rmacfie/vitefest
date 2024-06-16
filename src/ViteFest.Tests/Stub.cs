namespace ViteFest.Tests;

internal static class Stub
{
    internal static ViteResource Resource(
        string key,
        string? path = null,
        bool isEntry = false,
        bool isDynamicEntry = false,
        string[]? imports = null,
        string[]? dynamicImports = null,
        string[]? assetPaths = null,
        string[]? cssPaths = null
    )
    {
        return new ViteResource(
            key,
            path ?? "dir/" + key,
            isEntry,
            isDynamicEntry,
            imports ?? [],
            dynamicImports ?? [],
            assetPaths ?? [],
            cssPaths ?? []
        );
    }
}
