using APIBlog.Models;

namespace APIBlog.Services;

public interface ISecurityService
{
    string HashingSHA256(string text);
    string GetJwt(User user);
}