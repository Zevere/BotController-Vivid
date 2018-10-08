using System;

// ReSharper disable once CheckNamespace
namespace Vivid.Data
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message)
            : base(message)
        {
        }
    }
}
