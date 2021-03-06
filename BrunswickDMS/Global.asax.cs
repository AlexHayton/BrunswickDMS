﻿using DataLayer.Initializers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BrunswickDMS;

namespace BrunswickDMS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();

            this.InitializeDatabase();
        }

        /// <summary>
        /// Initialize the data layer
        /// </summary>
        private void InitializeDatabase()
        {
            DocumentDatabaseInitializer initializer = new DocumentDatabaseInitializer();
            Database.SetInitializer(new DocumentDatabaseInitializer());
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
