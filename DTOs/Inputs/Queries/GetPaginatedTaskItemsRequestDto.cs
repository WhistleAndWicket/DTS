namespace DTOs.Inputs.Queries
{
    /// <summary>
    /// Represents the data transfer object for getting the paginated task items.
    /// </summary>
    public class GetPaginatedTaskItemsRequestDto
    {
        /// <summary>
        /// The current page number (starting from 1).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = 5;
    }
}
