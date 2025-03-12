using Common.Models;

namespace APIBlog.Security;

public interface ISecurityService
{
    string HashingSHA256(string text);
    string GetJwt(User user);
}