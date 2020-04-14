using System;

namespace RJM.API.Framework.Exceptions
{
    public class DocumentException : Exception
    {
        public DocumentException(string message) : base(message) { }
    }

    public class FileServiceException : Exception
    {
        public FileServiceException(string message) : base(message) { }
    }

    #region Authentication

    public class LoginFailedException : Exception
    {
        public LoginFailedException(string message) : base(message) { }
    }

    public class RegistrationFailedException : Exception
    {
        public RegistrationFailedException(string message) : base(message) { }
    }

    public class ConfirmEmailFailedException : Exception
    {
        public ConfirmEmailFailedException(string message) : base(message) { }
    }

    public class ForgotPasswordFailedException : Exception
    {
        public ForgotPasswordFailedException(string message) : base(message) { }
    }

    public class ResetPasswordFailedException : Exception
    {
        public ResetPasswordFailedException(string message) : base(message) { }
    }

    #endregion
}
