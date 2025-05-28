using System;

namespace Gefco.CipQuai.Web.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string userId) : base($"Unknown User {userId}")
        {
            
        }
    }
    public class InvalidDeclarationException : Exception
    {
        public InvalidDeclarationException(string declarationId) : base($"Unknown Declaration {declarationId}")
        {
            
        }
    }
    public class DeclarationInProgressException : Exception
    {
        public DeclarationInProgressException() : base($"La déclaration est en cours d'utilisation")
        {
            
        }
    }
    public class DeclarationExistsException : Exception
    {
        public DeclarationExistsException() : base($"La déclaration a déjà été créée")
        {
            
        }
    }
}