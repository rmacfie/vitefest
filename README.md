# ViteFest

Utilize Vite build assets in ASP.NET Core and other .NET applications.

![ViteFest.AspNetCore](https://img.shields.io/nuget/vpre/ViteFest.AspNetCore)


## ASP.NET Core

`vite.config.ts`:
```typescript
import { defineConfig } from 'vite';

export default defineConfig({
  base: '/dist/',
  build: {
    outDir: 'wwwroot/dist',
    manifest: 'vite-manifest.json',
    rollupOptions: {
      input: ['ClientApp/shared.ts', 'ClientApp/login.ts']
    }
  }
});
```

`Program.cs`:
```csharp
builder.Services.AddViteFest(o =>
{
    o.ManifestFile = "dist/vite-manifest.json";
    o.BaseUrl = "/dist/";
});
```


`SomePage.cshtml` or `SomePage.razor`:
```html
@inject ViteFest.IVite Vite

<script src='@Vite.GetUrl("ClientApp/shared.ts")'></script>
<script src='@Vite.GetUrl("ClientApp/login.ts")'></script>

@foreach (var cssUrl in Vite.GetCssUrls("ClientApp/shared.ts"))
{
  <link rel="stylesheet" href="@cssUrl" />
}
```
