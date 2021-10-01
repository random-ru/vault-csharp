namespace vault
{
    using System;

    public class VaultBadRequestException : VaultException
    {
        public VaultBadRequestException(string message) : base(message)  { }
    }
    public class VaultAccessDeniedException : VaultException
    {
        public VaultAccessDeniedException(string message) : base(message)  { }
    }
    public class VaultException : Exception
    {
        public VaultException(string message) : base(message)  { }
    }
}