﻿using DevExpress.Xpo;
using Xpand.XAF.Persistent.BaseImpl;

namespace Xpand.XAF.Modules.MasterDetial.Tests.BOModel{
    public class Md:CustomBaseObject{
        public Md(Session session) : base(session){
        }

        string _propertyName;

        public string PropertyName{
            get => _propertyName;
            set => SetPropertyValue(nameof(PropertyName), ref _propertyName, value);
        }

    }
}