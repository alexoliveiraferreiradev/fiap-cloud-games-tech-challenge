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
        public static string UsuarioEmailInvalido = "O email do usuário é inválido.";
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
        public static string PermissaoNegadaCriarAdministrador = "Permissão negada para criar um administrador.";
        public static string UsuarioNaoEncontrado = "O usuário não foi encontrado.";
        public static string UsuarioPerfilRebaixarInvalido = "O perfil do usuário não pode ser rebaixado, pois ele não é um administrador.";
        public static string CrendenciasInvalidas = "Credenciais inválidas.";
        #endregion

        #region Jogos
        public static string JogoNomeObrigatorio = "O nome do jogo é obrigatório.";
        public static string JogoTamanhoNomeInvalido = "O nome do jogo deve conter entre 3 e 20 caracteres.";
        public static string JogoDescricaoTamanhoInvalido = "A descrição do jogo deve conter entre 5 e 100 caracteres.";
        public static string JogoDescricaoObrigatoria = "A descrição do jogo é obrigatória.";
        public static string JogoGeneroObrigatorio = "O gênero do jogo é inválido.";
        public static string JogoNaoEncontrado = "O jogo não foi encontrado.";
        public static string JogoInvalido = "O jogo já está desativado.";
        public static string JogoAtivo = "O jogo já está ativo.";
        public static string JogoMesmoNomeExistente = "Já existe um jogo com esse nome.";
        #endregion

        #region Pedido
        public static string PedidoSemUsuario = "O pedido deve estar associado a um usuário.";
        public static string PedidoSemJogos = "O pedido deve conter pelo menos um jogo para ser finalizado.";
        public static string PedidoNaoRascunhos = "Apenas pedidos em rascunho podem ser finalizados.";
        public static string PedidoJogoNaoRascunhos = "Não é possível adicionar itens a um pedido que não esteja em rascunho.";
        public static string PedidoJogoJaAdicionado = "Este jogo já foi adicionado ao pedido.";
        #endregion

        #region Promoção
        public static string PromocaoDataFimInvalida = "A data de término deve ser no futuro.";
        public static string PromocaoNaoEncontrada = "Promoção não encontrada para este jogo.";
        public static string PromocaoValorMaior = "O valor promocional deve ser menor que o preço base.";
        public static string PromocaoInativa = "A promoção já está desativada.";
        #endregion

        public static string ValorInvalido = "O valor deve ser positivo.";
        public static string PrecoObrigatorio = "O preço é obrigatório.";
    }
}
