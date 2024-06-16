using System;
using System.IO;

namespace ViteFest
{
    public interface IViteEnvironment
    {
        string ManifestFile { get; }

        string BaseUrl { get; }

        bool Watch { get; }
    }

    public class ViteEnvironment : IViteEnvironment
    {
        public ViteEnvironment(
            ViteOptions options,
            string? webRootPath = null,
            bool isDevelopment = false
        )
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            ManifestFile = Path.IsPathRooted(options.ManifestFile)
                ? options.ManifestFile
                : Path.Combine(
                    webRootPath ?? Directory.GetCurrentDirectory(),
                    options.ManifestFile
                );
            BaseUrl = options.BaseUrl ?? "/";
            Watch = options.Watch ?? isDevelopment;
        }

        public string ManifestFile { get; }
        public string BaseUrl { get; }
        public bool Watch { get; }
    }
}
