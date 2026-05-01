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

        public string Senha { get; private set; }

        public TipoUsuario Perfil { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public MotivoExclusao? MotivoDesativacao { get; private set; }

        private string confirmacaoSenha = string.Empty;

        public Usuario(Nome nomeUsuario, Email emailUsuario, string senhaUsuario,
            string confirmacaoSenhaUsuario)
        {
            NomeUsuario = nomeUsuario;
            EmailUsuario = emailUsuario;
            Senha = senhaUsuario;
            confirmacaoSenha = confirmacaoSenhaUsuario;
            Perfil = TipoUsuario.Jogador;
            Ativo = true;
            DataCadastro = DateTime.UtcNow;
            DataAlteracao = DataCadastro;
            ValidarEntidade();
        }

        public Usuario(Email emailUsuario, string senhaUsuario)
        {
            EmailUsuario = emailUsuario;
            Senha = senhaUsuario;
            ValidarDadosEntradas();
        }

        public void ValidarDadosEntradas()
        {
            AssertionConcern.AssertArgumentEmpty(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, MensagensDominio.UsuarioSenhaFraca);
            Ativo = true;
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentEmpty(Senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentEmpty(confirmacaoSenha, MensagensDominio.UsuarioConfirmacaoSenhaObrigatoria);         
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, MensagensDominio.UsuarioSenhaFraca);
            AssertionConcern.AssertArgumentEquals(Senha, confirmacaoSenha, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
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
            var novoNomeVO = new Nome(nomeNovo);            
            if (NomeUsuario == novoNomeVO) return;            
            NomeUsuario = novoNomeVO;
        }


        public void AtualizarEmail(string novoEmail)
        {
            var novoEmailVO = new Email(novoEmail);
            if (EmailUsuario == novoEmailVO) return;            
            EmailUsuario = novoEmailVO;
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
            if (Ativo) throw new DomainException(MensagensDominio.UsuarioAtivo);
            Ativo = true;
            DataAlteracao = DateTime.UtcNow;
            MotivoDesativacao = null;
        }
    }
}
