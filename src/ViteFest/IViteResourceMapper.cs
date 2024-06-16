using System;
using System.Collections.Generic;
using System.Linq;

namespace ViteFest
{
    internal interface IViteResourceMapper
    {
        IReadOnlyCollection<ViteResource> Map(IEnumerable<ViteManifestChunk> chunks);
    }

    internal class ViteResourceMapper : IViteResourceMapper
    {
        private readonly IViteEnvironment _env;

        public ViteResourceMapper(IViteEnvironment env)
        {
            _env = env;
        }

        public IReadOnlyCollection<ViteResource> Map(IEnumerable<ViteManifestChunk> chunks)
        {
            return chunks.Select(x => Map(x, _env.BaseUrl)).ToArray();
        }

        private static ViteResource Map(ViteManifestChunk chunk, string baseUrl)
        {
            return new ViteResource(
                chunk.Src,
                $"{baseUrl}{chunk.File}",
                chunk.IsEntry ?? false,
                chunk.IsDynamicEntry ?? false,
                chunk.Imports?.ToArray() ?? Array.Empty<string>(),
                chunk.DynamicImports?.ToArray() ?? Array.Empty<string>(),
                chunk.Assets?.Select(path => $"{baseUrl}{path}").ToArray() ?? Array.Empty<string>(),
                chunk.Css?.Select(path => $"{baseUrl}{path}").ToArray() ?? Array.Empty<string>()
            );
        }
    }
}
