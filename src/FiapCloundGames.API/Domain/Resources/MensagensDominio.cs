namespace FiapCloundGames.API.Domain.Resources
{
    public static class MensagensDominio
    {
        #region Usuario
        public static string UsuarioNomeObrigatorio = "O nome do usuário é obrigatório.";
        public static string UsuarioNomeAntigoObrigatorio = "O nome antigo do usuário é obrigatório.";
        public static string UsuarioNomeNovoObrigatorio = "O novo nome do usuário é obrigatório.";
        public static string UsuarioEmailObrigatorio = "O email do usuário é obrigatório.";
        public static string UsuarioSenhaObrigatoria = "A senha do usuário é obrigatória.";
        public static string UsuarioConfirmacaoSenhaObrigatoria = "A confirmação de senha do usuário é obrigatória.";
        public static string UsuarioTamanhoNomeInvalido = "O nome do usuário deve conter entre 3 e 20 caracteres.";
        public static string UsuarioSenhaFraca = "A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.";
        public static string UsuarioSenhaConfirmacaoDiferente = "A senha e a confirmação de senha devem ser iguais.";
        public static string UsuarioInativo = "O usuário deve estar ativo";
        public static string UsuarioAtivo = "O usuário deve estar inativo";
        public static string UsuarioJaDesativado = "O usuário já foi desativado.";
        public static string UsuarioEmailAntigoObrigatorio = "O email antigo do usuário é obrigatório.";
        public static string UsuarioEmailAntigoInvalido = "O email antigo do usuário é inválido.";
        public static string UsuarioEmailNovoObrigatorio = "O novo email do usuário é obrigatório.";
        public static string UsuarioEmailNovoInvalido = "O novo email do usuário é inválido.";
        public static string UsuarioSenhaAntigaObrigatoria = "A senha antiga do usuário é obrigatória.";
        public static string UsuarioSenhaAntigaFraca = "A senha antiga do usuário é fraca.";
        public static string UsuarioSenhaNovaObrigatoria = "A senha nova do usuário é obrigatória.";
        public static string UsuarioSenhaNovaFraca = "A senha nova do usuário é fraca.";        
        public static string UsuarioNaoEncontrado = "O usuário não foi encontrado.";
        public static string AdminNaoEncontrado = "O administrador não foi encontrado.";
        public static string UsuarioPerfilRebaixarInvalido = "O perfil do usuário não pode ser rebaixado para jogador, pois ele já é um jogador.";
        public static string CrendenciasInvalidas = "Credenciais inválidas.";
        public static string EmailJaCadastrado = "O email já foi cadastrado.";
        public static string NomeUsuarioJaCadastrado = "O nome de usuário já foi cadastrado.";
        public static string OperacaoDesativarInvalida = "Operação inválida: você não pode deletar seu próprio perfil.";
        public static string OperacaoRebaixarInvalida = "Operação inválida: você não pode rebaixar seu próprio perfil.";
        public static string OperacaoDesativarAdminInvalida = "Não é possível desativar o último administrador.";
        #endregion

        #region Jogos
        public static string JogoNomeObrigatorio = "O nome do jogo é obrigatório.";
        public static string JogoTamanhoNomeInvalido = "O nome do jogo deve conter entre 3 e 20 caracteres.";
        public static string JogoDescricaoTamanhoInvalido = "A descrição do jogo deve conter entre 5 e 100 caracteres.";
        public static string JogoDescricaoObrigatoria = "A descrição do jogo é obrigatória.";
        public static string JogoGeneroObrigatorio = "O gênero do jogo é inválido.";
        public static string JogoNaoEncontrado = "O jogo não foi encontrado.";
        public static string JogoInvalido = "O jogo está desativado.";
        public static string JogoAtivo = "O jogo já está ativo.";
        public static string JogoMesmoNomeExistente = "Já existe um jogo com esse nome.";
        public static string JogoSemPromocoes = "Não há promoções para este jogo.";
        #endregion

        #region Pedido
        public static string PedidoSemUsuario = "O pedido deve estar associado a um usuário.";
        public static string PedidoSemJogos = "O pedido deve conter pelo menos um jogo para ser finalizado.";
        public static string PedidoNaoRascunhos = "Apenas pedidos em rascunho podem ser finalizados.";
        public static string PedidoJogoNaoRascunhos = "Não é possível adicionar itens a um pedido que não esteja em rascunho.";
        public static string PedidoJogoJaAdicionado = "Este jogo já foi adicionado ao pedido.";
        #endregion

        #region Promoção
        public static string PeriodoObrigatorio = "O período da promoção é obrigatório.";
        public static string PromocaoNaoEncontrada = "Promoção não encontrada.";
        public static string PromocaoValorMaior = "O valor promocional deve ser menor que o preço base.";
        public static string PromocaoInativa = "A promoção já está desativada.";
        #endregion

        #region Value Objects
        public static string ValorInvalido = "O valor deve ser positivo.";
        public static string PrecoObrigatorio = "O preço é obrigatório.";
        public static string EmailObrigatorio = "O email do usuário é obrigatório.";
        public static string EmailInvalido = "O email é inválido.";
        public static string DataFimInvalida = "A data final deve ser no futuro.";
        public static string SenhaTamanhoInvalido = "O tamanho da senha é inválido.";
        public static string EmailTamanhoInvalido = "O tamanho do email é inválido.";
        public static string NomeNaoReal = "Por favor, informe o seu nome real.";
        public static string EmailNaoReal = "Por favor, informe um email real.";
        public static string NomeJogoNaoReal = "Por favor, informe um nome de jogo real.";
        public static string DescricaoJogoNaoReal = "Por favor, informe um descrição de jogo real.";
        #endregion

        #region Biblioteca
        public static string BibliotecaInativa = "A biblioteca está inativa.";
        public static string BibliotecaJogoRepetido = "O usuário já possui o jogo na biblioteca.";
        #endregion
    }
}
