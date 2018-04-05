using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPointOfSell.Services
{
    public static class  DataTransfer
    {
        //Static Value
        public static string SResturantName = "";
        public static int SPrintNum = 0;


        //regular
        public static int groupId = 0;
        public static string groupName = "";
        public static DataTable invoice;
        public static string itemName = "";
        public static string itemComment = "";
        public static int itemNumber = 0;
        public static float itemPrice = 0;
        public static bool authen = false;
        public static string tableNumber = "";
        public static float total = 0;

        //Print
        public static string ppay = "";
        public static string pcustomer = "";
        public static string ptd = "";
        public static string ptotal = "";

        //ManageBill
        public static int mId = 0;
        public static string mOrderNum = "";
        public static string mPayment = "";
        public static string mTD = "";
        public static float mTotal = 0;

        //Paid Form
        public static string PaidOrderNum = "";
        public static string Paidmethod = "";


    }
}
