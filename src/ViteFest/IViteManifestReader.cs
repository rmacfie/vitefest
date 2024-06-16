using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ViteFest
{
    internal interface IViteManifestReader
    {
        IReadOnlyCollection<ViteManifestChunk> ReadManifest(string manifestPath);
    }

    internal class ViteManifestReader : IViteManifestReader
    {
        private static JsonSerializerOptions JsonOptions { get; } = new();

        public IReadOnlyCollection<ViteManifestChunk> ReadManifest(string path)
        {
            var absolutePath = Path.GetFullPath(path);
            var json = File.ReadAllText(absolutePath);
            var chunks = JsonSerializer.Deserialize<Dictionary<string, ViteManifestChunk>>(
                json,
                JsonOptions
            );

            if (chunks is null)
            {
                throw new Exception($"The manifest file was empty ('{absolutePath}')");
            }

            return chunks.Values;
        }
    }
}
