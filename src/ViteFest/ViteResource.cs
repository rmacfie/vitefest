using System.Collections.Generic;

namespace ViteFest
{
    internal sealed class ViteResource
    {
        public ViteResource(
            string key,
            string url,
            bool isEntry,
            bool isDynamicEntry,
            IReadOnlyCollection<string> imports,
            IReadOnlyCollection<string> dynamicImports,
            IReadOnlyCollection<string> assetUrls,
            IReadOnlyCollection<string> cssUrls
        )
        {
            Key = key;
            Url = url;
            IsEntry = isEntry;
            IsDynamicEntry = isDynamicEntry;
            Imports = imports;
            DynamicImports = dynamicImports;
            AssetUrls = assetUrls;
            CssUrls = cssUrls;
        }

        /// <summary>
        ///     The key of the chunk, which usually is the path of the source file,
        ///     relative to the project root.
        /// </summary>
        /// <example>
        ///     <c>"Client/main.ts"</c>
        /// </example>
        public string Key { get; }

        /// <summary>
        ///     The public absolute path to the chunk, for use in the client.
        /// </summary>
        /// <remarks>
        ///     This corresponds to the <c>file</c> property in the Vite manifest.
        /// </remarks>
        /// <example>
        ///     <c>"/dist/assets/main.oauo411t.js"</c>
        /// </example>
        public string Url { get; }

        public bool IsEntry { get; }

        public bool IsDynamicEntry { get; }

        /// <summary>
        ///     Scripts imported by this source. The values are the keys of the chunks
        ///     that details the imported scripts.
        /// </summary>
        /// <example>
        ///     <c>["Components/Foo.ts", "Components/Bar.ts"]</c>
        /// </example>
        public IReadOnlyCollection<string> Imports { get; }

        /// <summary>
        ///     Scripts dynamically imported by this source. The values are the keys of the chunks
        ///     that details the imported scripts.
        /// </summary>
        /// <example>
        ///     <c>["Components/Foo.ts", "Components/Bar.ts"]</c>
        /// </example>
        public IReadOnlyCollection<string> DynamicImports { get; }

        /// <summary>
        ///     Asset files imported by this source. The values are the public paths of the files,
        ///     relative to base, for use in the client.
        ///     This corresponds to the <c>assets</c> property in the Vite manifest.
        /// </summary>
        /// <example>
        ///     <c>["/dist/assets/Photo1.nsha62mx.jpg", "/dist/assets/Logotype.9adf52l.svg"]</c>
        /// </example>
        public IReadOnlyCollection<string> AssetUrls { get; }

        /// <summary>
        ///     CSS files imported by this source. The values are the public paths of the files,
        ///     relative to base, for use in the client.
        ///     This corresponds to the <c>css</c> property in the Vite manifest.
        /// </summary>
        /// <example>
        ///     <c>["/dist/assets/main.nsha62mx.css", "/dist/assets/main.9adf52l.css"]</c>
        /// </example>
        public IReadOnlyCollection<string> CssUrls { get; }
    }
}
