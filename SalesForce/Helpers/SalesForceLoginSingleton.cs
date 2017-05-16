using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Main.Helpers
{
    public sealed class SalesForceLoginSingleton
    {
        private static ThreadLocal<SalesForceSVC.LoginResult> _loginResult;
       

        private SalesForceLoginSingleton() { }

        public static SalesForceSVC.LoginResult Instance
        {
            get
            {
                SalesForceSVC.SoapClient loginClient = new SalesForceSVC.SoapClient();
                SalesForceSVC.LoginScopeHeader loginHeader = new SalesForceSVC.LoginScopeHeader();
                String login = Helpers.Settings.SalesForceUserName, senha = Helpers.Settings.SalesForcePassword + Helpers.Settings.SalesForceToken;

                if (_loginResult == null)
                {
                    _loginResult = new ThreadLocal<SalesForceSVC.LoginResult>(true);
                    _loginResult.Value = loginClient.login(loginHeader, login, senha);

                    return _loginResult.Value;
                }
                else
                {
                    SalesForceSVC.LoginResult instance = null;

                    foreach (SalesForceSVC.LoginResult lr in _loginResult.Values)
                    {
                        if (lr != null)
                        {
                            instance = lr;
                            break;
                        }
                      
                    }

                    return instance;
                }
            }
        }

        public static void KillInstances()
        {
            if (_loginResult.IsValueCreated)
            {
                _loginResult.Dispose();
            }
        }
    }
}
