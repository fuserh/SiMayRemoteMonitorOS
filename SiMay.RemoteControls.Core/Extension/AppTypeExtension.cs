﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SiMay.Basic;
using SiMay.Core;

namespace SiMay.RemoteControls.Core
{
    public static class AppTypeExtension
    {
        public static PropertyInfo GetApplicationAdapterPropertyByName(this IApplication application, string appKey)
        {
            return GetApplicationAdapterPropertyByName(application.GetType(), appKey);
        }

        public static PropertyInfo[] GetApplicationAdapterProperty(this IApplication application)
        {
            return GetApplicationAdapterProperty(application.GetType());
        }

        /// <summary>
        /// 根据ApplicationKey查找应用适配器属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public static PropertyInfo GetApplicationAdapterPropertyByName(this Type type, string appKey)
        {
            return GetApplicationAdapterProperty(type).FristOrDefault(c => c.PropertyType.GetApplicationName().Equals(appKey, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 取应用所有适配器类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetApplicationAdapterProperty(this Type type)
        {
            var propertys = type
                .GetProperties();
            return propertys
                .Where(c => !c.GetCustomAttribute<ApplicationAdapterHandlerAttribute>(true).IsNull() && typeof(ApplicationBaseAdapterHandler).IsAssignableFrom(c.PropertyType))
                .ToArray();
        }

        //public static string[] GetActivateApplicationKey(this ApplicationAdapterHandler adapter)
        //{
        //    return GetActivateApplicationKey(adapter.GetType());
        //}

        /// <summary>
        /// 取应用所有应用适配器ApplicationId
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetActivateApplicationKey(this Type type)
        {
            var applicationName = type.Name;
            return type.GetApplicationAdapterProperty().Select(c => $"{applicationName}.{c.PropertyType.GetApplicationName() }").ToArray();
        }

        /// <summary>
        /// 获取适配器ApplicationKey
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        public static string GetApplicationName(this ApplicationBaseAdapterHandler adapter)
        {
            return GetApplicationName(adapter.GetType());
        }

        /// <summary>
        /// 根据适配器类型获取ApplicationKey
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetApplicationName(this Type type)
        {
            var attr = type.GetCustomAttribute<SiMay.Core.ApplicationNameAttribute>(true);
            return attr.Name;
        }
    }
}
