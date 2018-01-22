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
        private string mSystemKey;// ENM_PARAM_DELEGATE mOperation;
        private object[] mFixedParam;
        private string mPrintDesc;

        private Command mCommandOnSuccess;
        //private object[] mParamOnSuccess;

        public bool MRunOnEnterRoom { get; private set; }

        public Command(string name, string key, string shortKey, string systemKey, params object[] param)
        {
            mName = name;
            mKey = key;
            mShortKey = shortKey;
            //mOperation = oper;
            mSystemKey = systemKey;
            mFixedParam = param;
            
            StringBuilder sb = new StringBuilder();
            sb.Append(mName);
            if (!string.IsNullOrEmpty(mKey))
            {
                sb.AppendFormat("({0}", mKey);
                if (!string.IsNullOrEmpty(mShortKey))
                {
                    sb.AppendFormat("/{0}", mShortKey);
                }
                sb.Append(")");
                MRunOnEnterRoom = false;
            }
            else
            {
                MRunOnEnterRoom = true;
            }
            
            mPrintDesc = sb.ToString();
        }
        
        public void DoPrint()
        {
            Console.WriteLine(mPrintDesc);
        }

        public void Execute(object[] param)
        {
            if (mFixedParam != null)
                param = mFixedParam;

            switch (SingletonFactory<CommandController>.Instance.ExecuteCommand(mSystemKey, param))
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
                if (!string.IsNullOrEmpty(mSystemKey))
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
