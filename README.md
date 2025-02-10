### Como Executar o Projeto local

#### Pré-requisitos para Executar
1. **.NET Core**
2. **SQL Server**
3. **RabbitMQ**

#### Configuração Inicial

1. **Configurar o banco de dados**
Connection string está setada em launchsettings.json

Criar um banco de dados com o nome de Ambev. Abaixo está a configura para o ambiente que foi feito o teste:
  "Server=ORDNAEL\\SQLEXPRESS;Database=Ambev;Trusted_Connection=True;TrustServerCertificate=True"
   

2. **Rodar as Migrations**
Executar Update-Database para criar as tabelas.

3. **Configuração RabbitMQ**
As configurações do rabbit encontra-se em launchsettings.json.
Alterar de acordo com sua instancia local do rabbitmq:
    "RABBITMQHOSTNAME": "localhost",
    "RABBITMQUSERNAME": "guest",
    "RABBITMQPASSWORD": "guest",
    "RABBITMQVIRTUALHOST": "localhost"


