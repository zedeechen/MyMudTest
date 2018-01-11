using System;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class Command
    {
        private string  mName;
        private string  mKey;
        private string  mShortKey;
        private ENM_PARAM_DELEGATE mOperation;
        private object[] mFixedParam;
        private string mPrintDesc;

        private Command mCommandOnSuccess;
        //private object[] mParamOnSuccess;

        public Command(string name, string key, string shortKey, ENM_PARAM_DELEGATE oper, params object[] param)
        {
            mName = name;
            mKey = key;
            mShortKey = shortKey;
            mOperation = oper;
            mFixedParam = param;
            
            StringBuilder sb = new StringBuilder();
            sb.Append(mName);
            if (mKey != null)
            {
                sb.AppendFormat("({0}", mKey);
                if (mShortKey != null)
                {
                    sb.AppendFormat("/{0}", mShortKey);
                }
                sb.Append(")");
            }
            
            mPrintDesc = sb.ToString();
        }
        
        public void DoPrint()
        {
            Console.WriteLine(mPrintDesc);
        }

        private void Execute(object[] param)
        {
            if (mFixedParam != null)
                param = mFixedParam;

            switch (mOperation(param))
            {
                case enmCommandResult.FAILED:
                    SingletonFactory<MapController>.Instance.ShowCurrentMapInfo();
                    break;
                case enmCommandResult.SUCCESS:
                    if (mCommandOnSuccess != null)
                        mCommandOnSuccess.Execute(null);
                    break;
            }
        }

        internal bool TryOperateWithKey(string s, object[] param)
        {
            if ((mKey != null && mKey.ToLower() == s.ToLower())
                || ((mShortKey != null && mShortKey.ToLower() == s.ToLower())))
            {
                if (mOperation != null)
                {
                    Execute(param);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        internal void SetCommandOnSucess(Command command)
        {
            mCommandOnSuccess = command;
        }
    }
}
