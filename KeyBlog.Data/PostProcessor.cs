using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Utils;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text.RegularExpressions;
using System.Web;

namespace KeyBlog.Data;

public class PostProcessor
{
    private readonly LocalPost _localPost;
    private readonly string _importPath;
    private readonly string _assetsPath;

    public PostProcessor(string importPath, string assetsPath, LocalPost localPost)
    {
        _localPost = localPost;
        _assetsPath = assetsPath;
        _importPath = importPath;
    }

    /// <summary>
    /// Markdown内容解析，复制图片 & 替换图片链接
    /// </summary>
    /// <returns></returns>
    public string MarkdownParse()
    {
        if (_localPost.Content == null)
        {
            return string.Empty;
        }

        var document = Markdig.Markdown.Parse(_localPost.Content);

        foreach (var node in document.AsEnumerable())
        {
            if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;
            foreach (var inline in paragraphBlock.Inline)
            {
                if (inline is not LinkInline { IsImage: true } linkInline) continue;

                var imgUrl = HttpUtility.UrlDecode(linkInline.Url);
                if (imgUrl == null) continue;
                if (imgUrl.StartsWith("http")) continue;

                // 路径处理
                var imgPath = Path.Combine(_importPath, _localPost.Path ?? "", imgUrl);
                var imgFilename = Path.GetFileName(imgUrl);
                var destDir = Path.Combine(_assetsPath, _localPost.Id);
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                var destPath = Path.Combine(destDir, imgFilename);
                if (File.Exists(destPath))
                {
                    // 图片重名处理
                    var imgId = GuidUtils.GetGuid();
                    imgFilename =
                        $"{Path.GetFileNameWithoutExtension(imgFilename)}-{imgId}.{Path.GetExtension(imgFilename)}";
                    destPath = Path.Combine(destDir, imgFilename);
                }

                // 替换图片链接
                linkInline.Url = imgFilename;
                // 复制图片
                //File.Copy(imgPath, destPath);

                Console.WriteLine($"复制 {imgPath} 到 {destPath}");
            }
        }


        using var writer = new StringWriter();
        var render = new NormalizeRenderer(writer);
        render.Render(document);
        return writer.ToString();
    }

    /// <summary>
    /// 填充文章状态和标题
    /// </summary>
    /// <returns></returns>
    public (string, string) InflateStatusTitle()
    {
        const string pattern = @"^（(.+)）(.+)$";
        var status = _localPost.Status ?? "已发布";
        var title = _localPost.Title;
        if (string.IsNullOrEmpty(title)) return (status, $"未命名文章{_localPost.CreationTime.ToLongDateString()}");
        var result = Regex.Match(title, pattern);
        if (!result.Success) return (status, title);

        status = result.Groups[1].Value;
        title = result.Groups[2].Value;

        _localPost.Status = status;
        _localPost.Title = title;

        if (!new[] { "已发表", "已发布" }.Contains(_localPost.Status))
        {
            _localPost.IsPublish = false;
        }

        return (status, title);
    }
}