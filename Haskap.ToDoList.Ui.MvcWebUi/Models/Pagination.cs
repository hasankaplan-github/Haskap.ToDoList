namespace Haskap.ToDoList.Ui.MvcWebUi.Models;

public class Pagination
{
    public int PageSize { get; }
    public int CurrentPageIndex { get; } // starts from 1
    public int ItemCount { get; }
    public int PageCount { get; }

    public bool HasNextPage 
    {
        get
        {
            return (PageCount > 1 && CurrentPageIndex < PageCount);
        }
    }

    public bool HasPreviousPage
    {
        get
        {
            return (PageCount > 1 && CurrentPageIndex > 1);
        }
    }

    public int GetFirstIndex(int visiblePageNumberCount)
    {

        return CurrentPageIndex - visiblePageNumberCount < 1 ? 1 : CurrentPageIndex - visiblePageNumberCount;
    }

    public int GetLastIndex(int visiblePageNumberCount)
    {
        return CurrentPageIndex + visiblePageNumberCount > PageCount ? PageCount : CurrentPageIndex + visiblePageNumberCount;
    }

    public Pagination(int pageSize, int currentPageIndex, int itemCount)
    {
        PageSize=pageSize < 1 ? 10 : pageSize;
        ItemCount=itemCount<0 ? 0 : itemCount;
        PageCount = (int)Math.Ceiling(ItemCount / (double)PageSize);
        CurrentPageIndex=currentPageIndex < 1 || currentPageIndex > PageCount ? 1 : currentPageIndex;
    }
}
