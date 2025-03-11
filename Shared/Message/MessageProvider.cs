using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APIBlog.Shared;

public static class MessageProvider
{
    public static readonly Dictionary<Message, string> Messages = new()
    {
        { Message.user_created, "El usuario ha sido creado con éxito." },
        { Message.user_updated, "Los datos del usuario se han actualizado correctamente." },
        { Message.user_deleted, "El usuario ha sido eliminado exitosamente." },
        { Message.user_not_exist, "El usuario solicitado no existe." },

        { Message.user_not_owner_blog, "No tienes permisos para administrar este blog." },
        { Message.user_not_owner_post, "No tienes permisos para modificar esta publicación." },
        { Message.user_not_owner_comment, "No tienes permisos para modificar este comentario." },
        { Message.username_in_use, "El nombre de usuario ya está en uso. Intenta con otro." },

        { Message.blog_not_exist, "El blog solicitado no existe." },
        { Message.blog_updated, "El blog se ha actualizado correctamente."},
        { Message.blog_deleted, "El blog ha sido eliminado correctamente." },
        { Message.blogname_in_use, "El nombre del blog ya está en uso. Elige otro." },

        { Message.post_not_exist, "La publicación solicitada no existe." },
        { Message.post_updated, "La publicación se ha actualizado correctamente"},
        { Message.post_deleted, "La publicación ha sido eliminada exitosamente." },

        { Message.commet_not_exist, "El comentario solicitado no existe." },
        { Message.comment_updated, "El comentario se ha actualizado correctamente." },
        { Message.comment_deleted, "El comentario se ha eliminado exitosamente." },

        { Message.incorrect_login, "El nombre de usuario o la contraseña son incorrectos." },

        { Message.password_updated, "La contraseña se ha actualizado correctamente." },
        { Message.passwords_not_match, "Las contraseñas no coinciden. Inténtalo nuevamente." },

        { Message.unauthorize_access, "No tienes permisos para realizar esta acción." },
        { Message.unexpected_error, "Ha ocurrido un error inesperado. Inténtalo nuevamente más tarde." }
    };

    public static string Get(Message message)
    {
        return Messages.ContainsKey(message) ? Messages[message] : "Error desconocido";
    }
}