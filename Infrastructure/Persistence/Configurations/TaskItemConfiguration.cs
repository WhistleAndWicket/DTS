using Domain.Entities.Dts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuration for the <see cref="TaskItem"/> entity mapping to the database.
    /// Defines table name, primary key and column properties.
    /// </summary>
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            // Table name.
            builder.ToTable("Tasks");

            // Primary key.
            builder.HasKey(t => t.Id).HasName("PRIMARY");

            // Properties.
            builder.Property(t => t.Title).IsRequired();
            builder.Property(t => t.Status).IsRequired();
            builder.Property(t => t.DueDate).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            // Store enum as string instead of int
            builder.Property(t => t.Status)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
