using Domain.Entities.Dts;

namespace Tests.Unit.Helpers
{
    /// <summary>
    /// Provides helper methods for generating test data related to <see cref="TaskItem"/>.
    /// </summary>
    public static class TaskItemTestHelper
    {
        private static readonly Random _randomizer = new();

        /// <summary>
        /// Generates a dummy <see cref="TaskItem"/> with random values for testing purposes.
        /// </summary>
        /// <returns>A new instance of <see cref="TaskItem"/> populated with test values.</returns>
        public static TaskItem GetTaskItem(string? title = null)
        {
            var taskTitle = title ?? $"Task-{_randomizer.Next(1000, 9999)}";
            var taskDescription = $"Description-{_randomizer.Next(1000, 9999)}";

            return new TaskItem()
            {
                Description = taskDescription,
                DueDate = DateTime.UtcNow.AddDays(_randomizer.Next(1, 30)),
                Title = taskTitle,
                Id = _randomizer.Next()
            };
        }
    }
}
