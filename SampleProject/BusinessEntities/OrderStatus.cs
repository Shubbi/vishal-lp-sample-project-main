namespace BusinessEntities
{
    //This may not be the most comprehensible list.
    //So for example we may need status Draft for scenario when user 
    //  is online and trying to add items in the cart
    // but has not placed the order and may add/remove more
    //Also there might be status related to payments like WaitingOnPayment, PaymentReceived etc.
    //similarly there could be other statuses as well.
    //For simplicity I'm just keeping few statuses only
    public enum OrderStatus
    {   
        Pending = 1, //order submitted by the user
        Processing = 2, 
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Returned = 6
    }
}
