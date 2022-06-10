using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using CsvHelper;
using System.Globalization;
using parsing_C_Sharp;

var config = Configuration.Default.WithDefaultLoader();
using var context = BrowsingContext.New(config);

var homeUrl = "https://www.toy.ru";
var catalogUrl = "https://www.toy.ru/catalog/boy_transport/";

using var doc = await context.OpenAsync(catalogUrl);

var pagesLink = doc.QuerySelectorAll("a.page-link");
var count = 0; 
foreach (var pageLink in pagesLink)
{
    using var page = await context.OpenAsync(homeUrl + pageLink.Attributes["href"].Value);
    var docs = new List<Task<IDocument>>();
    var links = page.QuerySelectorAll("a.product-name");
    
    count += links.Count();

    var rows = new List<Card>();

    using var fs = new StreamWriter("data.csv", true);
    using var writer = new CsvWriter(fs, CultureInfo.CurrentCulture);

    foreach (var link in links) 
    {
        var url = $"{homeUrl}{link.Attributes["href"].Value}";
        docs.Add(context.OpenAsync(url));
    }

    Task.WaitAll(docs.ToArray());

    foreach (var t in docs)
    {
        var res = t.Result;
        var oldPrice = "";
        var region = res.QuerySelector("div.select-city-link").QuerySelector("a").Text().Trim();
        //var crumbs = res.QuerySelectorAll("a.breadcrumb-item");
        var name = res.QuerySelector("h1.detail-name").Text().Trim();
        if (res.QuerySelector("span.old-price") != null) 
        {
            oldPrice = res.QuerySelector("span.old-price").Text().Trim();
        }
        
        var price = res.QuerySelector("span.price").Text().Trim();
        var stock = res.QuerySelector("span.ok").Text().Trim();
        var imgUrl = res.QuerySelector("img.img-fluid").Attributes["src"].Value;

        //var fields = (from c in crumbs select c.Text().Trim());
        var row = new Card(region, name, oldPrice, price, stock, imgUrl
        );

        rows.Add(row);

        res.Dispose();
    }

    writer.WriteRecords(rows);

}

Console.WriteLine(count);


