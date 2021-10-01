namespace vault
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IVaultApp : IDisposable
    {
        /// <summary>
        /// Get data by field name.
        /// </summary>
        /// <typeparam name="T">
        /// Data type.
        /// </typeparam>
        /// <param name="key">Field path.</param>
        /// <param name="default">Default value.</param>
        /// <param name="cancellation">Async token.</param>
        /// <returns>
        /// Data or default value when request failed.
        /// </returns>
        ValueTask<T> FieldAsync<T>(string key, T @default, CancellationToken cancellation = default);
        /// <summary>
        /// Get data by field path.
        /// </summary>
        /// <typeparam name="T">
        /// Data type.
        /// </typeparam>
        /// <param name="key">Field path.</param>
        /// <param name="cancellation">Async token.</param>
        /// <returns>
        /// Data.
        /// </returns>
        /// <exception cref="VaultBadRequestException">Field or application not found.</exception>
        /// <exception cref="VaultAccessDeniedException">Need auth token if not passed.</exception>
        ValueTask<T> FieldAsync<T>(string key, CancellationToken cancellation = default);

        /// <summary>
        /// Update data by field path.
        /// </summary>
        /// <typeparam name="T">
        /// Data type.
        /// </typeparam>
        /// <param name="key">Field path.</param>
        /// <param name="data">Target data.</param>
        /// <param name="cancellation">Async token.</param>
        /// <exception cref="VaultBadRequestException">Field or application not found.</exception>
        /// <exception cref="VaultAccessDeniedException">Need auth token if not passed.</exception>
        ValueTask UpdateAsync<T>(string key, T data, CancellationToken cancellation = default);

        /// <summary>
        /// Set auth token for application.
        /// </summary>
        IVaultApp WithToken(string appToken);
    }
}