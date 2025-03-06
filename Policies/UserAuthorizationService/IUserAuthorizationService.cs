using APIBlog.Shared;
namespace APIBlog.Policies.Authorization;
public interface IUserAuthorizationService
{
    //Autorización de un admin o de un usuario sobre si mismo
    Task<Result> AuthorizeUserAsync(int userId);

    //Autorización de un admin sobre un blog o de un usuario sobre su propio blog
    Task<Result> AuthorizeUserBlogAsync(int userId, int blogId);

    //Autorización de un admin a crear un post o de un usuario a crear un post a nombre propio
    Task<Result> AuthorizeUserPostCreateAsync(int userId);

    //Autorización de un admin a modificar/eliminar un post o de un usuario a moficiar/eliminar su propio post
    Task<Result> AuthorizeUserPostRequestAsync(int userId, int postId);


    Task<Result> AuthorizeUserCommentAsync(int userId, int commentId);

}