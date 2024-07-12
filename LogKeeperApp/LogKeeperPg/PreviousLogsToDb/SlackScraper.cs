using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LogKeeperPg.PreviousLogsToDb;

public static class SlackScraper
{
    private const string WorkspaceUrl = "https://app.slack.com/client/TGUEYGR6W/GSZ5JJCCD";
    
    private static IWebElement? _messagesContainer;

    private const int ScrollIterations = 600;
    
    public static async Task<List<string>> GetLinksFromChannelAsync()
    {
        using var driver = new ChromeDriver();
        await driver.Navigate().GoToUrlAsync(WorkspaceUrl);

        Console.WriteLine("Please log in to Slack and complete the 2FA. Press Enter to continue...");
        Console.ReadLine();

        var potentialContainers = driver.FindElements(
            By.CssSelector(".c-virtual_list__scroll_container, .c-scrollbar__hider, .c-scrollbar__child"));

        foreach (var container in potentialContainers)
        {
            if (container.FindElements(By.CssSelector(".c-message_kit__gutter")).Count > 0)
            {
                _messagesContainer = container;
                break;
            }
        }

        if (_messagesContainer == null)
        {
            Console.WriteLine("Messages container not found. Exiting...");
            return [];
        }
        
        var links = new List<string>();

        var js = (IJavaScriptExecutor) driver;
        for (var i = 0; i < ScrollIterations; i++)
        {
            js.ExecuteScript("arguments[0].scrollBy(0, -350)", _messagesContainer);
            await Task.Delay(250);
            var messages = driver.FindElements(By.CssSelector(".p-rich_text_section"));
            
            foreach (var message in messages)
            {
                var linkElements = message.FindElements(By.CssSelector("a"));
                foreach (var linkElement in linkElements)
                {
                    var href = linkElement.GetAttribute("href");
                    if (!links.Contains(href) &&
                        (href.StartsWith("http://log.") || href.StartsWith("https://log.")))
                    {
                        links.Add(href);
                        Console.WriteLine(href);
                    }
                }
            }
        }

        return links;
    }
}