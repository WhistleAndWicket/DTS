namespace Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an entity already exists.
    /// </summary>
    public class EntityAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAlreadyExistsException"/> class with a default error message.
        /// </summary>
        public EntityAlreadyExistsException() : base("Entity already exists.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAlreadyExistsException"/> class with the <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntityAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAlreadyExistsException"/> class with the <paramref name="message"/>
        /// and a reference to the <paramref name="innerException"/> that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EntityAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
