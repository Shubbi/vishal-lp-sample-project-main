using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    //Just to have fast implementation 
    //Making customer as a type of user, even though MonthlySalary may not necessarily go with customer
    //Also I noticed that you had 3 types of users, so tried to built on top of it 
    public class Customer : User
    {
        public Customer()
        {
            this.SetType(UserTypes.Customer);
        }

        //I'm not using fields like State City, Zipcode etc. for now
        //Keeping it very minimal
        public string Address {  get; set; }
    }
}
