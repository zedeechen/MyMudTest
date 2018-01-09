namespace MyMMOFramework
{
    /// <summary>
    /// 针对随等级提升的属性提升
    /// propValue对应等级
    /// baseProps 存储等级对应的基础属性
    /// SetValue()中重算RefreshPropsFunc对应的属性算法，并标记需重算(isDirty)
    /// GetValue()获取当前等级
    /// </summary>
    public class NonBaseProp : Prop
    {
        protected int propValue;

        public NonBaseProp()
        {
            isDirty = true;
        }

        //public override void Execute()
        //{
        //    if (isDirty)
        //    {
        //        if (RefreshPropsFunc != null)
        //        {
        //            //baseProps = RefreshPropsFunc();
        //            RefreshPropsFunc();
        //        }
        //    }
        //    isDirty = false;
        //}

        public void SetValue(int value)
        {
            if (propValue != value)
            {
                IsDirty = true;
            }
            propValue = value;

        }

        public int GetValue()
        {
            return propValue;
        }
    }
}
