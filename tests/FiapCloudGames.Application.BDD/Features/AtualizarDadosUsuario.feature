Feature: AtualizarDadosUsuario

Como um usuário logado
Eu quero atualizar meus dados
Para manter meu perfil em dia

Background:
Given que o usuário já está cadastrado e quer atualizar suas informações

@AtualizarNomeUsuarioSucesso
Scenario: Atualizar nome do usuário - dados válidos e nome único
	When o usuário submete um novo nome de usuário válido
	Then o sistema atualiza os dados daquele usuário

@AtualizarNomeUsuarioInvalido
Scenario: Atualizar nome do usuário - o novo nome inserido já pertence a outro usuário
When  o usuário submete um novo nome de usuário, porém o novo nome é pertencente a outro usuário
Then  o sistema não atualiza os dados do usuário 
And retorna mensagem de erro em relação ao nome do usuário já estar cadastrado

@AtualizarSenhaSucesso
Scenario: Atualizar a senha do usuário - dados válidos
When  o usuário submete uma nova senha válida 
Then  o sistema atualiza os dados daquele usuário

@AtualizarSenhaTamanhoInvalido
Scenario: Atualizar a senha do usuário - tamanho senha inválida 
When  o usuário submete uma nova senha, porém com o tamanho de caracteres inválidos
Then o sistema não atualiza os dados do usuário 
And retorna mensagem de erro em relação ao tamanho de caracteres da senha ao usuário

@AtualizarUsuarioUsuarioNaoEncontrado
Scenario: Atualizar o usuario - usuário não encontrado
When o usuário submete um novo nome de usuário e senha válidos, porém o Id do usuário não é encontrado
Then o sistema não atualiza os dados do usuário
And retorna mensagem de erro de usuário não encontrado

