namespace Domain.Entities.Base
{
    /// <summary>
    /// Base class for all domain entities. Provides identity and equality handling.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// The primary key identifier.
        /// </summary>
        public int Id { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        /// <summary>
        /// Determines whether two <see cref="Entity"/> instances are equal.
        /// </summary>
        /// <param name="a">The first entity to compare.</param>
        /// <param name="b">The second entity to compare.</param>
        /// <returns><c>true</c> if both entities are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two <see cref="Entity"/> instances are not equal.
        /// </summary>
        /// <param name="a">The first entity to compare.</param>
        /// <param name="b">The second entity to compare.</param>
        /// <returns><c>true</c> if both entities are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Entity? a, Entity? b) => !(a == b);

        /// <inheritdoc/>
        public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();

        /// <summary>
        /// Checks if the entity has not been persisted yet.
        /// </summary>
        public bool IsTransient() => Id == 0;

        /// <inheritdoc/>
        public override string ToString() => $"{GetType().Name} Id={Id}";
    }
}
