using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IUsuarioService  
    {
        Usuario CriaAdministrador(Usuario entity, bool hasPermision, string token);
        Usuario CriaJogador(Usuario entity);
    }
}