using System.Text.Json.Serialization;
using dotenv.net;
using MailKit.Net.Smtp;
using MimeKit;
using stock_quote_alert.Services.JSONServices;

namespace stock_quote_alert.Services.EmailServices
{
    class EmailHandler
    {
        public readonly string title;
        private readonly string Mailtrap_Key;
        private readonly string Mailtrap_username;
        private readonly string Mailtrap_Host;
        private readonly int Mailtrap_PORT;
        private readonly List<Subscribers> Subscribers;

        public EmailHandler()
        {
            DotEnv.Load();
            Mailtrap_Key = Environment.GetEnvironmentVariable("Mailtrap_Key") ?? "";
            Mailtrap_PORT = int.Parse(Environment.GetEnvironmentVariable("Mailtrap_PORT") ?? "-1");
            Mailtrap_username = Environment.GetEnvironmentVariable("Mailtrap_Username") ?? "";
            Mailtrap_Host = Environment.GetEnvironmentVariable("Mailtrap_Host") ?? "";
            var subscribersString = Environment.GetEnvironmentVariable("SubscribersMail") ?? "";
            Subscribers = JSONConverter.ParseSubscribersJSON2Object(subscribersString);
            title = "Stock Alert";
        }

        /// <summary>
        /// Build Text to given Stock and Subscriber
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="stockDataRecord"></param>
        /// <param name="monitoredValue"></param>
        /// <param name="events"></param>
        /// <returns></returns>

        public static string BuildTextBody(
            Subscribers subscriber,
            StockDataRecord stockDataRecord,
            decimal monitoredValue,
            Events events
        )
        {
            string advisedAction = events switch
            {
                Events.TAKE_PROFIT => "vender e realizar lucros",
                Events.BUY_THE_DIP => "comprar e aproveitar a baixa",
                _ => "",
            };
            string orderType = events switch
            {
                Events.TAKE_PROFIT => "Preço de Venda",
                Events.BUY_THE_DIP => "Preço de Compra",
                _ => "",
            };

            string textBody =
                $@"
                    <!DOCTYPE html>
                    <html lang='pt-BR'>
                    <head>
                        <meta charset='UTF-8'>
                        <title>Alerta de Preço da Ação</title>
                    </head>
                    <body>
                        <p>📈 <strong>Alerta de Preço da Ação</strong> - {stockDataRecord.MarketTime}</p>

                        <p>Olá, <strong>{subscriber.Name}</strong></p>

                        <p>Gostaríamos de informá-lo que o preço da ação da <strong>**{stockDataRecord.Name}**</strong> atingiu o valor que você estava monitorando.</p>

                        <ul>
                            <li><strong>Ação</strong>: {stockDataRecord.Symbol} - {stockDataRecord.Name}</li>
                            <li><strong>Preço Atual</strong>: {stockDataRecord.Price} BRL</li>
                            <li><strong>{orderType}</strong>: {monitoredValue} BRL</li>
                        </ul>

                        <p>Este é um ótimo momento para revisar sua estratégia de investimento, para <strong>{advisedAction}</strong>.</p>

                        <p><strong>Estamos aqui para ajudar!</strong><br>
                        Se você tiver alguma dúvida ou precisar de assistência, não hesite em nos contatar.</p>

                        <p><strong>Lembrete:</strong> Esta é uma notificação automática baseada no valor definido por você. As condições do mercado podem mudar rapidamente, portanto, mantenha-se atualizado.</p>

                        <p>Atenciosamente,</p>

                        <p>Equipe de Alerta de Investimentos<br>Stock Alert Demo</p>

                    </body>
                    </html>
                    ";

            return textBody;
        }
        /// <summary>
        /// Notifies Subscribers for given event
        /// </summary>
        /// <param name="stockDataRecord"></param>
        /// <param name="monitoredValue"></param>
        /// <param name="events"></param>
        public void NotifySubscribers(
            StockDataRecord stockDataRecord,
            decimal monitoredValue,
            Events events
        )
        {
            string messageType = events switch
            {
                Events.TAKE_PROFIT => "VENDE QUE TÁ NA ALTA!!",
                Events.BUY_THE_DIP => "COMPRA QUE TÁ NA BAIXA",
                _ => "",
            };

            foreach (var subscriber in Subscribers)
            {
                var textBody = BuildTextBody(subscriber, stockDataRecord, monitoredValue, events);
                var email = BuildEmail(subscriber.Email, subscriber.Name, messageType, textBody);
                Console.WriteLine($"Preparing to Notify {subscriber.Name} at email {subscriber.Email}");
                SendMail(email);
            }
        }
        /// <summary>
        /// Build Email to be send
        /// </summary>
        /// <param name="subscriberEmail"></param>
        /// <param name="subscriberName"></param>
        /// <param name="messageType"></param>
        /// <param name="textBody"></param>
        /// <returns></returns>
        public MimeMessage BuildEmail(
            string subscriberEmail,
            string subscriberName,
            string messageType,
            string textBody
        )
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(title, Mailtrap_username));
            email.To.Add(new MailboxAddress(subscriberName, subscriberEmail));
            email.Subject = messageType;
            email.Body = new TextPart("html") { Text = textBody };
            return email;
        }

        public void SendMail(MimeMessage email)
        {
            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(
                    Mailtrap_Host,
                    Mailtrap_PORT,
                    MailKit.Security.SecureSocketOptions.StartTls
                );
                smtp.Authenticate(Mailtrap_username, Mailtrap_Key);
                smtp.Send(email);
                Console.WriteLine($"Sent");
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
