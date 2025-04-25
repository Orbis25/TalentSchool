using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TalentSchool.Helpers;

public static class MarkdownHelper
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseSoftlineBreakAsHardlineBreak()
        .Build();

    public static HtmlString RenderMarkdown(this IHtmlHelper html, string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return new HtmlString(string.Empty);

        var result = Markdown.ToHtml(markdown, Pipeline);
        return new HtmlString(result);
    }
}