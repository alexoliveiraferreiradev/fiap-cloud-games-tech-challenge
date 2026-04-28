using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Usuario : AgreggateRoot
    {
        protected Usuario()
        {

        }
        public string NomeUsuario { get; private set; }

        public string Email { get; private set; }

        public string Senha { get; private set; }

        public TipoUsuario Perfil { get;private set; }
        public bool Ativo {  get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }

        private string confirmacaoSenha = string.Empty;

        public Usuario(string nomeUsuario, string emailUsuario, string senhaUsuario,
            string confirmacaoSenhaUsuario)
        {
            NomeUsuario = nomeUsuario;
            Email = emailUsuario;
            Senha = senhaUsuario;
            confirmacaoSenha = confirmacaoSenhaUsuario;
            Perfil = TipoUsuario.Jogador;
            Ativo = true;
            DataCadastro = DateTime.UtcNow;
            DataAlteracao = DataCadastro;
            ValidarEntidade();
        }

        public Usuario(string emailUsuario, string senhaUsuario)
        {
            Email = emailUsuario;   
            Senha = senhaUsuario;
            ValidarDadosEntradas();
        }
               
        public void ValidarDadosEntradas()
        {
            AssertionConcern.AssertArgumentNotEmpty(Email, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentNotEmpty(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, MensagensDominio.UsuarioSenhaFraca);
            AssertionConcern.AssertArgumentEmailFormat(Email, MensagensDominio.UsuarioEmailInvalido);
            Ativo = true;
        }
        
        public override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentNotEmpty(NomeUsuario, MensagensDominio.UsuarioNomeObrigatorio);
            AssertionConcern.AssertArgumentNotEmpty(Email, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentNotEmpty(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentNotEmpty(confirmacaoSenha, MensagensDominio.UsuarioConfirmacaoSenhaObrigatoria);
            AssertionConcern.AssertArgumentLength(NomeUsuario, 3, 20, MensagensDominio.UsuarioTamanhoNomeInvalido);
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, MensagensDominio.UsuarioSenhaFraca);            
            AssertionConcern.AssertArgumentEquals(Senha, confirmacaoSenha, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
            AssertionConcern.AssertArgumentEmailFormat(Email, MensagensDominio.UsuarioEmailInvalido); 
        }

        public void Deletar(string emailUsuario,string senhaUsuario)
        {
            if(!Ativo) throw new DomainException(MensagensDominio.UsuarioJaExcluido);            
            if(string.IsNullOrEmpty(emailUsuario)) throw new DomainException(MensagensDominio.UsuarioEmailObrigatorio);
            if (Email != emailUsuario) throw new DomainException(MensagensDominio.UsuarioEmailInvalido);
            if(string.IsNullOrEmpty(senhaUsuario)) throw new DomainException(MensagensDominio.UsuarioSenhaObrigatoria);
           if(Senha != senhaUsuario) throw new DomainException(MensagensDominio.UsuarioSenhaFraca);
            Ativo = false;
            DataAlteracao = DateTime.UtcNow;
        }

        public void AtualizarEmail(string antigoEmail, string novoEmail)
        {
            AssertionConcern.AssertStateTrue(Ativo, MensagensDominio.UsuarioInativo);
            AssertionConcern.AssertArgumentNotEmpty(antigoEmail, MensagensDominio.UsuarioEmailAntigoObrigatorio);
            AssertionConcern.AssertArgumentNotEmpty(novoEmail, MensagensDominio.UsuarioEmailNovoObrigatorio);
            AssertionConcern.AssertArgumentEmailFormat(antigoEmail, MensagensDominio.UsuarioEmailInvalido);
            AssertionConcern.AssertArgumentEmailFormat(novoEmail, MensagensDominio.UsuarioEmailNovoInvalido);
            AssertionConcern.AssertArgumentNotEquals(antigoEmail,novoEmail, MensagensDominio.UsuarioEmailAtualizaDiferente);
            Email = novoEmail;
        }

        public void AtualizarSenha(string senhaAntiga, string novaSenha,string confirmacaoSenhaNova)
        {
            AssertionConcern.AssertStateTrue(Ativo, MensagensDominio.UsuarioInativo);
            AssertionConcern.AssertArgumentNotEmpty(senhaAntiga, MensagensDominio.UsuarioSenhaAntigaObrigatoria);
            AssertionConcern.AssertArgumentNotEmpty(novaSenha, MensagensDominio.UsuarioSenhaNovaObrigatoria);            
            AssertionConcern.AssertArgumentPasswordStrenght(senhaAntiga, MensagensDominio.UsuarioSenhaFraca);
            AssertionConcern.AssertArgumentPasswordStrenght(novaSenha, MensagensDominio.UsuarioSenhaNovaFraca);
            AssertionConcern.AssertArgumentEquals(novaSenha, confirmacaoSenhaNova, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
            Senha = novaSenha;
        }

        public void RebaixarPerfil()
        {
            if(Perfil!= TipoUsuario.Administrador)
                throw new DomainException(MensagensDominio.UsuarioPerfilRebaixarInvalido);

            Perfil = TipoUsuario.Jogador;
        }

        public void PromoverPerfil(Usuario usuario)
        {
            AssertionConcern.AssertArgumentNotNull(usuario, MensagensDominio.UsuarioObrigatorio);
            typeof(Usuario).GetProperty("Perfil").SetValue(usuario, TipoUsuario.Administrador);
        }
    }
}
