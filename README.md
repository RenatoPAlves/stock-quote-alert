# stock-quote-alert

O Projeto se propõe a monitora valores de ações da B3, e enviar uma mensagem por email quando for verificado o atendimento de uma das condições. Seja a de venda pela ação encontrar-se num momento de alta, ou de compra ao se verificar uma baixa no mercado para tal ação.

## Índice

- [Compilação](#compilação)
- [Uso](#uso)
- [Funcionalidades](#funcionalidades)

## Compilação

`bash dotnet build`

Caso deseje rodar com a extensão .exe basta: 

`bash dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./release`

Substituindo "win-x64" pela distribuição presente na máquina.

## Uso
Basta adicionar o nome e o email do inscrito no arquivo .env para que seja enviado email a tal inscrito, como por exemplo:

SubscribersMail=[{"email":"inscrito@email.com","name":"João Silva"}]

`bash dotnet run -- [StockName] [SellMonitorPrice] [BuyMonitorPrice]`

Exemplo : `bash dotnet run -- "PETR4"  38.67 36.59`

## Classes

**Events Enum** : enumera eventos de interesse para o cliente, e torna uma solução escalável, para monitoramento de ativos de forma mais complexa do que uma mera comparação de valores de venda e compra.

**StockDataRecord** : armazena dados importantes da Stock num objeto, para facilitar sua manipulação.

**APIHandler** : monitora os ativos atráves de requisição Http. Roda num loop infinito a cada 30 minutos (limitação da versão gratuita da BRAPI). A ideia de rodar "infinitamente", vem de uma tentativa de fazer a requisição HTTP se assemelhar a uma requisição utilizando WebSocket

**Services** : separação dos serviços presentes na aplicação, a fim de garantir um alto acoplamento.

## Funcionalidades

Basicamente, a aplicação guarda a lista de usuários inscritos para receber notificações.
Roda o método APIHandler.**Subscribe2Stock**(symbol,sell,buy), o qual por sua vez ao verificar o atendimento de uma das condições monitoradas, chama o método **NotifySubscribers**(stockDataRecord,monitoredValue,events) o qual enviará uma mensagem por email de acordo com a condição verificada.
