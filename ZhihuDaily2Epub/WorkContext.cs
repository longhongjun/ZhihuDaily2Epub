namespace ZhihuDaily2Epub
{
    public class WorkContext
    {
        private static object LockObj = new object(); 

        private static dynamic _config;
        public static dynamic Config
        {
            get
            {
                if (_config == null)
                {
                    lock (LockObj)
                    {
                        if (_config == null)
                        {
                            _config =  JsonConfig.Config.Default;
                        }
                    }
                }
                return _config;
            }
        } 
    }
}