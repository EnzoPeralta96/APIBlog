using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using AutoMapper;

namespace APIBlog.Services;
public class CommentService : ICommentService
{
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    public CommentService(IUserAuthorizationService userAuthorizationService, ICommentRepository commentRepository, IMapper mapper)
    {
        _userAuthorizationService = userAuthorizationService;
        _commentRepository = commentRepository;
        _mapper = mapper;
    }
}