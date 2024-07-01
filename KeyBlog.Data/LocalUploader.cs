using FreeSql;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using KeyBlog.Data.Utils;

namespace KeyBlog.Data;
public class LocalUploader
{
    private readonly string _importDir;
    private readonly string _assetPath = Path.GetFullPath("../../../../keyblog.client/src/assets");
    private List<string> exclusionDirs = new List<string> { ".git", "logseq", "pages" };
    private readonly IBaseRepository<Post> _postRepo;
    private readonly IBaseRepository<Category> _categoryRepo;
    private readonly CategoryService _categoryService;

    public LocalUploader(string importDir, IBaseRepository<Post> postRepo, IBaseRepository<Category> categoryRepo)
    {
        _importDir = importDir;
        _postRepo = postRepo;
        _categoryRepo = categoryRepo;
        _categoryService = new CategoryService(categoryRepo);
    }


    public void Upload()
    {
        // 数据导入
        WalkDirectoryTree(new DirectoryInfo(_importDir));

    }


    void WalkDirectoryTree(DirectoryInfo root)
    {
        // 参考资料：https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree

        Console.WriteLine($"正在扫描文件夹：{root.FullName}");

        FileInfo[]? files = null;
        DirectoryInfo[]? subDirs = null;

        try
        {
            files = root.GetFiles("*.md");
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }

        if (files != null)
        {
            foreach (var fi in files)
            {
                Console.WriteLine(fi.FullName);
                var postPath = fi.DirectoryName!.Replace(_importDir, "");

                var categoryNames = postPath.Split(Path.DirectorySeparatorChar);
                Console.WriteLine($"categoryNames: {string.Join(",", categoryNames)}");
                var categories = new List<Category>();
                if (categoryNames.Length > 0)
                {
                    var rootCategory = _categoryRepo.Where(a => a.Name == categoryNames[0]).First();
                    if (rootCategory == null)
                    {
                        _categoryRepo.Insert(new Category { Name = categoryNames[0] });
                        rootCategory = _categoryRepo.Where(a => a.Name == categoryNames[0]).First();
                        rootCategory.Categories = _categoryService.GetCategoryHierarchy(rootCategory.Id);
                        _categoryRepo.Update(rootCategory);
                    }
                    categories.Add(rootCategory);
                    Console.WriteLine($"+ 添加分类: {rootCategory.Id}.{rootCategory.Name}");
                    for (var i = 1; i < categoryNames.Length; i++)
                    {
                        var name = categoryNames[i];
                        var parent = categories[i - 1];
                        var category = _categoryRepo.Where(a => a.ParentId == parent.Id && a.Name == name).First();
                        if (category == null)
                        {
                            _categoryRepo.Insert(new Category
                            {
                                Name = name,
                                ParentId = parent.Id
                            });
                            category = _categoryRepo.Where(a => a.ParentId == parent.Id && a.Name == name).First();
                            category.Categories = _categoryService.GetCategoryHierarchy(category.Id);
                            _categoryRepo.Update(category);
                        }
                        categories.Add(category);
                        Console.WriteLine($"+ 添加子分类：{category.Id}.{category.Name}");
                    }
                }

                var reader = fi.OpenText();
                var content = reader.ReadToEnd();
                reader.Close();

                // 保存文章
                var localPost = new LocalPost
                {
                    Id = GuidUtils.GetGuid(),
                    Status = "已发布",
                    Title = fi.Name.Replace(".md", ""),
                    IsPublish = true,
                    Content = content,
                    Summary = null,
                    Path = postPath,
                    CreationTime = fi.CreationTime,
                    LastUpdateTime = fi.LastWriteTime,
                    CategoryId = categories[^1].Id
                };


                var processor = new PostProcessor(_importDir, _assetPath, localPost);

                // 处理文章标题和状态
                processor.InflateStatusTitle();

                // 处理文章正文内容
                // 导入文章的时候一并导入文章里的图片，并对图片相对路径做替换操作
                localPost.Content = processor.MarkdownParse();

                _postRepo.Insert(new Post
                {
                    Id = localPost.Id,
                    Title = localPost.Title,
                    IsPublish = localPost.IsPublish,
                    Content = localPost.Content,
                    Summary = localPost.Summary,
                    CreationTime = localPost.CreationTime,
                    LastUpdateTime = localPost.LastUpdateTime,
                    CategoryId = localPost.CategoryId
                });
            }
        }

        // Now find all the subdirectories under this directory.
        subDirs = root.GetDirectories();

        if (subDirs != null)
        {
            foreach (var dirInfo in subDirs)
            {
                if (exclusionDirs.Contains(dirInfo.Name))
                {
                    continue;
                }

                if (dirInfo.Name.EndsWith(".assets"))
                {
                    continue;
                }

                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo);
            }
        }


    }

}