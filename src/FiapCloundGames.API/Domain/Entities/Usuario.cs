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

        public TipoUsuario Perfil { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public MotivoExclusao? MotivoDesativacao { get; private set; }

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
            AssertionConcern.AssertArgumentEmpty(Email, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentEmpty(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, MensagensDominio.UsuarioSenhaFraca);
            AssertionConcern.AssertArgumentEmailFormat(Email, MensagensDominio.UsuarioEmailInvalido);
            Ativo = true;
        }

        public override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentEmpty(NomeUsuario, MensagensDominio.UsuarioNomeObrigatorio);
            AssertionConcern.AssertArgumentEmpty(Email, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentEmpty(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentEmpty(confirmacaoSenha, MensagensDominio.UsuarioConfirmacaoSenhaObrigatoria);
            AssertionConcern.AssertArgumentLength(NomeUsuario, 3, 20, MensagensDominio.UsuarioTamanhoNomeInvalido);
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, MensagensDominio.UsuarioSenhaFraca);
            AssertionConcern.AssertArgumentEquals(Senha, confirmacaoSenha, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
            AssertionConcern.AssertArgumentEmailFormat(Email, MensagensDominio.UsuarioEmailInvalido);
        }

        public void Desativar(MotivoExclusao motivo)
        {
            if (!Ativo) throw new DomainException(MensagensDominio.UsuarioJaDesativado);
            
            Ativo = false;
            DataAlteracao = DateTime.UtcNow;
            MotivoDesativacao = motivo;
        }

        public void Atualizar(string novoNome, string novoEmail, string novaSenha, string novaReSenha)
        {
            AssertionConcern.AssertStateFalse(Ativo, MensagensDominio.UsuarioInativo);

            AtualizarNomeUsuario(novoNome);
            AtualizarEmail(novoEmail);
            AtualizarSenha(novaSenha, novaReSenha);
        }

        public void AtualizarNomeUsuario(string nomeNovo)
        {
            AssertionConcern.AssertArgumentEmpty(nomeNovo, MensagensDominio.UsuarioNomeNovoObrigatorio);
            if (NomeUsuario == nomeNovo) return;
            AssertionConcern.AssertArgumentLength(nomeNovo, 3, 20, MensagensDominio.UsuarioTamanhoNomeInvalido);
            NomeUsuario = nomeNovo;
        }


        public void AtualizarEmail(string novoEmail)
        {
            AssertionConcern.AssertArgumentEmpty(novoEmail, MensagensDominio.UsuarioEmailNovoObrigatorio);
            if (Email == novoEmail) return;
            AssertionConcern.AssertArgumentEmailFormat(novoEmail, MensagensDominio.UsuarioEmailNovoInvalido);
            Email = novoEmail;
        }

        public void AtualizarSenha(string novaSenha, string confirmacaoSenhaNova)
        {
            AssertionConcern.AssertArgumentEmpty(novaSenha, MensagensDominio.UsuarioSenhaNovaObrigatoria);
            if (Senha == novaSenha) return;
            AssertionConcern.AssertArgumentPasswordStrenght(novaSenha, MensagensDominio.UsuarioSenhaNovaFraca);
            AssertionConcern.AssertArgumentEquals(novaSenha, confirmacaoSenhaNova, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
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
            AssertionConcern.AssertStateTrue(!Ativo, MensagensDominio.UsuarioJaAtivo);
            Ativo = true;
            DataAlteracao = DateTime.UtcNow;
            MotivoDesativacao = null;   
        }
    }
}
