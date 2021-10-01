namespace vault
{
    using System.Threading;
    using System.Threading.Tasks;
    using Flurl.Http;

    internal class VaultApp : IVaultApp
    {
        private readonly VaultSpace _core;
        private readonly string _appID;
        private readonly IFlurlClient _client;
        internal string _token;

        protected string Namespace => _core._spaceId;
        protected string App => _appID;
        
        public VaultApp(VaultSpace core, string appID)
        {
            _core = core;
            _appID = appID;
            _client = new FlurlClient(core._core._baseUrl)
                .WithHeader("User-Agent", "Flurl/3.2");
        }
        /// <inheritdoc/>
        public ValueTask<T> FieldAsync<T>(string key, T @default, CancellationToken cancellation = default) 
            => FieldAsync<T>(key, cancellation).TryAsync(@default);
        /// <inheritdoc/>
        public async ValueTask<T> FieldAsync<T>(string key, CancellationToken cancellation = default)
        {
            var result = await $"/@/{Namespace}/{App}/{key}"
                .WithRequest(_client)
                .WithHeader("Authorization", getAuthKey())
                .AllowHttpStatus("400,418")
                .GetAsync(cancellation);

            if (result is { StatusCode: 200 })
                return await result.GetJsonAsync<T>();
            var error = await result.GetJsonAsync();

            throw result switch
            {
                { StatusCode: 400 } => new VaultBadRequestException(error.message),
                { StatusCode: 418 } => new VaultAccessDeniedException(error.message),
                _ => new VaultException(await result.GetStringAsync())
            };
        }
        /// <inheritdoc/>
        public async ValueTask UpdateAsync<T>(string key, T data, CancellationToken cancellation = default)
        {
            var result = await $"/@/{Namespace}/{App}/{key}"
                .WithRequest(_client)
                .WithHeader("Authorization", getAuthKey())
                .PutJsonAsync(data, cancellation);

            if (result is { StatusCode: 200 })
                return;

            var error = await result.GetJsonAsync();

            throw result switch
            {
                { StatusCode: 400 } => new VaultBadRequestException(error.message),
                { StatusCode: 418 } => new VaultAccessDeniedException(error.message),
                _ => new VaultException(await result.GetStringAsync())
            };
        }
        /// <inheritdoc/>
        public IVaultApp WithToken(string appToken)
        {
            _token = appToken;
            return this;
        }
        /// <inheritdoc/>
        public void Dispose() => _client.Dispose();
        private string getAuthKey() => $"{_core._token}.{_token}";
    }
}