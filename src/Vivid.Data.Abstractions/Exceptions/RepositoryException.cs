using System;

// ReSharper disable once CheckNamespace
namespace Vivid.Data.Abstractions
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message)
            : base(message)
        {
        }
    }
}