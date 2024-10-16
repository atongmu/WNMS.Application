using Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WNMS.IService;
using WNMS.Service;

namespace WNMS.Application.Utility
{
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        { 
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var builder = new ContainerBuilder();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();



            containerBuilder.Register(c => new CustomAutofacAop());//aop注册  

            containerBuilder.RegisterType<Model.DataModels.WNMSContext>().Named<DbContext>("databaseA").InstancePerLifetimeScope();
            containerBuilder.RegisterType<Model.GPRSModels.gprsContext>().Named<DbContext>("databaseB").InstancePerLifetimeScope();

            containerBuilder.RegisterType<Cache.MemoryCacheExtensions>().As<Cache.ICache>();

            containerBuilder.RegisterType<SysUserService>().As<ISysUserService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_RoleService>().As<ISys_RoleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_UserRoleService>().As<ISys_UserRoleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_AreaService>().As<ISys_AreaService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_LogService>().As<ISys_LogService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_DepartMentService>().As<ISys_DepartMentService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_DataItemDetailService>().As<ISys_DataItemDetailService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_DataItemService>().As<ISys_DataItemService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_RoleModuleService>().As<ISys_RoleModuleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_ModuleService>().As<ISys_ModuleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_ModuleButtonService>().As<ISys_ModuleButtonService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_DeviceInfo01Service>().As<ISws_DeviceInfo01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_DeviceInfo02Service>().As<ISws_DeviceInfo02Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_DeviceInfo03Service>().As<ISws_DeviceInfo03Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_StationService>().As<ISws_StationService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_GUIInfoService>().As<ISws_GUIInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_RTUInfoService>().As<ISws_RTUInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_UserStationService>().As<ISws_UserStationService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<AttachmentService>().As<IAttachmentService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_UserModuleService>().As<ISys_UserModuleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventHistoryService>().As<ISws_EventHistoryService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventInfoService>().As<ISws_EventInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_CutOffMessageService>().As<ISws_CutOffMessageService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_DataInfoService>().As<ISws_DataInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_PropertyInfoService>().As<ISws_PropertyInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_TemplateService>().As<ISws_TemplateService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventInfoService>().As<ISws_EventInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<HourQuartZService>().As<IHourQuartZService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<DayQuartZService>().As<IDayQuartZService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<MonthQuartZService>().As<IMonthQuartZService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventHistoryService>().As<ISws_EventHistoryService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventHandleService>().As<ISws_EventHandleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventAttentionService>().As<ISws_EventAttentionService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_AccessControlService>().As<ISws_AccessControlService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_AccessHistoryService>().As<ISws_AccessHistoryService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_CameraService>().As<ISws_CameraService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_ComTypesService>().As<ISws_ComTypesService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_DPCInfoService>().As<ISws_DPCInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<View_DeviceInfoService>().As<IView_DeviceInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<MonthQuartZ01Service>().As<IMonthQuartZ01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<DayQuartZ01Service>().As<IDayQuartZ01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_DeviceTemplateService>().As<ISws_DeviceTemplateService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_ValveWith01Service>().As<ISws_ValveWith01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<DDayQuartZ01Service>().As<IDDayQuartZ01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<DDayQuartZ02Service>().As<IDDayQuartZ02Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<DHourQuartZ01Service>().As<IDHourQuartZ01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<DMonthQuartZ01Service>().As<IDMonthQuartZ01Service>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_TeamInfoService>().As<IGD_TeamInfoService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_InspectionService>().As<IGD_InspectionService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_RepairService>().As<IGD_RepairService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_ResourceService>().As<IGD_ResourceService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_EventsService>().As<IGD_EventsService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_WorkOrderService>().As<IGD_WorkOrderService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_WOOperationService>().As<IGD_WOOperationService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_MaintainService>().As<IGD_MaintainService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_RepairService>().As<IGD_RepairService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_TeamUserService>().As<IGD_TeamUserService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_WOExtensionService>().As<IGD_WOExtensionService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<GD_WOReviewService>().As<IGD_WOReviewService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<JR_AreaService>().As<IJR_AreaService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseB"));
            containerBuilder.RegisterType<Sws_AccessoriesEquService>().As<ISws_AccessoriesEquService>().WithParameter((pi, c) => pi.Name == "content",
                     (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_AcccessoriesChartService>().As<ISws_AcccessoriesChartService>().WithParameter((pi, c) => pi.Name == "content",
                     (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_AccessoriesMaintenanceService>().As<ISws_AccessoriesMaintenanceService>().WithParameter((pi, c) => pi.Name == "content",
                     (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_AccessoriesService>().As<ISws_AccessoriesService>().WithParameter((pi, c) => pi.Name == "content",
                     (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_WorkOrderService>().As<IWO_WorkOrderService>().WithParameter((pi, c) => pi.Name == "content",
                     (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_EventsService>().As<IWO_EventsService>().WithParameter((pi, c) => pi.Name == "content",
                     (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_ResourceService>().As<IWO_ResourceService>().WithParameter((pi, c) => pi.Name == "content",
                    (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_TeamInfoService>().As<IWO_TeamInfoService>().WithParameter((pi, c) => pi.Name == "content",
                    (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_TeamUserService>().As<IWO_TeamUserService>().WithParameter((pi, c) => pi.Name == "content",
                    (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_AreaInfoService>().As<IWO_AreaInfoService>().WithParameter((pi, c) => pi.Name == "content",
                (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_AssignmentPlanService>().As<IWO_AssignmentPlanService>().WithParameter((pi, c) => pi.Name == "content",
    (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_TemplateInfoService>().As<IWO_TemplateInfoService>().WithParameter((pi, c) => pi.Name == "content",
(pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_InspectPlanCheckService>().As<IWO_InspectPlanCheckService>().WithParameter((pi, c) => pi.Name == "content",
(pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_WOExtensionService>().As<IWO_WOExtensionService>().WithParameter((pi, c) => pi.Name == "content",
                  (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_InspectionPlanService>().As<IWO_InspectionPlanService>().WithParameter((pi, c) => pi.Name == "content",
                   (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_TemplateInfoService>().As<IWO_TemplateInfoService>().WithParameter((pi, c) => pi.Name == "content",
                 (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_FeedBackInfoService>().As<IWO_FeedBackInfoService>().WithParameter((pi, c) => pi.Name == "content",
               (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_TeamInfoService>().As<IWO_TeamInfoService>().WithParameter((pi, c) => pi.Name == "content",
                (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_TeamUserService>().As<IWO_TeamUserService>().WithParameter((pi, c) => pi.Name == "content",
                (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_PlanInspectOService>().As<IWO_PlanInspectOService>().WithParameter((pi, c) => pi.Name == "content",
              (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_ForwardService>().As<IWO_ForwardService>().WithParameter((pi, c) => pi.Name == "content",
              (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WarnRuleService>().As<IWarnRuleService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WarnRule_DetailService>().As<IWarnRule_DetailService>().WithParameter((pi, c) => pi.Name == "content",
                      (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sys_EarlyWarningPlanService>().As<ISys_EarlyWarningPlanService>().WithParameter((pi, c) => pi.Name == "content",
                   (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_ConsumpSettingService>().As<ISws_ConsumpSettingService>().WithParameter((pi, c) => pi.Name == "content",
                  (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_MaDeBaseInfoService>().As<ISws_MaDeBaseInfoService>().WithParameter((pi, c) => pi.Name == "content",
                  (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_InsExtensionService>().As<IWO_InsExtensionService>().WithParameter((pi, c) => pi.Name == "content",
             (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_InsForwardService>().As<IWO_InsForwardService>().WithParameter((pi, c) => pi.Name == "content",
             (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_AlarmReportsService>().As<ISws_AlarmReportsService>().WithParameter((pi, c) => pi.Name == "content",
                (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<WO_WOOperationService>().As<IWO_WOOperationService>().WithParameter((pi, c) => pi.Name == "content",
               (pi, c) => c.ResolveNamed<DbContext>("databaseA"));

            containerBuilder.RegisterType<Sws_UserPositionService>().As<ISws_UserPositionService>().WithParameter((pi, c) => pi.Name == "content",
              (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_GpsborrowingService>().As<ISws_GpsborrowingService>().WithParameter((pi, c) => pi.Name == "content",
              (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_GPSModuleService>().As<ISws_GPSModuleService>().WithParameter((pi, c) => pi.Name == "content",
             (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            //containerBuilder.Register<FirstController>();
            containerBuilder.RegisterType<Sws_GPSModuleService>().As<ISws_GPSModuleService>().WithParameter((pi, c) => pi.Name == "content", (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
            containerBuilder.RegisterType<Sws_EventSchemeService>().As<ISws_EventSchemeService>().WithParameter((pi, c) => pi.Name == "content", (pi, c) => c.ResolveNamed<DbContext>("databaseA"));
        }
    }
}
