using System.Text;

namespace GameSample
{
    public class Command
    {
        private string  mName;
        private string  mKey;
        private string  mShortKey;
        private VOID_PARAM_DELEGATE mOperation;
        private object[] mFixedParam;
        private string mPrintDesc;

        public Command(string name, string key, string shortKey, VOID_PARAM_DELEGATE oper, params object[] param)
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
                //if (mShortKey != null && mKey.Substring(0, 1) == mShortKey)
                //{
                    
                //}
                //else
                //{
                    sb.AppendFormat("({0}", mKey);
                    if (mShortKey != null)
                    {
                        sb.AppendFormat("/{0}", mShortKey);
                    }
                    sb.Append(")");
                //}
            }
            
            mPrintDesc = sb.ToString();
        }
        
        public string DoPrint()
        {
            return mPrintDesc;
        }

        internal bool TryOperateWithKey(string s, object[] param)
        {
            if ((mKey != null && mKey.ToLower() == s.ToLower())
                || ((mShortKey != null && mShortKey.ToLower() == s.ToLower())))
            {
                if (mOperation != null)
                {
                    if (mFixedParam != null)
                        mOperation(mFixedParam);
                    else
                        mOperation(param);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
