using System;
using System.Collections.Generic;

namespace MyMMOFramework
{
    public enum PropType
    {
        POWER = -1,
        MIN = 0,
        HP = 0,
        ATK = 1,
        DEF = 2,
        MAX = 2,
        
    };

    public class PropUtils
    {
        public static string GetPropName(PropType type)
        {
            switch (type)
            {
                case PropType.HP:
                    return "生命";
                case PropType.ATK:
                    return "攻击";
                case PropType.DEF:
                    return "防御";
                default:
                    return string.Empty;
            }
        }
    }

    public abstract class Prop
    {
        public virtual void Execute() { }

        public Dictionary<PropType, BaseProp> baseProps = null;
        public delegate void RefreshPropsDelegate();

        protected RefreshPropsDelegate RefreshPropsFunc;

        protected bool isDirty;
        protected List<Prop> parentProps;

        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
                if (parentProps != null)
                {
                    foreach (Prop prop in parentProps)
                    {
                        prop.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// 建立各层属性间的关联。下层属性为分割不同功能的属性（如装备基础属性、装备强化属性）上层属性为总和属性（如装备总属性）
        /// </summary>
        /// <param name="parentProp"></param>
        public void AddRelation(Prop parentProp)
        {
            if (parentProps == null)
                parentProps = new List<Prop>();

            parentProps.Add(parentProp);
            parentProp.IsDirty = true;
        }

        /// <summary>
        /// 移除关联（如卸下装备）
        /// </summary>
        /// <param name="parentProp"></param>
        public void RemoveRelation(Prop parentProp)
        {
            parentProps.Remove(parentProp);
            parentProp.IsDirty = true;
        }

        public void AddProp(Prop propAdd)
        {
            propAdd.Execute();
            if (propAdd.baseProps != null)
            {
                foreach (PropType pt in propAdd.baseProps.Keys)
                {
                    if (baseProps.ContainsKey(pt))
                    {
                        baseProps[pt].SetValue(baseProps[pt].GetValue() + propAdd.baseProps[pt].GetValue());
                    }
                    else
                    {
                        baseProps[pt].SetValue(propAdd.baseProps[pt].GetValue());
                    }
                }
            }
        }

        public void CopyProp(Prop prop)
        {
            prop.Execute();
            baseProps.Clear();
            if (prop.baseProps != null)
            {
                foreach (PropType pt in prop.baseProps.Keys)
                {
                    if (baseProps.ContainsKey(pt))
                    {
                        baseProps[pt].SetValue(prop.baseProps[pt].GetValue());
                    }
                    else
                    {
                        baseProps[pt] = new BaseProp(pt, prop.baseProps[pt].GetValue());
                    }
                }
            }
        }

        public void SetProp(PropType type, float val)
        {
            if (baseProps == null)
                baseProps = new Dictionary<PropType, BaseProp>();

            BaseProp prop;
            if (!baseProps.TryGetValue(type, out prop))
            {
                prop = new BaseProp(type, val);
                baseProps[type] = prop;
            }
            else
            {
                prop.SetValue(val);
            }
        }

        public void AddProp(PropType type, float addVal)
        {
            if (baseProps == null)
                baseProps = new Dictionary<PropType, BaseProp>();

            BaseProp prop;
            if (!baseProps.TryGetValue(type, out prop))
            {
                prop = new BaseProp(type, addVal);
                baseProps[type] = prop;
            }
            else
            {
                prop.SetValue(prop.GetValue() + addVal);
            }
        }

        public void PropMultiply(float mul)
        {
            foreach (PropType pt in baseProps.Keys)
            {
                baseProps[pt].SetValue(baseProps[pt].GetValue() * mul);
            }
        }
    }
}
