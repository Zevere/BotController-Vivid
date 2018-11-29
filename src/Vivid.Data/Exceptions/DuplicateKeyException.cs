using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Vivid.Data
{
    /// <summary>
    /// Unique index key(s) violation error
    /// </summary>
    public class DuplicateKeyException : Exception
    {
        /// <summary>
        /// Name of the unique key(s)
        /// </summary>
        public IEnumerable<string> Keys { get; }

        /// <inheritdoc />
        public DuplicateKeyException(params string[] keys)
            : base(string.Format(@"Duplicate key{0}: ""{1}""",
                keys.Length > 1 ? "s" : string.Empty,
                string.Join(", ", keys)
            ))
        {
            Keys = keys;
        }
    }
}
