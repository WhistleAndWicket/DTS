namespace Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an entity does not exist.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a default error message.
        /// </summary>
        public EntityNotFoundException() : base("Entity does not exist.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with the <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with the <paramref name="message"/>
        /// and a reference to the <paramref name="innerException"/> that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
