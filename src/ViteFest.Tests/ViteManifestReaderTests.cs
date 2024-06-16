using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using NUnit.Framework;

namespace ViteFest.Tests;

public class ViteManifestReaderTests
{
    private const string ManifestFile = "TestFiles/manifest.json";
    private const string ManifestEmptyFile = "TestFiles/manifest-empty.json";
    private const string ManifestInvalidFile = "TestFiles/manifest-invalid.json";
    private const string ManifestNonExistentFile = "TestFiles/manifest-not-exists.json";

    private ViteManifestReader _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new ViteManifestReader();
    }

    [Test]
    public void It_reads_a_manifest_file()
    {
        // act
        var manifest = _sut.ReadManifest(ManifestFile);

        // assert
        Assert.That(manifest, Is.Not.Null);
    }

    [Test]
    public void It_maps_an_entry_script_chunk()
    {
        var manifest = _sut.ReadManifest(ManifestFile);

        // act
        var chunk = manifest.FirstOrDefault(x => x.Src == "Components/Home.ts")!;

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(chunk, Is.Not.Null);
            Assert.That(chunk.File, Is.EqualTo("assets/Home-B2lgNECc.js"));
            Assert.That(chunk.IsEntry, Is.True);
            Assert.That(chunk.IsDynamicEntry, Is.Null);
            Assert.That(chunk.Imports, Is.Null);
            Assert.That(chunk.DynamicImports, Is.Null);
            Assert.That(chunk.Assets, Is.EquivalentTo(new[] { "assets/Logotype-BQihiMnY.svg" }));
            Assert.That(chunk.Css, Is.EquivalentTo(new[] { "assets/Home-Bs7ZzuBZ.css" }));
        });
    }

    [Test]
    public void It_maps_an_entry_css_chunk()
    {
        var manifest = _sut.ReadManifest(ManifestFile);

        // act
        var chunk = manifest.FirstOrDefault(x => x.Src == "Components/Root.css")!;

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(chunk, Is.Not.Null);
            Assert.That(chunk.File, Is.EqualTo("assets/Root-B-NYCzS9.css"));
            Assert.That(chunk.IsEntry, Is.True);
            Assert.That(chunk.IsDynamicEntry, Is.Null);
            Assert.That(chunk.Imports, Is.Null);
            Assert.That(chunk.DynamicImports, Is.Null);
            Assert.That(chunk.Assets, Is.Null);
            Assert.That(chunk.Css, Is.Null);
        });
    }

    [Test]
    public void It_maps_an_asset_chunk()
    {
        var manifest = _sut.ReadManifest(ManifestFile);

        // act
        var chunk = manifest.FirstOrDefault(x => x.Src == "Components/Common/Logotype.svg")!;

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(chunk, Is.Not.Null);
            Assert.That(chunk.File, Is.EqualTo("assets/Logotype-BQihiMnY.svg"));
            Assert.That(chunk.IsEntry, Is.Null);
            Assert.That(chunk.IsDynamicEntry, Is.Null);
            Assert.That(chunk.Imports, Is.Null);
            Assert.That(chunk.DynamicImports, Is.Null);
            Assert.That(chunk.Assets, Is.Null);
            Assert.That(chunk.Css, Is.Null);
        });
    }

    [Test]
    public void It_throws_when_manifest_file_is_empty()
    {
        // act
        var act = new Action(() => _sut.ReadManifest(ManifestEmptyFile));

        // assert
        Assert.That(act, Throws.TypeOf<JsonException>());
    }

    [Test]
    public void It_throws_when_manifest_file_is_invalid()
    {
        // act
        var act = new Action(() => _sut.ReadManifest(ManifestInvalidFile));

        // assert
        Assert.That(act, Throws.TypeOf<JsonException>());
    }

    [Test]
    public void It_throws_when_manifest_file_is_missing()
    {
        // act
        var act = new Action(() => _sut.ReadManifest(ManifestNonExistentFile));

        // assert
        Assert.That(act, Throws.TypeOf<FileNotFoundException>());
    }
}
