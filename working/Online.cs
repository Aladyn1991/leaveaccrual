using Microsoft.Playwright;
using termo;
//ignore some compiler warnings
#pragma warning disable CS8981
public class Online
{
    public IBrowser inner;
    public List<IPage> pages => inner.Contexts.First().Pages.ToList();
    public IPage page => pages.Last();

    public Online(IBrowser inner)
    {
        this.inner = inner;

    }

    public Online open(string url)
    {
        page.GotoAsync(url).Wait();
        return this;
    }

    public Online visit(string url)
    {
        if (!url.StartsWith("http"))
            url = "https://" + url;
            show.action("VISITING", url);
        execute_sync(page, async (b) => await b.GotoAsync(url));
        execute_sync(page, async (b) => await b.WaitForLoadStateAsync(LoadState.NetworkIdle));
        return this;
    }

    public Online click(string selector)
    {
        show.action("CLICKING", selector);
        execute_sync(page, async (b) => await b.ClickAsync(selector));
        return this;
    }

    public Online type(string selector, string text)
    {
        show.action("TYPING", text, "IN", selector);
        execute_sync(page, (b) => b.FillAsync(selector, text));
        return this;
    }

    public void close()
    {
        show.action("CLOSING");
        inner.CloseAsync().Wait();
    }

    public Online wait_for(string selector, int timeout=5)
    {
        show.action("WAITING FOR", selector);
        execute_sync(page, (b) => b.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = timeout *60*1000}));
        return this;
    }

    public Online wait_until_loaded()
    {
        show.action("WAITING ","FOR PAGE"," TO LOAD");
        execute_sync(page, (b) => b.WaitForLoadStateAsync(LoadState.NetworkIdle));
        return this;
    }

    public ILocator locate(string selector,string? has_text= null ,  string? has_not_text = null , ILocator? has = null , ILocator? has_not = null )
    {
        show.action("LOCATING", selector);
        return  page.Locator(selector, options: new PageLocatorOptions { HasText = has_text ,Has = has, HasNot = has_not, HasNotText = has_not_text });

    }




    public Online wait_for_navigation(string url)
    {   
        show.action("WAITING FOR NAVIGATION TO", url);
        execute_super(page, (b) => b.GotoAsync(url), (b) => b.WaitForLoadStateAsync(LoadState.NetworkIdle));
        return this;
    }



    public Online new_tab()
    {
        show.action("OPENING NEW TAB");
        execute_sync(inner, (b) => b.NewPageAsync());
        return this;
    }



    public Online switch_to_tab(int tabIndex)
    {
        show.action("SWITCHING TO TAB", tabIndex.ToString());
        if (tabIndex < 0 || tabIndex >= pages.Count)
            throw new IndexOutOfRangeException("Invalid tab index.");
        return this;
    }

    public Online back()
    {
        show.action("GOING BACK");
        page.GoBackAsync().Wait();
        return this;
    }

    public Online forward()
    {
        show.action("GOING FORWARD");
        page.GoForwardAsync().Wait();
        return this;
    }

    public Online refresh()
    {
        show.action("REFRESHING PAGE");
        page.ReloadAsync().Wait();
        return this;
    }

    public Online execute_js(string script)
    {
        show.action("EXECUTING JS", script);
        page.EvaluateAsync(script).Wait();
        return this;
    }

    public Online screenshot(string path)
    {
        show.action("TAKING SCREENSHOT", path);
        page.ScreenshotAsync(new PageScreenshotOptions { Path = path }).Wait();
        return this;
    }

    public async Task<T> execute<T>(T target, Func<T, Task> action)
    {
        try
        {
            
            await action(target);
            return target;
        }
        catch (Exception e)
        {
            show.error(e.Message);
            return target;
        }
    }

    public T execute_sync<T>(T target, Func<T, Task> action)
    {
        try
        {
            action(target).Wait();
            return target;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return target;
        }
    }

    public T execute_super<T>(T target, params Func<T, Task>[] actions)
    {
        try
        {
            foreach (var action in actions)
                action(target).Wait();
            return target;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return target;
        }
    }

    internal Online right_click(string table_selector2)
    {
        execute_sync(page, async (b) => await b.ClickAsync(table_selector2, new PageClickOptions { Button = MouseButton.Right }));
        return this;
    }
}
