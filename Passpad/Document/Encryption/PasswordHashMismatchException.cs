using System;

namespace Passpad.Encryption
{
	class PasswordHashMismatchException : Exception
	{
		public PasswordHashMismatchException() : base() { }
		public PasswordHashMismatchException(string message) : base(message) { }
		public PasswordHashMismatchException(string message, Exception innerException) : base(message, innerException) { }
	}
}
