using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;

// ReSharper disable CollectionNeverUpdated.Local

namespace ViteFest.Tests;

public class ViteStateTests
{
    private List<ViteResource> _resources;
    private ViteState _sut;

    [TearDown]
    public void TearDown()
    {
        _sut.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        var chunks = new List<ViteManifestChunk>();
        _resources = [];

        var environment = A.Fake<IViteEnvironment>();
        var manifestReader = A.Fake<IViteManifestReader>();
        var resourceMapper = A.Fake<IViteResourceMapper>();

        A.CallTo(() => environment.ManifestFile).Returns("/tmp/manifest.json");
        A.CallTo(() => manifestReader.ReadManifest("/tmp/manifest.json")).Returns(chunks);
        A.CallTo(() => resourceMapper.Map(chunks)).Returns(_resources);

        _sut = new ViteState(environment, manifestReader, resourceMapper);
    }

    [Test]
    public void TryGet_finds_existing_chunk()
    {
        var chunkA = Stub.Resource("a.ts");
        var chunkB = Stub.Resource("b.ts");
        _resources.AddRange(new[] { chunkA, chunkB });
        _sut.Initialize();

        var actualReturn = _sut.TryGet("b.ts", out var actualChunk);

        Assert.Multiple(() =>
        {
            Assert.That(actualReturn, Is.True);
            Assert.That(actualChunk, Is.SameAs(chunkB));
        });
    }

    [Test]
    public void TryGet_is_case_insensitive()
    {
        var chunk = Stub.Resource("CHUNKNaME.ts");
        _resources.AddRange(new[] { chunk });
        _sut.Initialize();

        var actualReturn = _sut.TryGet("chunknAme.ts", out var actualChunk);

        Assert.Multiple(() =>
        {
            Assert.That(actualReturn, Is.True);
            Assert.That(actualChunk, Is.SameAs(chunk));
        });
    }

    [Test]
    public void TryGet_returns_null_when_chunk_is_not_loaded()
    {
        var chunk = Stub.Resource("a.ts");
        _resources.AddRange(new[] { chunk });
        _sut.Initialize();

        var actualReturn = _sut.TryGet("b.ts", out var actualChunk);

        Assert.Multiple(() =>
        {
            Assert.That(actualReturn, Is.False);
            Assert.That(actualChunk, Is.Null);
        });
    }

    [Test]
    public void Initialize_throws_when_called_twice()
    {
        _sut.Initialize();

        var act = new Action(() => _sut.Initialize());

        Assert.That(act, Throws.InvalidOperationException);
    }

    [Test]
    public void TryGet_throws_if_not_initialized()
    {
        var act = new Action(() => _sut.TryGet("a.ts", out _));

        Assert.That(act, Throws.InvalidOperationException);
    }
}
