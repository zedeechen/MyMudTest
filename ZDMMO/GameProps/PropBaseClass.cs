using System;
using System.Collections.Generic;
namespace ZDMMO
{
    [Serializable]
    public abstract class StateBase{
        protected List<StateBase> mParentProps;
        protected bool mIsDirty;

        protected void MarkAsDirty()
        {
            mIsDirty = true;
            if (mParentProps != null)
            {
                for (int i = 0; i < mParentProps.Count; i++)
                {
                    mParentProps[i].MarkAsDirty();
                }
            }
        } 

        public void AddParentProp(StateBase prop)
        {
            if (mParentProps == null)
                mParentProps = new List<StateBase>();
            if (!mParentProps.Contains(prop))
            {
                mParentProps.Add(prop);
                MarkAsDirty();
            }
        }

        public void RemoveParentProp(StateBase prop)
        {
            if (mParentProps == null)
                mParentProps = new List<StateBase>();
            if (mParentProps.Contains(prop))
            {
                mParentProps.Remove(prop);
                prop.MarkAsDirty();
            }
        }

        public void RemoveAllParentProp()
        {
            if (mParentProps != null)
            {
                StateBase prop;
                for (int i = 0; i < mParentProps.Count; ++i)
                {
                    prop = mParentProps[i];
                    mParentProps.Remove(prop);
                    prop.MarkAsDirty();
                }
                mParentProps.Clear();
            }
        }
    }

    [Serializable]
    public class PropItem<T> : StateBase
    {
        private T mValue;
        public T GetValue()
        {
            return mValue;
        }
        public void SetValue(T val)
        {
            if (!mValue.Equals(val))
            {
                mValue = val;
                MarkAsDirty();
            }
            else
            {
                mValue = val; 
            }
        }
    }

    [Serializable]
    public abstract class PropBase : StateBase
    {
        protected void_void_delegate mCalcFunc;
//#if DEBUG
//        protected string propDesc;
//        protected StringBuilder propDescFormula;
//#endif
        protected void SetFunc(void_void_delegate func)
        {
            mCalcFunc = func;
        }
    }

    [Serializable]
    public class PropGroup<T> : PropBase
    {
        protected Dictionary<int, PropItem<T>> mProps;

        public PropGroup(void_void_delegate func)//, string desc = null)
        {
            SetFunc(func);
//#if DEBUG
//            propDesc = desc;
//            propDescFormula = new StringBuilder();
//#endif
        }

        public PropItem<T> GetPropItem(int key)
        {
            if (mProps == null)
                mProps = new Dictionary<int, PropItem<T>>();

            if (mIsDirty && mCalcFunc != null)
            {
                mCalcFunc();
                mIsDirty = false;
            }

            PropItem<T> prop;
            mProps.TryGetValue(key, out prop);
            return prop;
        }

        public void SetPropItem(int key, T value)//, string formula = null)
        {
            if (mProps == null)
                mProps = new Dictionary<int, PropItem<T>>();

            PropItem<T> prop;
            if (!mProps.TryGetValue(key, out prop))
            {
                prop = new PropItem<T>();
                mProps[key] = prop;
                prop.AddParentProp(this);
            }
//#if DEBUG
//            if (formula == null)
//                propDescFormula.Clear();
//            else
//                propDescFormula.Append(formula);
//#endif
            prop.SetValue(value);
        }
    }

    [Serializable]
    public class PropReference<T> : PropBase
    {
        private PropItem<T> mProp;
        
        public PropReference(void_void_delegate func)
        {
            SetFunc(func);
        }

        public void SetPropItem(T val)
        {
            if (mProp == null)
                mProp = new PropItem<T>();

            mProp.SetValue(val);
        }

        public PropItem<T> GetPropItem()
        {
            if (mIsDirty && mCalcFunc != null)
            {
                mCalcFunc();
                mIsDirty = false;
            }
            return mProp;
        }
    }
}
