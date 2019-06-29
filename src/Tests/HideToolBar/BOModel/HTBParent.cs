﻿using DevExpress.Xpo;
using Xpand.XAF.Persistent.BaseImpl;

namespace Xpand.XAF.Modules.HideToolBar.Tests.BOModel{
    public class HTBParent:CustomBaseObject{
        public HTBParent(Session session) : base(session){
        }

        [Association("HTBParent-HTBChilds")]
        public XPCollection<HTBChild> HTBChilds => GetCollection<HTBChild>(nameof(HTBChilds));
    }

    public class HTBChild:CustomBaseObject{
        public HTBChild(Session session) : base(session){
        }

        HTBParent _hTBParent;

        [Association("HTBParent-HTBChilds")]
        public HTBParent HTBParent{
            get => _hTBParent;
            set => SetPropertyValue(nameof(HTBParent), ref _hTBParent, value);
        }
    }
}
