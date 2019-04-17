namespace Server.Models
{
    public class TransactionDto
    {
        /// <summary>
        /// Transaction user name 
        /// </summary>
        public string UserName { get;set; }

        /// <summary>
        /// Transaction Card Number From
        /// </summary>
        /// 
        /// Обезличенная
        public string CardNumberFrom { get;set; }

        /// <summary>
        /// Transaction Card To
        /// </summary>
        /// 
        /// Обезличенная
        public string CardNumberTo { get;set; }

        /// <summary>
        /// Transaction Money
        /// </summary>
        public double Money { get;set; }

        /// <summary>
        /// Transaction Date
        /// </summary>
        public string Date { get;set; }
    }
}