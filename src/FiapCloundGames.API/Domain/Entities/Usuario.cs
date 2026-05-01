using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Usuario : AgreggateRoot
    {
        protected Usuario()
        {

        }
        public Nome NomeUsuario { get; private set; }

        public Email EmailUsuario { get; private set; }

        public Senha Senha { get; private set; }

        public TipoUsuario Perfil { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public MotivoExclusao? MotivoDesativacao { get; private set; }

        

        public Usuario(Nome nomeUsuario, Email emailUsuario, Senha senhaUsuario)
        {
            NomeUsuario = nomeUsuario;
            EmailUsuario = emailUsuario;
            Senha = senhaUsuario;
            Perfil = TipoUsuario.Jogador;
            Ativo = true;
            DataCadastro = DateTime.UtcNow;
            DataAlteracao = DataCadastro;
            ValidarEntidade();
        }

        public Usuario(Email emailUsuario, Senha senhaUsuario)
        {
            EmailUsuario = emailUsuario;
            Senha = senhaUsuario;
            ValidarDadosEntradas();
        }

        public void ValidarDadosEntradas()
        {
            AssertionConcern.AssertArgumentNotNull(EmailUsuario, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentNotNull(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            Ativo = true;
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentNotNull(NomeUsuario, MensagensDominio.UsuarioNomeObrigatorio);
            AssertionConcern.AssertArgumentNotNull(EmailUsuario, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentNotNull(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
        }

        public void Desativar(MotivoExclusao motivo)
        {
            if (!Ativo) throw new DomainException(MensagensDominio.UsuarioJaDesativado);

            Ativo = false;
            DataAlteracao = DateTime.UtcNow;
            MotivoDesativacao = motivo;
        }

        public void Atualizar(Nome novoNome, Email novoEmail, Senha novaSenha)
        {
            AssertionConcern.AssertStateFalse(Ativo, MensagensDominio.UsuarioInativo);

            AtualizarNomeUsuario(novoNome);
            AtualizarEmail(novoEmail);
            AlterarSenha(novaSenha);
        }

        public void AtualizarNomeUsuario(Nome nomeNovo)
        {
            if (NomeUsuario == nomeNovo) return;            
            NomeUsuario = nomeNovo;
        }


        public void AtualizarEmail(Email novoEmail)
        {         
            if (EmailUsuario == novoEmail) return;            
            EmailUsuario = novoEmail;
        }

        public void AlterarSenha(Senha novaSenha)
        {
            if (Senha == novaSenha) return;
            Senha = novaSenha;
        }

        public void RebaixarPerfil()
        {
            if (Perfil != TipoUsuario.Administrador)
                throw new DomainException(MensagensDominio.UsuarioPerfilRebaixarInvalido);

            Perfil = TipoUsuario.Jogador;
        }

        public void PromoverPerfil(Usuario usuario)
        {
            AssertionConcern.AssertArgumentNotNull(usuario, MensagensDominio.UsuarioNaoEncontrado);
            typeof(Usuario).GetProperty("Perfil").SetValue(usuario, TipoUsuario.Administrador);
        }

        public void Reativar()
        {
            if (Ativo) throw new DomainException(MensagensDominio.UsuarioAtivo);
            Ativo = true;
            DataAlteracao = DateTime.UtcNow;
            MotivoDesativacao = null;
        }
    }
}
