namespace ServiceBus
{
    public record OrderCreatedEvent(int orderId, Dictionary<int, int> stockInfo);

    //public class OrderCreatedEvent
    //{
    //    public OrderCreatedEvent(int orderId, Dictionary<int, int> stockInfo)
    //    {
    //        OrderId = orderId;
    //        StockInfo = stockInfo;
    //    }

    //    // init => immituble after creation

    //    public int OrderId { get; init; }

    //    public Dictionary<int,int> StockInfo { get; init; }

    //}




    //Fonksiyonel Programlama Paradigması
    //    1. İmmutable Nesneler ile Çalışmak
    //    2. Metodlara argüman olarak fonksiyon geçmek
    //    3. Yan Etkisiz Programlama (Side-Effect Free Programming)
      
        


}


