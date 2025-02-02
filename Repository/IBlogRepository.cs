using APIBlog.Models;

namespace APIBlog.Repository;

public interface IBlogRepository
{
    public void Create(Blog blog);
    public void Update(int id, Blog blog);
    public void Delete(int id);
    public Blog GetBlog(int id);
    public Blog GetBlog(string name);
    public List<Blog> Blogs();
    public List<Post> PostsByBlog(int id);
    public List<Post> PostsByBlog(string name);

}