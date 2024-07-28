using Hotel_Management_System.Models;

namespace Hotel_Management_System.Pagination
{
    public class DataPagination
    {
        public PaginatedResponse<T> GetPaginatedResponse<T>(List<T> items, int page, int pageSize)
        {
            var totalCount = items.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            if (page > totalPages)
            {
                return new PaginatedResponse<T>
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    Data = new List<T>()
                };
            }

            var paginatedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResponse<T>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Data = paginatedItems
            };
        }
    }
}
