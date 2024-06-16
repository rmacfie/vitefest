using System;

namespace ViteFest
{
    public class ViteOptions
    {
        /// <summary>
        ///     The file system path of the manifest file.
        ///     When a relative path is provided, it is resolved relative to the application's
        ///     content root path.
        /// </summary>
        public string ManifestFile { get; set; } = default!;

        /// <summary>
        ///     The base public path, or base URL, to the Vite output directory.
        ///     Defaults to <c>"/"</c>.
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        ///     Indicates whether to watch and reload the manifest file when it changes.
        ///     Defaults to <c>false</c> in Production environments and <c>true</c> in Development.
        /// </summary>
        public bool? Watch { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ManifestFile))
            {
                throw new Exception($"The {nameof(ManifestFile)} option is required");
            }
        }
    }
}
