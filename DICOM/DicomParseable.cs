﻿// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).


namespace Dicom
{
    using System.Reflection;
    using System.Linq;

    public abstract class DicomParseable
    {
        public static T Parse<T>(string value)
        {
            if (!typeof(T).GetTypeInfo().IsSubclassOf(typeof(DicomParseable))) throw new DicomDataException("DicomParseable.Parse expects a class derived from DicomParseable");

            var method = typeof(T).GetTypeInfo().GetDeclaredMethods("Parse").Single(m => m.IsPublic && m.IsStatic);
            return (T)method.Invoke(null, new object[] { value });
        }
    }
}
