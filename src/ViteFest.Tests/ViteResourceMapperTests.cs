using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;

namespace ViteFest.Tests;

public class ViteResourceMapperTests
{
    private IViteEnvironment _environment;
    private ViteResourceMapper _sut;

    [SetUp]
    public void SetUp()
    {
        _environment = A.Fake<IViteEnvironment>();
        A.CallTo(() => _environment.BaseUrl).Returns("/dist/");

        _sut = new ViteResourceMapper(_environment);
    }

    [Test]
    public void It_maps_all_values()
    {
        var resources = _sut.Map(
            new[]
            {
                new ViteManifestChunk
                {
                    Src = "a.ts",
                    File = "assets/a.js",
                    IsEntry = true,
                    IsDynamicEntry = true,
                    Imports = new List<string> { "b.ts" },
                    DynamicImports = new List<string> { "c.ts" },
                    Css = new List<string> { "assets/styles.css" },
                    Assets = new List<string> { "assets/image.png" }
                }
            }
        );

        Assert.Multiple(() =>
        {
            Assert.That(resources, Has.Count.EqualTo(1));

            var resource = resources.Single();

            Assert.That(resource.Key, Is.EqualTo("a.ts"));
            Assert.That(resource.Url, Is.EqualTo("/dist/assets/a.js"));
            Assert.That(resource.IsEntry, Is.True);
            Assert.That(resource.IsDynamicEntry, Is.True);
            Assert.That(resource.Imports, Is.EquivalentTo(new[] { "b.ts" }));
            Assert.That(resource.DynamicImports, Is.EquivalentTo(new[] { "c.ts" }));
            Assert.That(resource.CssUrls, Is.EquivalentTo(new[] { "/dist/assets/styles.css" }));
            Assert.That(resource.AssetUrls, Is.EquivalentTo(new[] { "/dist/assets/image.png" }));
        });
    }

    [Test]
    public void It_maps_empty_optional_values()
    {
        var resources = _sut.Map(
            new[]
            {
                new ViteManifestChunk { Src = "a.ts", File = "assets/a.js" }
            }
        );

        Assert.Multiple(() =>
        {
            Assert.That(resources, Has.Count.EqualTo(1));

            var resource = resources.Single();

            Assert.That(resource.Key, Is.EqualTo("a.ts"));
            Assert.That(resource.Url, Is.EqualTo("/dist/assets/a.js"));
            Assert.That(resource.IsEntry, Is.False);
            Assert.That(resource.IsDynamicEntry, Is.False);
            Assert.That(resource.Imports, Is.Empty);
            Assert.That(resource.DynamicImports, Is.Empty);
            Assert.That(resource.CssUrls, Is.Empty);
            Assert.That(resource.AssetUrls, Is.Empty);
        });
    }
}
