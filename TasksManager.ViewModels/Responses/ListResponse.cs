using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModels.Responses
{
    public class ListResponse<TItem> where TItem : class
    {
        public ICollection<TItem> Items { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string Sorting { get; set; }

        public int TotalItemsCount { get; set; }

        public int TotalPagesCount
        {
            get
            {
                if (PageSize == 0) return TotalItemsCount;

                int pageCount = TotalItemsCount / PageSize;

                if (TotalItemsCount % PageSize != 0)
                    pageCount++;

                return pageCount;
            }
        }
    }
}
