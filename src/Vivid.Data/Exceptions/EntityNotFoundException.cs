using System;

// ReSharper disable once CheckNamespace
namespace Vivid.Data
{
    /// <summary>
    /// Entity not found error
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <inheritdoc />
        public EntityNotFoundException(string id)
            : base($"Entity with id of \"{id}\" does not exist.") { }

        /// <inheritdoc />
        public EntityNotFoundException(string field, string value)
            : base($"Entity with \"{field}\" of \"{value}\" does not exist.") { }
    }
}
