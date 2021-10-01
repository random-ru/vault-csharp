namespace vault
{
    using System;
    using System.Collections.Generic;
    using MoreLinq;

    internal class VaultSpace : IVaultSpace, IDisposable
    {
        internal readonly string _spaceId;
        internal readonly Vault _core;
        internal string _token;

        private readonly Dictionary<string, IVaultApp> _apps = new ();


        public VaultSpace(Vault core, string spaceID)
        {
            _core = core;
            _spaceId = spaceID;
        }

        public IVaultApp App(string app)
        {
            if (string.IsNullOrEmpty(app)) 
                throw new ArgumentNullException(nameof(app));
            if (_apps.ContainsKey(app))
                return _apps[app];
            return new VaultApp(this, app);
        }

        public IVaultSpace WithToken(string spaceToken)
        {
            _token = spaceToken;
            return this;
        }

        public void Dispose() 
            => _apps
                .Values
                .Pipe(x => x.Dispose())
                .Consume();
    }
}