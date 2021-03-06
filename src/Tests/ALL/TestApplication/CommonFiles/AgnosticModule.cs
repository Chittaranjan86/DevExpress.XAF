﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.AuditTrail;
using DevExpress.ExpressApp.Chart;
using DevExpress.ExpressApp.CloneObject;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.ExpressApp.Kpi;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.Objects;
using DevExpress.ExpressApp.PivotChart;
using DevExpress.ExpressApp.PivotGrid;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Scheduler;
using DevExpress.ExpressApp.ScriptRecorder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Xpo;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.TreeListEditors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.ExpressApp.Workflow;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using TestApplication.MicrosoftService;
using TestApplication.MicrosoftTodoService;
using Xpand.TestsLib;
using Xpand.TestsLib.BO;
using Xpand.XAF.Modules.AutoCommit;
using Xpand.XAF.Modules.CloneMemberValue;
using Xpand.XAF.Modules.CloneModelView;
using Xpand.XAF.Modules.HideToolBar;
using Xpand.XAF.Modules.MasterDetail;
using Xpand.XAF.Modules.ModelMapper;
using Xpand.XAF.Modules.ModelViewInheritance;
using Xpand.XAF.Modules.Office.Cloud.Microsoft.Todo;
using Xpand.XAF.Modules.ProgressBarViewItem;
using Xpand.XAF.Modules.Reactive;
using Xpand.XAF.Modules.Reactive.Extensions;
using Xpand.XAF.Modules.Reactive.Logger;
using Xpand.XAF.Modules.Reactive.Logger.Hub;
using Xpand.XAF.Modules.RefreshView;
using Xpand.XAF.Modules.SequenceGenerator;
using Xpand.XAF.Modules.SuppressConfirmation;
using Xpand.XAF.Modules.ViewEditMode;
using Xpand.XAF.Modules.ViewItemValue;

namespace TestApplication{
	public abstract class AgnosticModule:ModuleBase{
		protected AgnosticModule(){
			#region XAF Modules

			RequiredModuleTypes.Add(typeof(AuditTrailModule));
			RequiredModuleTypes.Add(typeof(ChartModule));
			RequiredModuleTypes.Add(typeof(CloneObjectModule));
			RequiredModuleTypes.Add(typeof(ConditionalAppearanceModule));
			RequiredModuleTypes.Add(typeof(DashboardsModule));
			RequiredModuleTypes.Add(typeof(KpiModule));
			RequiredModuleTypes.Add(typeof(NotificationsModule));
			RequiredModuleTypes.Add(typeof(BusinessClassLibraryCustomizationModule));
			RequiredModuleTypes.Add(typeof(PivotChartModuleBase));
			RequiredModuleTypes.Add(typeof(PivotGridModule));
			RequiredModuleTypes.Add(typeof(ReportsModuleV2));
			RequiredModuleTypes.Add(typeof(SchedulerModuleBase));
			RequiredModuleTypes.Add(typeof(ScriptRecorderModuleBase));
			RequiredModuleTypes.Add(typeof(SecurityModule));
			RequiredModuleTypes.Add(typeof(SecurityXpoModule));
			RequiredModuleTypes.Add(typeof(StateMachineModule));
			RequiredModuleTypes.Add(typeof(TreeListEditorsModuleBase));
			RequiredModuleTypes.Add(typeof(ModuleBase));
			RequiredModuleTypes.Add(typeof(SystemModule));
			RequiredModuleTypes.Add(typeof(ValidationModule));
			RequiredModuleTypes.Add(typeof(ViewVariantsModule));
			RequiredModuleTypes.Add(typeof(WorkflowModule));
			RequiredModuleTypes.Add(typeof(ServerUpdateDatabaseModule));

			#endregion

            AdditionalExportedTypes.Add(typeof(Order));

			RequiredModuleTypes.Add(typeof(AutoCommitModule));
			RequiredModuleTypes.Add(typeof(CloneMemberValueModule));
			RequiredModuleTypes.Add(typeof(CloneModelViewModule));
			RequiredModuleTypes.Add(typeof(HideToolBarModule));
			RequiredModuleTypes.Add(typeof(MasterDetailModule));
			RequiredModuleTypes.Add(typeof(ModelMapperModule));
			RequiredModuleTypes.Add(typeof(ModelViewInheritanceModule));
			RequiredModuleTypes.Add(typeof(MicrosoftTodoModule));
			RequiredModuleTypes.Add(typeof(ProgressBarViewItemModule));
			RequiredModuleTypes.Add(typeof(ReactiveModule));
			RequiredModuleTypes.Add(typeof(ReactiveLoggerModule));
			RequiredModuleTypes.Add(typeof(RefreshViewModule));
			RequiredModuleTypes.Add(typeof(SequenceGeneratorModule));
			RequiredModuleTypes.Add(typeof(SuppressConfirmationModule));
			RequiredModuleTypes.Add(typeof(ViewEditModeModule));
			RequiredModuleTypes.Add(typeof(ViewItemValueModule));
			RequiredModuleTypes.Add(typeof(ReactiveLoggerHubModule));
			AdditionalExportedTypes.Add(typeof(Task));
		}

		public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB){
			base.GetModuleUpdaters(objectSpace, versionFromDB);
			yield return new DefaultUserModuleUpdater(objectSpace, versionFromDB);
		}

		public override void Setup(XafApplication application){
			base.Setup(application);
			application.Security = new SecurityStrategyComplex(typeof(PermissionPolicyUser),
				typeof(PermissionPolicyRole), new AuthenticationStandard(typeof(PermissionPolicyUser), typeof(AuthenticationStandardLogonParameters)));
		}

		public override void Setup(ApplicationModulesManager moduleManager){
			base.Setup(moduleManager);
			moduleManager.ConnectMicrosoftService()
                .Merge(moduleManager.ConnectMicrosoftTodoService())
                .Subscribe(this);
        }
	}
}