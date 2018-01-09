using System.Collections.Generic;

namespace MyMMOFramework
{
    /// <summary>
    /// 针对对象的总属性值
    /// 
    /// baseProps 存储总属性
    /// 
    /// GetValue()获取指定属性类型的值
    /// </summary>
    public class BasePropGroup : Prop
    {
        public BasePropGroup(RefreshPropsDelegate numFunc)
        {
            RefreshPropsFunc = numFunc;
            if (numFunc != null)
            {
                baseProps = new Dictionary<PropType, BaseProp>();
            }
            isDirty = true;
        }

        public override void Execute()
        {
            if (isDirty)
            {
                if (RefreshPropsFunc != null)
                {
                    //baseProps = RefreshPropsFunc();
                    RefreshPropsFunc();
                }
            }
            isDirty = false;
        }

        public float GetValue(PropType type)
        {
            Execute();

            if (baseProps != null)
            {
                BaseProp bp;
                baseProps.TryGetValue(type, out bp);
                if (bp != null)
                    return bp.GetValue();
            }
            return 0f;
        }
    }

    /// <summary>
    /// 对应单个属性项的属性值
    /// </summary>
    public class BaseProp : Prop
    {
        protected PropType propType;
        protected float propValue;
        //protected float propValue;
        public BaseProp(PropType type, float value)
        {
            propType = type;
            propValue = value;
        }
        public PropType GetPropType()
        {
            return propType;
        }

        public void SetValue(float value)
        {
            propValue = value;
        }
        public float GetValue()
        {
            return propValue;
        }
    }
}
