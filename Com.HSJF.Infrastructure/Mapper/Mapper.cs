using EmitMapper;

namespace Com.HSJF.Infrastructure.Mapper
{
    /// <summary>
    /// 对象映射类
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TSourse">映射来源对象类型</typeparam>
        /// <typeparam name="TDest">映射目标对象类型</typeparam>
        /// <param name="src">映射源对象</param>
        /// <returns>映射结果</returns>
        public static TDest Map<TSourse, TDest>(TSourse src)
        {
           var mapper=ObjectMapperManager.DefaultInstance.GetMapper<TSourse, TDest>();
            return mapper.Map(src);
        }
    }
}
