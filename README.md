### Desafio Técnico  
  - Entidades:   
    | Contas | Tipo |
    |-|-|
    | idConta | Numérico |
    | idPessoa | Numérico |
    | saldo | Monetário |
    | limiteSaqueDiario | Monetário |
    | flagAtivo | Condicional |
    | tipoConta | Numérido |
    | dataCriacao | Data |

    ```
    * Tabela de transações realizadas na conta
    ```
    | Transacoes | Tipo |
    |-|-|
    | idTransacao | Numérico |
    | idConta | Numérico |
    | valor | Monetário |
    | dataTransacao | Data |

    ```
    * P.S.: Não é necessário realizar operações com a tabela pessoa, mas é necessária a criação da tabela para mapeamento da relação com a conta e enviar script de criação de pelo menos uma pessoa.
    ```

    | Pessoas | Tipo |
    |-|-|
    | idPessoa | Numérico |
    | nome | Texto |
    | cpf | Texto |
    | dataNascimento | Data |    

  - Escopo:
    ```
    * Implementar path que realiza a criação de uma conta;
    * Implementar path que realiza operação de depósito em uma conta;
    * Implementar path que realiza operação de consulta de saldo em determinada conta;
    * Implementar path que realiza operação de saque em uma conta;
    * Implementar path que realiza o bloqueio de uma conta;
    * Implementar path que recupera o extrato de transações de uma conta;
    ```

### Desafio Dock:
O desafio foi feito utilizando a linguagem c#.
O framework usado foi o net core 5.0 com banco SQL;
Os testes unitários foram feitos usando xunit + autofixture + fluent assertions;

#### Ferramentas necessárias para executar o projeto:
 -Docker (através do docker compose, um comando, será provisionado o ambiente).
 -PowerShell (executar os comandos do docker);
 -Sql server management studio (ou alguma outra ferramenta para logar e executar scripts na sua base de dados);
 - Visual Studio/ Code -> para que os testes unitários sejam executados(net core 5.0 instalado).  
    * P.S.: A execução dos testes ainda não foi automatiza, quando for desenvolvido, ficará direto no "dockerFile" da aplicação, removendo assim essa depêndencia;   
	
#### Como Executar:
- Clonar o projeto;
- Abrir o powerShell;
- Vá para a pasta que contenha o arquivo "docker-compose.yml" (no meu caso é "desafio2");
- Caso você queira manter as informações que serão criadas dentro da sua base de dados, abrir o arquivo "docker-compose.yml" descomentar as linhas(11 e 12) relacionadas a "volume" e colocar um diretório da sua máquina;
- Execute o comando "docker-compose up --build -d";
- Após esse comando será gerado um banco de dados local. Será necessário e popular as a base e as tabelas desse banco de dados.Para isso siga os proximos passos:
	- Fazer login na base local:
		- Logar normalmente pelo sql server management studio;
		  - Abrir o sql server management studio;
		  - Colocar as credenciais : servename "localhost", usuario(sa) e senha (as informações de senha estão no arquivo "docker-compose.yml").
	    - Logar via linha de comando:
			- Será necessario descobrir o nome do seu container use o comando "docker container ps" no meu caso (desafio2_mssql-server_1).
			- Execute o seguinte comando com o nome do seu container "docker exec -it desafio2_mssql-server_1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa".
			- Insira a senha (as informações de senha estão no arquivo "docker-compose").
	- Vá na pasta Scripts procure pelo arquivo "createDatabase", abra-o e execute(será necessario estar logado dentro da sua base) a instrução sql.
	- Após, execute as intruções dentro do arquivo  "createTables" abra-o  e execute.
	- Após, execute as instruções que estão dentro do arquivo "load" para criar uma pessoa.
	
#### Sua aplicação ja encontra-se pronta para ser testada. Vá em http://localhost:8080/swagger para ver e executar todos os endpoints;

PS: As datas seguem o formato : "MM/dd/yyyy". Então na hora de testar por favor se atentar a isso.(Caso o mês possua apenas uma casa decimal, completar com 0 esquerda, exemplo janeiro,1, vira 01);
PS2: Os Valores(amounts/statement) tratados são em reais (R$);
PS3: O endpoint statement, caso nao seja passado valor irá retornar as ultimas 24 horas de transações(com paginação) até agora.
	Caso seja passado data final de periodo, irá buscar ate o ultimo segundo do dia passado. Ex: endDate = 01/20/2022 irá buscar as transações até o ultimo momento do dia 20, no caso 23:59:59;
