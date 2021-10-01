namespace vault
{
    using System;
    using System.Collections.Generic;

    public class Vault : IVault
    {
        private readonly Dictionary<string, IVaultSpace> _spaces = new ();
        internal string _baseUrl;

        internal Vault(string domain) => _baseUrl = $"https://{domain}";

        public IVaultSpace Space(string space)
        {
            if (string.IsNullOrEmpty(space)) 
                throw new ArgumentNullException(nameof(space));
            if (_spaces.ContainsKey(space))
                return _spaces[space];
            return _spaces[space] = new VaultSpace(this, space);
        }

        public static IVault Create(string domain)
        {
            if (string.IsNullOrEmpty(domain)) throw new ArgumentNullException(nameof(domain));

            return new Vault(domain);
        }
    }
}
