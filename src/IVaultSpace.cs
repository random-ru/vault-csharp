namespace vault
{
    public interface IVaultSpace
    {
        IVaultApp App(string app);
        IVaultSpace WithToken(string spaceToken);
    }
}