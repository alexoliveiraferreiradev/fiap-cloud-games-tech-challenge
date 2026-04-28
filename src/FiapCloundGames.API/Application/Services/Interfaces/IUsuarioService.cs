using FiapCloundGames.API.Application.Dtos;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IUsuarioService  
    {
        Task<Usuario> CriaAdministrador(CriaUsuarioRequest request, bool hasPermision, string token);
        Task<Usuario> CriaJogador(CriaUsuarioRequest request);
    }
}