using System;

namespace FinancialPortal.Web.Models.Exceptions
{
    //
    // Special exception
    //
    // When thrown, will be caught by global exception handler, resulting in redirection to My Accounts
    // Because My Accounts is 'authorised' this will result in a further redirection to the Home (Login)
    // page, if the user is anonymous (not logged in)
    //
    // This will happen if My Accounts (or DPA) is bypassed or a stale surrogate key (GUID) is used
    // during navigation. Guids are refreshed on every login or after DPA session expiry.
    // See IApplicationSessionState LowellReferenceSurrogateKey methods.
    //
    public class LowellReferenceSurrogateKeyException : Exception
    {
        public LowellReferenceSurrogateKeyException(string message) : base(message)
        {
        }
    }
}
