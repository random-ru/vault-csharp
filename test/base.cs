namespace test
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Flurl.Http.Testing;
    using NUnit.Framework;
    using vault;

    [TestFixture]
    public class @base
    {
        public static string _SPACE_ = "test_space";
        public static string _APP_ = "test_app";

        [Test]
        public void InitTest() =>
            Assert.DoesNotThrow(() => Vault.Create("vault.api")
                .Space(_SPACE_)
                .App(_APP_));

        [Test]
        public async Task SuccessTest()
        {
            using var vault = Vault.Create("vault.api")
                .Space(_SPACE_)
                .App(_APP_);
            using var httpTest = new HttpTest();
            var target = new[] { "s1", "s2" };
            httpTest.RespondWithJson(target);
            var result = await vault.FieldAsync("array", new []{ "incorrect" });
            httpTest.ShouldHaveCalled("https://vault.api/*")
                .WithVerb(HttpMethod.Get)
                .WithHeader("Authorization");

            Assert.AreEqual(result, target);
        }

        [Test]
        public void TestBadRequest()
        {
            using var vault = Vault.Create("vault.api")
                .Space(_SPACE_)
                .App(_APP_);
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(new { message = "error" }, 400);
            Assert.ThrowsAsync<VaultBadRequestException>(async () => await vault.FieldAsync<string[]>("array"));
            httpTest.ShouldHaveCalled("https://vault.api/*")
                .WithVerb(HttpMethod.Get)
                .WithHeader("Authorization");
        }

        [Test]
        public void TestAccessDenied()
        {
            using var vault = Vault.Create("vault.api")
                .Space(_SPACE_)
                .App(_APP_);
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(new { message = "error" }, 418);
            Assert.ThrowsAsync<VaultAccessDeniedException>(async () => await vault.FieldAsync<string[]>("array"));
            httpTest.ShouldHaveCalled("https://vault.api/*")
                .WithVerb(HttpMethod.Get)
                .WithHeader("Authorization");
        }

        [Test]
        public void TestUpdate()
        {
            using var vault = Vault.Create("vault.api")
                .Space(_SPACE_)
                .App(_APP_);
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(new { message = "ok" });
            Assert.DoesNotThrowAsync(async () => await vault.UpdateAsync("array", new [] {"test"}));
            httpTest.ShouldHaveCalled("https://vault.api/*")
                .WithVerb(HttpMethod.Put)
                .WithHeader("Authorization");
        }
    }
}