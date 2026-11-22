using DigitizedDallet.Controllers;
using DigitizedDallet.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DigitizedDallet.Helpers;

public static class MyHtmlHelperLinkExtensions
{
    public static IHtmlContent ActionLink(this IHtmlHelper helper, RootModel root)
    {
        return helper.ActionLink(root.Name,
            nameof(ArticleController.Index),
            nameof(ArticleController).Remove(nameof(ArticleController).Length - "Controller".Length),
            protocol: null,
            hostname: null,
            fragment: root.Name,
            routeValues: new { id = root.Letter.Name },
            htmlAttributes: null);
    }
    

    public static IHtmlContent ActionLink(this IHtmlHelper helper,
        ArticleModel article,
        string? prefix = null,        
        bool withMark = true,
        bool withDirectionalParticle = true,
        bool withNegative = false,
        bool withDallet = true,
        bool withDalletEdit = true,
        bool useGuid = false)
    {
        var requestedScript = helper.ViewBag.RequestedScript as string; 

        var content = new HtmlContentBuilder().AppendHtml(helper.ActionLink(prefix?.GetTransliteratedString(requestedScript) + article.Resolved.Name.GetTransliteratedString(requestedScript),
                nameof(ArticleController.Details),
                nameof(ArticleController).Remove(nameof(ArticleController).Length - "Controller".Length),
                new { name = article.Resolved.Name, guid = useGuid ? article.Resolved.Id : null }));

        if (withDirectionalParticle && (article.Resolved.HasDirectionalParticle ?? false))
        {
            content.Append("-d");
        }

        if (withMark && article.Resolved.Mark != null)
        {
            content.AppendHtml("""<font color ="red">""")
                .Append(article.Resolved.Mark)
                .AppendHtml("</font>");
        }       

        if (withDallet)
        {   
            content.AppendHtml(article.GetHtmlDalletNotation(prefix: prefix, withDalletEdit: withDalletEdit, withDirectionalParticle : withDirectionalParticle));           
        }

        return content;
    }


    public static IHtmlContent GetHtmlDalletNotation(this ArticleModel article, string? prefix = null, bool withDalletEdit = true, bool withDirectionalParticle = true)
    {
#if !DEBUG
        withDalletEdit = false;
#endif
        var content = new HtmlContentBuilder();

        if (article.StandardizedFormOf is not null)
        {
            return content;
        }

        if (withDalletEdit)
        {
            content.Append(" ");
            if (article.Resolved.DalletNames.Any())
            {   
                content.AppendHtml("""<span style="font-size: 2.5em; white-space: nowrap;">""");
                content.Append("[");
                for (int i = 0; i < article.Resolved.DalletNames.Count; i++)
                {
                    var dalletName = article.Resolved.DalletNames[i];

                    content.AppendHtml($"""<span id="{article.Resolved.Id}+{i}" style="white-space: nowrap">""");
                    content.AppendHtml($"""<span style="display:none; white-space: nowrap;">{dalletName}</span>""");
                    foreach (var c in dalletName)
                    {
                        content.AppendHtml($"""<input type="button" value="{c}" style="color: black;padding: 0;border: none;background: none; white-space: nowrap;" onclick="toggleLetter(this)" />""");
                    }
                    content.AppendHtml("""</span>""");

                    if (i < article.Resolved.DalletNames.Count - 1)
                    {
                        content.Append(" / ");
                    }
                }
                content.Append("]");
                content.AppendHtml("""</span>""");                
            }
            else
            {
                content.AppendHtml($"""<input type="button" value="+" onclick="addDallet('{article.Resolved.Id}')" />""");
            }
        }
        else if (article.Resolved.DalletNames.Any())
        {
            var suffixedDirectionalParticle = withDirectionalParticle && (article.HasDirectionalParticle ?? false) ? "-ḋ" : string.Empty;            

            content.Append(" ");
            content.AppendHtml("<small>")
                    .Append($"[{string.Join(" / ", article.Resolved.DalletNames.ToDalletNotation().Select(x => prefix + x + suffixedDirectionalParticle))}]")
                    .AppendHtml("</small>");
        }       

        return content;
    }
}