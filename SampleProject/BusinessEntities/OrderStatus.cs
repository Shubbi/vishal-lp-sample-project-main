namespace BusinessEntities
{
    //This may not be the most comprehensible list.
    //So for example we may need status Draft for scenario when User(Customer/Admin) 
    //is online and trying to add items in the cart
    //but has not placed the order and may add/remove more
    //Also there might be status related to payments like WaitingOnPayment, PaymentReceived etc.
    //similarly there could be other statuses as well.
    //For simplicity I'm just keeping few statuses only    
    public enum OrderStatus
    {   
        Pending = 1, //order submitted 
        Processing = 2, //only Admin  
        Cancelled = 3, //cancellation can only happen if the order is in Pending/Processing Status
        Shipped = 4, //only Admin
        Delivered = 5, //only Admin - or - might be application from a trusted Career like UPS, USPS, FedEx etc.        
        ReturnRequested = 6, // ReturnRequested can only happen if the order is Delivered - Only Admin
        Returned = 7 // Return can only be done by Admin 
    }
}
