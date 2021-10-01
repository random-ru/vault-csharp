namespace vault
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;

    public static class ValueTaskExtensions
    {
        public static async ValueTask<T> TryAsync<T>(this ValueTask<T> task, T @default)
        {
            try
            {
                return await task;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return @default;
            }
        }
    }

    public static class FlurlExtensions
    {
        public static IFlurlRequest WithRequest(this string url, IFlurlClient client) 
            => client.Request(url);

    }
}