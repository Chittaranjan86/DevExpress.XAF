﻿using DevExpress.Xpo;

namespace DevExpress.XAF.Agnostic.Specifications.Modules.ModelViewInheritance.BOModel{
    public class AMvi: ABaseMvi{
        public AMvi(Session session) : base(session){
        }

        int _quantity;

        public int Quantity{
            get => _quantity;
            set => SetPropertyValue(nameof(Quantity), ref _quantity, value);
        }


        [Association("AMvi-FileMvis")]
        public XPCollection<FileMvi> FileMvis => GetCollection<FileMvi>(nameof(FileMvis));
    }
}