using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloundGames.UnitTests.Entities
{
    public class PedidoTests
    {
        private readonly UsuarioFixture _usuarioFixture;
        public PedidoTests()
        {
            _usuarioFixture = new UsuarioFixture();
        }

        /// <summary>
        /// Valida a criação de um pedido com um usuário válido. O teste verifica se o pedido é criado com sucesso, se o status inicial é "Rascunho", se a lista de jogos está vazia e se a data de adição é definida corretamente.
        /// </summary>
        /// <remarks> Este teste faz parte da suíte de testes de unidade para a entidade Pedido. </remarks>
        [Fact(DisplayName = "Adicionar Pedido - Deve criar um pedido com sucesso")]
        [Trait("Categoria", "Pedido")]
        public void AdicionarPedido_PedidoValido_DeveCriarPedidoComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act 
            var pedido = new Pedido(usuario.Id);
            //Assert
            Assert.Equal(usuario.Id, pedido.UsuarioId);
            Assert.Equal(PedidoStatus.Rascunho, pedido.Status);
            Assert.Empty(pedido.Jogos);
            Assert.NotEqual(default, pedido.DataCadastro);
        }

        [Fact(DisplayName = "Finalizar Pedido - Deve finalizar o pedido com sucesso")]
        [Trait("Categoria", "Pedido")]
        public void FinalizarPedido_PedidoValido_DeveFinalizarPedidoComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var pedido = new Pedido(usuario.Id);
            var precoVO = new Preco(150.00m);
            //Act 
            pedido.AdicionarItem(Guid.NewGuid(), precoVO);
            pedido.FinalizarPedido();
            //Assert
            Assert.Equal(PedidoStatus.Finalizado, pedido.Status);
            Assert.NotEmpty(pedido.Jogos);
        }


        [Fact(DisplayName = "Falha ao finalizar pedido - deve ter jogos no pedido")]
        [Trait("Categoria", "Pedido")]
        public void FinalizarPedido_PedidoInvalido_SemJogo_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var pedido = new Pedido(usuario.Id);
            //Act 
            var result = Assert.Throws<DomainException>(() => pedido.FinalizarPedido());
            //Assert
            Assert.Equal(MensagensDominio.PedidoSemJogos, result.Message);
        }
    }

}
