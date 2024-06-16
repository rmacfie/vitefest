using System.IO;
using NUnit.Framework;

namespace ViteFest.Tests;

public class ViteEnvironmentTests
{
    [Test]
    public void It_uses_complete_values_from_options()
    {
        var env = new ViteEnvironment(
            new ViteOptions
            {
                ManifestFile = "/dev/foo/wwwroot/dist/manifest.json",
                BaseUrl = "/dist/",
                Watch = true
            }
        );

        Assert.Multiple(() =>
        {
            Assert.That(env.ManifestFile, Is.EqualTo("/dev/foo/wwwroot/dist/manifest.json"));
            Assert.That(env.BaseUrl, Is.EqualTo("/dist/"));
            Assert.That(env.Watch, Is.True);
        });
    }

    [Test]
    public void It_combines_web_root_with_relative_manifest_path()
    {
        var env = new ViteEnvironment(
            new ViteOptions { ManifestFile = "dist/manifest.json" },
            "/dev/foo/wwwroot"
        );

        Assert.That(env.ManifestFile, Is.EqualTo("/dev/foo/wwwroot/dist/manifest.json"));
    }

    [Test]
    public void It_defaults_to_root_base_url()
    {
        var env = new ViteEnvironment(new ViteOptions { ManifestFile = "/dev/proj/manifest.json" });

        Assert.That(env.BaseUrl, Is.EqualTo("/"));
    }

    [Test]
    public void It_defaults_to_disable_watch()
    {
        var env = new ViteEnvironment(new ViteOptions { ManifestFile = "/dev/proj/manifest.json" });

        Assert.That(env.Watch, Is.False);
    }

    [Test]
    public void It_falls_back_on_dev_mode_for_watch()
    {
        var env = new ViteEnvironment(
            new ViteOptions { ManifestFile = "/dev/proj/manifest.json" },
            isDevelopment: true
        );

        Assert.That(env.Watch, Is.True);
    }

    [Test]
    public void It_falls_back_on_current_dir_for_web_root()
    {
        var env = new ViteEnvironment(new ViteOptions { ManifestFile = "dist/manifest.json" });

        Assert.That(
            env.ManifestFile,
            Is.EqualTo(Path.Combine(Directory.GetCurrentDirectory(), "dist/manifest.json"))
        );
    }
}
