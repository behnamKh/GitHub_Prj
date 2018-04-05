using RestaurantPointOfSell.Model;
using RestaurantPointOfSell.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantPointOfSell
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
        
        static void OnProcessExit(object sender, EventArgs e)
        {
            try
            {
                RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
                if (!Properties.Settings.Default.Dev)
                {
                    var dev = db.T_appSettings.FirstOrDefault(x => x.item == "d");
                    dev.value = "0";
                    db.SaveChanges();
                }
            }
            catch 
            {

               
            }
           
        }
    }
}
