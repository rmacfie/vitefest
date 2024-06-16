using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ViteFest;

/// <summary>
///     Represents a raw chunk from the Vite manifest file.
///     See https://vitejs.dev/guide/backend-integration
/// </summary>
internal class ViteManifestChunk
{
    [JsonPropertyName("src")]
    public string Src { get; set; } = default!;

    [JsonPropertyName("file")]
    public string File { get; set; } = default!;

    [JsonPropertyName("isEntry")]
    public bool? IsEntry { get; set; }

    [JsonPropertyName("isDynamicEntry")]
    public bool? IsDynamicEntry { get; set; }

    [JsonPropertyName("imports")]
    public List<string>? Imports { get; set; }

    [JsonPropertyName("dynamicImports")]
    public List<string>? DynamicImports { get; set; }

    [JsonPropertyName("assets")]
    public List<string>? Assets { get; set; }

    [JsonPropertyName("css")]
    public List<string>? Css { get; set; }
}
