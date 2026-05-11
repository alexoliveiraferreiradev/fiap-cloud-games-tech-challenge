Feature: Cadastro de Usuários
  Como um visitante do sistema
  Eu quero me cadastrar
  Para ter acesso à plataforma

Background:
Given que o usuário está na página de cadastro


@UsuarioCadastroSucesso
Scenario: Cadastro de usuário com credenciais válidas
 When o usuário submete o formulário com dados válidos
 Then o sistema deve registrar o usuário com sucesso
 And deve gerar um token de acesso válido

@UsuarioCadastroInvalidoNomeNaoPreenchido
Scenario: Cadastro de usuário com credências inválidas - nome não preenchido
  When o usuário submete o formulário com dados válidos de email e senha, porém não preenche o nome
  Then o sistema não registra o usuário 
  And retorna a mensagem de erro sobre o nome obrigatório ao usuário


@UsuarioCadastroInvalidoNomeTamanhoInvalido
Scenario: Cadastro de usuário com credências inválidas - nome tamanho inválido
When o usuário submete o formulário com dados válidos de email e senha, porém preenche um nome com tamanho de caracteres maior ou menor que o permitido
Then o sistema não registra o usuário 
And retorna a mensagem de erro sobre o nome tamanho inválido ao usuário

@UsuarioCadastroInvalidoEmailNaoPreenchido
Scenario: Cadastro de usuário com credências inválidas - email não preenchido
When o usuário submete o formulário com dados válidos de nome de usuário e senha, porém não preenche o email 
Then o sistema não registra o usuário 
And retorna mensagem de erro sobre o email obrigatório ao usuário

@UsuarioCadastroInvalidoEmailTamanhoInvalido
Scenario: Cadastro de usuário credências inválidas - email tamanho inválido
When o usuário submete o formulário com dados válidos de nome de usuário e senha, porém com o tamanho de caractere maior ou menor que o permitido
Then o sistema não registra o usuário
And retorna mensagem de erro sobre o email de tamanho inválido ao usuário

@UsuarioCadastroFormatoEmailIncorreto
Scenario: Cadastro de usuário credências inválidas - formato de email incorreto
When o usuário submete o formulário com dados válidos de nome de usuário e senha, porém o formato do email preenchido está incorreto
Then o sistema não registra o usuário
And retorna mensagem de erro sobre o email em formato inválido ao usuário

@UsuarioCadastroSenhaNaoPreenchida
Scenario: Cadastro de usuário credênciais inválidas - senha não preenchida
When o usuário submete o formulário com dados válidos de nome de usuário e email, porém a senha não é preenchida
Then o sistema não registra o usuário
And retorna a mensagem de erro sobre a senha não preenchida ao usuário 

@UsuarioCadastroSenhaTamanhoInvalido
Scenario: Cadastro de usuário credências inválidas - senha tamanho inválido
When o usuário submete o formulário com dados válidos de nome de usuário e email, porém o tamanho de caracteres da senha está inválido
Then o sistema não registra o usuário 
And retorna a mensagem de erro sobre tamanho de senha inválido ao usuário

@UsuarioCadastroSenhaFraca
Scenario: Cadastro de usuário credências inválidas - senha fraca
When o usuário submete o formulário com dados válidos de nome de usuário e email, porém preenche uma senha fraca
Then o sistema não registra o usuário 
And retorna a mensagem de erro sobre senha informada é fraca ao usuário


