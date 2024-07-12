using System.Globalization;
using HtmlAgilityPack;
using LogKeeperPg.Models;

namespace LogKeeperPg.PreviousLogsToDb;

public class WebScraper
{
    const string DateTimeFormat = "MM/dd/yyyy HH:mm:ss";
    public static async Task<LogInformation> ScrapeLogInformation(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var uuid = Guid.TryParse(url.Split("id=")[1], out var guid) ? guid : Guid.NewGuid();
        
        var titleNode = doc.DocumentNode.SelectSingleNode("//h1");
        var title = HtmlEntity.DeEntitize(titleNode?.InnerText.Trim() ?? "N/A");
        // title = title.Replace("&#xA;", " ");
        
        var authorNode = doc.DocumentNode.SelectSingleNode("//p[contains(text(), 'Author:')]");
        var author = HtmlEntity.DeEntitize(authorNode?.InnerText.Split(": ")[1] ?? "N/A");
        
        var projectNode = doc.DocumentNode.SelectSingleNode("//p[contains(text(), 'Project:')]");
        var project = HtmlEntity.DeEntitize(projectNode?.InnerText.Split(": ")[1] ?? "N/A");
        
        var timeNode = doc.DocumentNode.SelectSingleNode("//p[contains(text(), 'Time:')]");
        var dateTime = DateTime.ParseExact(timeNode?.InnerText.Split(": ")[1] ?? string.Empty, DateTimeFormat, CultureInfo.InvariantCulture);
        switch (dateTime.Kind)
        {
            case DateTimeKind.Local:
                dateTime = dateTime.ToUniversalTime();
                break;
            case DateTimeKind.Unspecified:
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                break;
        }
        
        var contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='content']");
        var contents = HtmlEntity.DeEntitize(contentNode?.InnerText.Trim() ?? "N/A");

        return new LogInformation(uuid, title, author, project, dateTime, contents);
    }
}