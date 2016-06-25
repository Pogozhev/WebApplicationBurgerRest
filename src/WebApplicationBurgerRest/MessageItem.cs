using Microsoft.WindowsAzure.Storage.Table;

namespace WebApplicationBurgerRest
{
    public class MessageItem : TableEntity
    {
        /// <summary>
        /// Объект хранения сообщений
        /// </summary>
        /// <param name="idChat">id Chat</param>
        /// <param name="idMessage">id Message</param>
        /// <param name="idUser">id User</param>
        /// <param name="typeUser">Type User</param>
        /// <param name="messageSource">Message source (Messenger)</param>
        /// <param name="dateTime">Время создания</param>
        /// <param name="status">Статус сообщения</param>
        /// <param name="messageData">Данные</param>
        public MessageItem(string idChat, string idMessage, string idUser, string typeUser, string messageSource, string dateTime, string status, string messageData)
        {
            this.PartitionKey = idChat;
            this.RowKey = idMessage;
            IdChat = idChat;
            IdMessage = idMessage;
            IdUser = idUser;
            TypeUser = typeUser;
            MessageSource = messageSource;
            DateTime = dateTime;
            Status = status;
            MessageData = messageData;
        }

        public MessageItem() { }
        public string IdChat { get; set; }
        public string IdMessage { get; set; }
        public string IdUser { get; set; }
        public string TypeUser { get; set; }
        public string DateTime { get; set; }
        public string MessageSource { get; set; }
        public string Status { get; set; }
        public string MessageData { get; set; }
    }
}
