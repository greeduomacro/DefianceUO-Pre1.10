namespace Server
{
    class StaticConfiguration
    {
        // Server Configuration
        //---------------------
        /* Address:
         *
         * The default setting, a value of 'null', will attempt to detect your IP address automatically:
         * public const string ServerAddress = "null";
         *
         * This detection, however, does not work for servers behind routers. If you're running behind a router, put in your IP:
         * public const string ServerAddress = "12.34.56.78";
         *
         * If you need to resolve a DNS host name, you can do that too:
         * public const string ServerAddress = "shard.host.com";
         */
        public const string ServerName     = "Defiance UOR";
        public const string ServerAddress  = "193.192.51.4";
        public const int    ServerPort     = 2594;


        // Testcenter configuration
        //-------------------------
        public const bool TestCenterActive = false;


        // MyRunUO
        public const bool MyRunUOActive = false;


        // Database configuration
        //-----------------------
        public static readonly string AccountDatabaseConnectString =
		"Server=dfi.defianceuo.com;Database=ShardInfo;User ID=defiance_tiger;password=tiEpbMTc577Zh00;port=3306";


        public static readonly string DonationDatabaseConnectString =
		"Server=www.defianceuo.com;Database=defiance_eshop.DISABLED;User ID=defiance_tiger;password=oZam7jNlrOD4BTG;port=3306";


	public static readonly string ModsDatabaseConnectString =
		"Server=dfi.defianceuo.com;Database=uomods;User ID=uomods;password='Jai5eighoh';port=3306";

        public static readonly string MyRunUODatabaseConnectString =
				"Server=213.228.232.50;Database=MyDefiance;User ID=nick;password='1nfr4p4ss';port=3306";

    }
}