using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WNMS.Utility;

namespace WNMS.Model.DataModels
{
    public partial class WNMSContext : DbContext
    {
        public WNMSContext()
        {
        }

        public WNMSContext(DbContextOptions<WNMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<DayQuartZ> DayQuartZ { get; set; }
        public virtual DbSet<DayQuartZ01> DayQuartZ01 { get; set; }
        public virtual DbSet<DayQuartZ02> DayQuartZ02 { get; set; }
        public virtual DbSet<DayQuartZ03> DayQuartZ03 { get; set; }
        public virtual DbSet<DdayQuartZ01> DdayQuartZ01 { get; set; }
        public virtual DbSet<DdayQuartZ02> DdayQuartZ02 { get; set; }
        public virtual DbSet<DdayQuartZ03> DdayQuartZ03 { get; set; }
        public virtual DbSet<DhourQuartZ01> DhourQuartZ01 { get; set; }
        public virtual DbSet<DhourQuartZ02> DhourQuartZ02 { get; set; }
        public virtual DbSet<DhourQuartZ03> DhourQuartZ03 { get; set; }
        public virtual DbSet<DmonthQuartZ01> DmonthQuartZ01 { get; set; }
        public virtual DbSet<DmonthQuartZ02> DmonthQuartZ02 { get; set; }
        public virtual DbSet<DmonthQuartZ03> DmonthQuartZ03 { get; set; }
        public virtual DbSet<GdEvents> GdEvents { get; set; }
        public virtual DbSet<GdInspection> GdInspection { get; set; }
        public virtual DbSet<GdMaintain> GdMaintain { get; set; }
        public virtual DbSet<GdRepair> GdRepair { get; set; }
        public virtual DbSet<GdResource> GdResource { get; set; }
        public virtual DbSet<GdTankCleaning> GdTankCleaning { get; set; }
        public virtual DbSet<GdTeamInfo> GdTeamInfo { get; set; }
        public virtual DbSet<GdTeamUser> GdTeamUser { get; set; }
        public virtual DbSet<GdWoextension> GdWoextension { get; set; }
        public virtual DbSet<GdWooperation> GdWooperation { get; set; }
        public virtual DbSet<GdWoreview> GdWoreview { get; set; }
        public virtual DbSet<GdWorkOrder> GdWorkOrder { get; set; }
        public virtual DbSet<HourQuartZ> HourQuartZ { get; set; }
        public virtual DbSet<HourQuartZ01> HourQuartZ01 { get; set; }
        public virtual DbSet<HourQuartZ02> HourQuartZ02 { get; set; }
        public virtual DbSet<HourQuartZ03> HourQuartZ03 { get; set; }
        public virtual DbSet<HourQuartZtest> HourQuartZtest { get; set; }
        public virtual DbSet<MonthQuartZ> MonthQuartZ { get; set; }
        public virtual DbSet<MonthQuartZ01> MonthQuartZ01 { get; set; }
        public virtual DbSet<MonthQuartZ02> MonthQuartZ02 { get; set; }
        public virtual DbSet<MonthQuartZ03> MonthQuartZ03 { get; set; }
        public virtual DbSet<StatisticQuartZ> StatisticQuartZ { get; set; }
        public virtual DbSet<SwsAcccessoriesChart> SwsAcccessoriesChart { get; set; }
        public virtual DbSet<SwsAccessAlarmType> SwsAccessAlarmType { get; set; }
        public virtual DbSet<SwsAccessControl> SwsAccessControl { get; set; }
        public virtual DbSet<SwsAccessHistory> SwsAccessHistory { get; set; }
        public virtual DbSet<SwsAccessories> SwsAccessories { get; set; }
        public virtual DbSet<SwsAccessoriesEqu> SwsAccessoriesEqu { get; set; }
        public virtual DbSet<SwsAccessoriesMaintenance> SwsAccessoriesMaintenance { get; set; }
        public virtual DbSet<SwsAccessoriesRtMaintenance> SwsAccessoriesRtMaintenance { get; set; }
        public virtual DbSet<SwsCamera> SwsCamera { get; set; }
        public virtual DbSet<SwsComTypes> SwsComTypes { get; set; }
        public virtual DbSet<SwsConsumpSetting> SwsConsumpSetting { get; set; }
        public virtual DbSet<SwsCutOffMessage> SwsCutOffMessage { get; set; }
        public virtual DbSet<SwsDataInfo> SwsDataInfo { get; set; }
        public virtual DbSet<SwsDataInfo1> SwsDataInfo1 { get; set; }
        public virtual DbSet<SwsDataInfo2> SwsDataInfo2 { get; set; }
        public virtual DbSet<SwsDataInfo222> SwsDataInfo222 { get; set; }
        public virtual DbSet<SwsDeviceInfo01> SwsDeviceInfo01 { get; set; }
        public virtual DbSet<SwsDeviceInfo02> SwsDeviceInfo02 { get; set; }
        public virtual DbSet<SwsDeviceInfo03> SwsDeviceInfo03 { get; set; }
        public virtual DbSet<SwsDeviceTemplate> SwsDeviceTemplate { get; set; }
        public virtual DbSet<SwsDpcinfo> SwsDpcinfo { get; set; }
        public virtual DbSet<SwsEventAttention> SwsEventAttention { get; set; }
        public virtual DbSet<SwsEventHandle> SwsEventHandle { get; set; }
        public virtual DbSet<SwsEventHistory> SwsEventHistory { get; set; }
        public virtual DbSet<SwsEventInfo> SwsEventInfo { get; set; }
        public virtual DbSet<SwsEventScheme> SwsEventScheme { get; set; }
        public virtual DbSet<SwsGpsborrowing> SwsGpsborrowing { get; set; }
        public virtual DbSet<SwsGpsmodule> SwsGpsmodule { get; set; }
        public virtual DbSet<SwsGuiinfo> SwsGuiinfo { get; set; }
        public virtual DbSet<SwsMaDeBaseInfo> SwsMaDeBaseInfo { get; set; }
        public virtual DbSet<SwsOpcsetting> SwsOpcsetting { get; set; }
        public virtual DbSet<SwsPropertyInfo> SwsPropertyInfo { get; set; }
        public virtual DbSet<SwsRtuinfo> SwsRtuinfo { get; set; }
        public virtual DbSet<SwsStation> SwsStation { get; set; }
        public virtual DbSet<SwsTemplate> SwsTemplate { get; set; }
        public virtual DbSet<SwsUserStation> SwsUserStation { get; set; }
        public virtual DbSet<SwsUserStation2> SwsUserStation2 { get; set; }
        public virtual DbSet<SwsValveWith01> SwsValveWith01 { get; set; }
        public virtual DbSet<SysArea> SysArea { get; set; }
        public virtual DbSet<SysDataItem> SysDataItem { get; set; }
        public virtual DbSet<SysDataItemDetail> SysDataItemDetail { get; set; }
        public virtual DbSet<SysDepartMent> SysDepartMent { get; set; }
        public virtual DbSet<SysEarlyWarningPlan> SysEarlyWarningPlan { get; set; }
        public virtual DbSet<SysLog> SysLog { get; set; }
        public virtual DbSet<SysModule> SysModule { get; set; }
        public virtual DbSet<SysModuleButton> SysModuleButton { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysRoleModule> SysRoleModule { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysUserModule> SysUserModule { get; set; }
        public virtual DbSet<SysUserRole> SysUserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ViewDeviceInfo> ViewDeviceInfo { get; set; }
        public virtual DbSet<WarnData> WarnData { get; set; }
        public virtual DbSet<WarnData1> WarnData1 { get; set; }
        public virtual DbSet<WarnRule> WarnRule { get; set; }
        public virtual DbSet<WarnRuleDetail> WarnRuleDetail { get; set; }
        public virtual DbSet<WoAreaInfo> WoAreaInfo { get; set; }
        public virtual DbSet<WoAreaRtu> WoAreaRtu { get; set; }
        public virtual DbSet<WoAssignmentPlan> WoAssignmentPlan { get; set; }
        public virtual DbSet<WoEvents> WoEvents { get; set; }
        public virtual DbSet<WoFbOfTemplate> WoFbOfTemplate { get; set; }
        public virtual DbSet<WoFbtemplate> WoFbtemplate { get; set; }
        public virtual DbSet<WoFeedBackInfo> WoFeedBackInfo { get; set; }
        public virtual DbSet<WoForward> WoForward { get; set; }
        public virtual DbSet<WoInsExtension> WoInsExtension { get; set; }
        public virtual DbSet<WoInsForward> WoInsForward { get; set; }
        public virtual DbSet<WoInspectPlanCheck> WoInspectPlanCheck { get; set; }
        public virtual DbSet<WoInspectionPlan> WoInspectionPlan { get; set; }
        public virtual DbSet<WoPlanInspectO> WoPlanInspectO { get; set; }
        public virtual DbSet<WoResource> WoResource { get; set; }
        public virtual DbSet<WoTeamInfo> WoTeamInfo { get; set; }
        public virtual DbSet<WoTeamUser> WoTeamUser { get; set; }
        public virtual DbSet<WoTemplateInfo> WoTemplateInfo { get; set; }
        public virtual DbSet<WoWoextension> WoWoextension { get; set; }
        public virtual DbSet<WoWooperation> WoWooperation { get; set; }
        public virtual DbSet<WoWorkOrder> WoWorkOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(StaticConstraint.WNMSConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId)
                    .HasColumnName("FileID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.Affiliation).HasComment("附件所属主表id");

                entity.Property(e => e.Classify).HasComment("分类（大类）");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("附件名");

                entity.Property(e => e.FileSize).HasComment("附件大小");

                entity.Property(e => e.FileType).HasComment("附件类型（图片还是文本等）");

                entity.Property(e => e.FileUnit)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("大小单位");

                entity.Property(e => e.FileUrl)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("附件路径");

                entity.Property(e => e.Suffix)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("后缀");

                entity.Property(e => e.UploadTime)
                    .HasColumnType("datetime")
                    .HasComment("上传时间");
            });

            modelBuilder.Entity<DayQuartZ>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClAver).HasColumnName("CL_Aver");

                entity.Property(e => e.ConductivityAver).HasColumnName("Conductivity_Aver");

                entity.Property(e => e.EnergyCon).HasComment("用电量");

                entity.Property(e => e.FlowCon).HasComment("用水量");

                entity.Property(e => e.OrpAver).HasColumnName("ORP_Aver");

                entity.Property(e => e.OxygenAver).HasColumnName("Oxygen_Aver");

                entity.Property(e => e.PhAver).HasColumnName("PH_Aver");

                entity.Property(e => e.SalinityAver).HasColumnName("Salinity_Aver");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TurbidityAver).HasColumnName("Turbidity_Aver");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DayQuartZ01>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DayQuartZ02>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DayQuartZ03>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DdayQuartZ01>(entity =>
            {
                entity.ToTable("DDayQuartZ01");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DdayQuartZ02>(entity =>
            {
                entity.ToTable("DDayQuartZ02");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClAver)
                    .HasColumnName("CL_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ConductivityAver)
                    .HasColumnName("Conductivity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HardnessAver)
                    .HasColumnName("Hardness_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrpAver)
                    .HasColumnName("ORP_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OxygenAver)
                    .HasColumnName("Oxygen_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PhAver)
                    .HasColumnName("PH_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalinityAver)
                    .HasColumnName("Salinity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TurbidityAver)
                    .HasColumnName("Turbidity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DdayQuartZ03>(entity =>
            {
                entity.ToTable("DDayQuartZ03");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DhourQuartZ01>(entity =>
            {
                entity.ToTable("DHourQuartZ01");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DhourQuartZ02>(entity =>
            {
                entity.ToTable("DHourQuartZ02");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClAver)
                    .HasColumnName("CL_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ConductivityAver)
                    .HasColumnName("Conductivity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HardnessAver)
                    .HasColumnName("Hardness_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrpAver)
                    .HasColumnName("ORP_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OxygenAver)
                    .HasColumnName("Oxygen_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PhAver)
                    .HasColumnName("PH_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalinityAver)
                    .HasColumnName("Salinity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TurbidityAver)
                    .HasColumnName("Turbidity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DhourQuartZ03>(entity =>
            {
                entity.ToTable("DHourQuartZ03");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DmonthQuartZ01>(entity =>
            {
                entity.ToTable("DMonthQuartZ01");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DmonthQuartZ02>(entity =>
            {
                entity.ToTable("DMonthQuartZ02");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClAver)
                    .HasColumnName("CL_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ConductivityAver)
                    .HasColumnName("Conductivity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HardnessAver)
                    .HasColumnName("Hardness_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrpAver)
                    .HasColumnName("ORP_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OxygenAver)
                    .HasColumnName("Oxygen_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PhAver)
                    .HasColumnName("PH_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalinityAver)
                    .HasColumnName("Salinity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TurbidityAver)
                    .HasColumnName("Turbidity_Aver")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DmonthQuartZ03>(entity =>
            {
                entity.ToTable("DMonthQuartZ03");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EnergyCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FlowCon).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GdEvents>(entity =>
            {
                entity.HasKey(e => e.IncidentId);

                entity.ToTable("GD_Events");

                entity.Property(e => e.IncidentId)
                    .HasColumnName("IncidentID")
                    .HasComment("事件主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditContent)
                    .HasMaxLength(500)
                    .HasComment("审核内容");

                entity.Property(e => e.AuditState).HasComment("是否审核 0：未审核   1：已审核");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasComment("事件说明");

                entity.Property(e => e.DisposeState).HasComment("实时处理状态（1未处理，2处理中，3处理完成）");

                entity.Property(e => e.IncidentContent)
                    .HasMaxLength(500)
                    .HasComment("事件内容");

                entity.Property(e => e.IncidentNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("事件编号");

                entity.Property(e => e.IncidentSource).HasComment("事件来源   0：移动端    1：pc 端 2:报警监控");

                entity.Property(e => e.IncidentState).HasComment("事件状态（0未处理，1已派发，2无效）");

                entity.Property(e => e.IncidentType).HasComment("事件类型   0：巡检    1：保养    2：维修");

                entity.Property(e => e.Picture)
                    .HasMaxLength(300)
                    .HasComment("现场图片");

                entity.Property(e => e.Recording)
                    .HasMaxLength(300)
                    .HasComment("现场录音");

                entity.Property(e => e.ReportTime)
                    .HasColumnType("datetime")
                    .HasComment("上报时间 取各自事件的 CreateTime");

                entity.Property(e => e.ReportUser).HasComment("上报人");

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasComment("任务ID 根据事件类型 到各自的表里查找关联ID");
            });

            modelBuilder.Entity<GdInspection>(entity =>
            {
                entity.HasKey(e => e.InspectionId);

                entity.ToTable("GD_Inspection");

                entity.Property(e => e.InspectionId)
                    .HasColumnName("InspectionID")
                    .HasComment("主键 ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ControlCabinet).HasComment("控制柜状态");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.ElectricQuantity).HasComment("电量状态");

                entity.Property(e => e.Electricity).HasComment("电流状态");

                entity.Property(e => e.FeedbackMsg)
                    .HasMaxLength(500)
                    .HasComment("反馈信息");

                entity.Property(e => e.Frequency).HasComment("频率状态");

                entity.Property(e => e.Health).HasComment("卫生状态");

                entity.Property(e => e.InspectionTime)
                    .HasColumnType("datetime")
                    .HasComment("巡检时间，表单的提交时间");

                entity.Property(e => e.InspectionUser).HasComment("巡检人员");

                entity.Property(e => e.IsFeedback).HasComment("是否反馈，区分移动端是否提交   0：未反馈    1：已反馈");

                entity.Property(e => e.LiquidLevel).HasComment("液位状态");

                entity.Property(e => e.Noise).HasComment("噪音状态");

                entity.Property(e => e.Num)
                    .HasMaxLength(50)
                    .HasComment("巡检编号 XJ_20200415_1015");

                entity.Property(e => e.Penetration).HasComment("渗透状态");

                entity.Property(e => e.PipeState).HasComment("管道状态");

                entity.Property(e => e.Pressure).HasComment("进出设压力状态");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TaskDescription)
                    .HasMaxLength(500)
                    .HasComment("任务描述");

                entity.Property(e => e.Temperature).HasComment("温度状态");

                entity.Property(e => e.ValveParts).HasComment("阀件状态");

                entity.Property(e => e.Voltage).HasComment("电压状态");
            });

            modelBuilder.Entity<GdMaintain>(entity =>
            {
                entity.HasKey(e => e.MaintainId);

                entity.ToTable("GD_Maintain");

                entity.Property(e => e.MaintainId)
                    .HasColumnName("MaintainID")
                    .HasComment("主键编号")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.FeedbackMsg)
                    .HasMaxLength(500)
                    .HasComment("反馈说明");

                entity.Property(e => e.IsFeedback).HasComment("0：未反馈     1：已反馈");

                entity.Property(e => e.MaintainState).HasComment("保养状态");

                entity.Property(e => e.MaintainTime)
                    .HasColumnType("datetime")
                    .HasComment("保养时间  表单提交时间");

                entity.Property(e => e.MaintainUser).HasComment("保养人员");

                entity.Property(e => e.Num)
                    .HasMaxLength(50)
                    .HasComment("保养编号 BY_20200415_1055");

                entity.Property(e => e.Project).HasComment("保养项目");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TaskDescription)
                    .HasMaxLength(500)
                    .HasComment("任务说明");
            });

            modelBuilder.Entity<GdRepair>(entity =>
            {
                entity.HasKey(e => e.RepairId);

                entity.ToTable("GD_Repair");

                entity.Property(e => e.RepairId)
                    .HasColumnName("RepairID")
                    .HasComment("主键ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.FaultContent)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FaultDescription).HasMaxLength(500);

                entity.Property(e => e.Num)
                    .HasMaxLength(50)
                    .HasComment("维修编号 WX_20200415_1113");

                entity.Property(e => e.RepairDescription).HasMaxLength(500);

                entity.Property(e => e.ReportTime)
                    .HasColumnType("datetime")
                    .HasComment("报障时间");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<GdResource>(entity =>
            {
                entity.ToTable("GD_Resource");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .HasComment("nvarchar(200)");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("路径");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("所属 ID");

                entity.Property(e => e.ResourceType).HasComment("所属类型（1.维修任务，2.保养任务，3.维修反馈，4，保养反馈，5.事件，6.工单）枚举");

                entity.Property(e => e.Suffix)
                    .HasMaxLength(50)
                    .HasComment("上传文件的后缀名");

                entity.Property(e => e.Type).HasComment("类型（1图片，2音频，3视频）");
            });

            modelBuilder.Entity<GdTankCleaning>(entity =>
            {
                entity.HasKey(e => e.CleaningId);

                entity.ToTable("GD_TankCleaning");

                entity.Property(e => e.CleaningId)
                    .HasColumnName("CleaningID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Num).HasMaxLength(50);
            });

            modelBuilder.Entity<GdTeamInfo>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.ToTable("GD_TeamInfo");

                entity.Property(e => e.TeamId)
                    .HasColumnName("TeamID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GdTeamUser>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId });

                entity.ToTable("GD_TeamUser");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<GdWoextension>(entity =>
            {
                entity.ToTable("GD_WOExtension");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("主键 ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingTime)
                    .HasColumnType("datetime")
                    .HasComment("审核时间");

                entity.Property(e => e.Auditor).HasComment("审核人");

                entity.Property(e => e.CompleteTime)
                    .HasColumnType("datetime")
                    .HasComment("申请完成时间");

                entity.Property(e => e.ExtensionTime)
                    .HasColumnType("datetime")
                    .HasComment("申请时间");

                entity.Property(e => e.OldCompleteTime)
                    .HasColumnType("datetime")
                    .HasComment("申请之前的完成时间");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("申请原因");

                entity.Property(e => e.State).HasComment("审核状态");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("申请人");

                entity.Property(e => e.Woid)
                    .HasColumnName("WOID")
                    .HasComment("工单 ID");
            });

            modelBuilder.Entity<GdWooperation>(entity =>
            {
                entity.ToTable("GD_WOOperation");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasComment("描述");

                entity.Property(e => e.OperationTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("所属操作ID");

                entity.Property(e => e.State).HasComment("操作结果（审核是否通过等）");

                entity.Property(e => e.Type).HasComment("操作类型");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<GdWoreview>(entity =>
            {
                entity.ToTable("GD_WOReview");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("申请ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingTime)
                    .HasColumnType("datetime")
                    .HasComment("审核时间");

                entity.Property(e => e.Auditor).HasComment("审核人");

                entity.Property(e => e.CompleteTime)
                    .HasColumnType("datetime")
                    .HasComment("完成时间");

                entity.Property(e => e.ExtensionTime)
                    .HasColumnType("datetime")
                    .HasComment("申请时间");

                entity.Property(e => e.RecipientId)
                    .HasColumnName("RecipientID")
                    .HasComment("接收人");

                entity.Property(e => e.Remake)
                    .HasMaxLength(500)
                    .HasComment("备注");

                entity.Property(e => e.State).HasComment("状态（1.审核通过2.不通过3.未审核）");

                entity.Property(e => e.Type).HasComment("申请类型（1.退单、2.移交）");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("申请人");

                entity.Property(e => e.Woid)
                    .HasColumnName("WOID")
                    .HasComment("工单 ID");
            });

            modelBuilder.Entity<GdWorkOrder>(entity =>
            {
                entity.HasKey(e => e.Woid);

                entity.ToTable("GD_WorkOrder");

                entity.Property(e => e.Woid)
                    .HasColumnName("WOID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingContent).HasComment("审核意见");

                entity.Property(e => e.CompleteTime)
                    .HasColumnType("datetime")
                    .HasComment("完成时间");

                entity.Property(e => e.CurrentState).HasComment("当前状态");

                entity.Property(e => e.Degree).HasComment("紧急程度");

                entity.Property(e => e.EventId)
                    .HasColumnName("EventID")
                    .HasComment("事件ID");

                entity.Property(e => e.HandleLevel).HasComment("处理级别");

                entity.Property(e => e.IsAuditing).HasComment("审核状态（1审核通过，2审核未通过，3未审核）");

                entity.Property(e => e.Num).HasMaxLength(50);

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("父工单ID   工单移交所用的字段");

                entity.Property(e => e.ReleaseTime)
                    .HasColumnType("datetime")
                    .HasComment("发布时间");

                entity.Property(e => e.ReleaseUser).HasComment("派发人");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("接收人");
            });

            modelBuilder.Entity<HourQuartZ>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClAver).HasColumnName("CL_Aver");

                entity.Property(e => e.ConductivityAver).HasColumnName("Conductivity_Aver");

                entity.Property(e => e.FlowCon).HasComment("用水量");

                entity.Property(e => e.OrpAver).HasColumnName("ORP_Aver");

                entity.Property(e => e.OxygenAver).HasColumnName("Oxygen_Aver");

                entity.Property(e => e.PhAver).HasColumnName("PH_Aver");

                entity.Property(e => e.SalinityAver).HasColumnName("Salinity_Aver");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TurbidityAver).HasColumnName("Turbidity_Aver");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<HourQuartZ01>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<HourQuartZ02>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<HourQuartZ03>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<HourQuartZtest>(entity =>
            {
                entity.ToTable("HourQuartZTest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MonthQuartZ>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClAver).HasColumnName("CL_Aver");

                entity.Property(e => e.ConductivityAver).HasColumnName("Conductivity_Aver");

                entity.Property(e => e.FlowCon).HasComment("用水量");

                entity.Property(e => e.OrpAver).HasColumnName("ORP_Aver");

                entity.Property(e => e.OxygenAver).HasColumnName("Oxygen_Aver");

                entity.Property(e => e.PhAver).HasColumnName("PH_Aver");

                entity.Property(e => e.SalinityAver).HasColumnName("Salinity_Aver");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TurbidityAver).HasColumnName("Turbidity_Aver");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MonthQuartZ01>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MonthQuartZ02>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MonthQuartZ03>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataKey).HasMaxLength(50);

                entity.Property(e => e.DataValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<StatisticQuartZ>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataId)
                    .IsRequired()
                    .HasColumnName("DataID")
                    .HasMaxLength(50);

                entity.Property(e => e.DataKey)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DataName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SwsAcccessoriesChart>(entity =>
            {
                entity.HasKey(e => e.AccessoriesId);

                entity.ToTable("Sws_AcccessoriesChart");

                entity.Property(e => e.AccessoriesId)
                    .HasColumnName("AccessoriesID")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<SwsAccessAlarmType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sws_AccessAlarmType");

                entity.Property(e => e.AlarmName).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            modelBuilder.Entity<SwsAccessControl>(entity =>
            {
                entity.HasKey(e => e.DoorId)
                    .HasName("PK_Sws_DoorInfo");

                entity.ToTable("Sws_AccessControl");

                entity.Property(e => e.DoorId)
                    .HasColumnName("DoorID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("门禁名称");

                entity.Property(e => e.AppKey).HasMaxLength(100);

                entity.Property(e => e.Brand).HasComment("品牌");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("IP")
                    .HasMaxLength(20)
                    .HasComment("IP");

                entity.Property(e => e.Num)
                    .HasMaxLength(30)
                    .HasComment("门禁编号");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("门禁密码");

                entity.Property(e => e.Port).HasComment("端口");

                entity.Property(e => e.Secret).HasMaxLength(200);

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("门禁帐号");
            });

            modelBuilder.Entity<SwsAccessHistory>(entity =>
            {
                entity.ToTable("Sws_AccessHistory");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("主键");

                entity.Property(e => e.DoorId)
                    .HasColumnName("DoorID")
                    .HasComment("AccessControl 对应主键");

                entity.Property(e => e.InOutWay)
                    .HasMaxLength(50)
                    .HasComment("进出");

                entity.Property(e => e.Information)
                    .HasMaxLength(50)
                    .HasComment("报警说明");

                entity.Property(e => e.OperatingUser)
                    .HasMaxLength(50)
                    .HasComment("卡号");

                entity.Property(e => e.PoliceTime)
                    .HasColumnType("datetime")
                    .HasComment("报警时间");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasComment("用户名");
            });

            modelBuilder.Entity<SwsAccessories>(entity =>
            {
                entity.ToTable("Sws_Accessories");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(20)
                    .HasComment("器件编号（主键）");

                entity.Property(e => e.Inventory).HasComment("库存量");

                entity.Property(e => e.IsConsumable).HasComment("是否是易消耗品（1是易消耗品）");

                entity.Property(e => e.MaintenancePeriod).HasComment("保养周期");

                entity.Property(e => e.Manufacturer)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Material)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("材质");

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("器件名称");

                entity.Property(e => e.Place).HasMaxLength(128);

                entity.Property(e => e.ReplacementCycle).HasComment("更换周期");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("器件型号");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("计量单位");
            });

            modelBuilder.Entity<SwsAccessoriesEqu>(entity =>
            {
                entity.ToTable("Sws_AccessoriesEqu");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessoriesId)
                    .IsRequired()
                    .HasColumnName("AccessoriesID")
                    .HasMaxLength(20)
                    .HasComment("器件 ID");

                entity.Property(e => e.AccessoriesNo)
                    .HasMaxLength(20)
                    .HasComment("具体器件编号");

                entity.Property(e => e.CommissioningDate)
                    .HasColumnType("datetime")
                    .HasComment("调试日期");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("更换费用");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime")
                    .HasComment("交付日期");

                entity.Property(e => e.ElectricalDate)
                    .HasColumnType("datetime")
                    .HasComment("电气安装日期");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasComment("结束日期");

                entity.Property(e => e.EquType).HasComment("1代表供水设备，2代表直饮水设备");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EquipmentID")
                    .HasComment("设备 ID");

                entity.Property(e => e.GuaranteePeriod)
                    .HasColumnType("datetime")
                    .HasComment("质保期");

                entity.Property(e => e.MaintenanceDate)
                    .HasColumnType("datetime")
                    .HasComment("上一次保养时间");

                entity.Property(e => e.MchanicalDate)
                    .HasColumnType("datetime")
                    .HasComment("机械安装日期");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasComment("数量");

                entity.Property(e => e.Reason)
                    .HasMaxLength(200)
                    .HasComment("更换原因");
            });

            modelBuilder.Entity<SwsAccessoriesMaintenance>(entity =>
            {
                entity.ToTable("Sws_AccessoriesMaintenance");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("自增 ID");

                entity.Property(e => e.AccessoriesDetailId)
                    .HasColumnName("AccessoriesDetailID")
                    .HasComment("器件具体 ID");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("保养费用");

                entity.Property(e => e.MaintenanceContent)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("保养内容");

                entity.Property(e => e.MaintenanceDate)
                    .HasColumnType("datetime")
                    .HasComment("保养日期");

                entity.Property(e => e.MaintenanceUser)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("保养人");
            });

            modelBuilder.Entity<SwsAccessoriesRtMaintenance>(entity =>
            {
                entity.HasKey(e => e.AccessoriesId);

                entity.ToTable("Sws_AccessoriesRtMaintenance");

                entity.Property(e => e.AccessoriesId)
                    .HasColumnName("AccessoriesID")
                    .ValueGeneratedNever();

                entity.Property(e => e.MaintenanceDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SwsCamera>(entity =>
            {
                entity.HasKey(e => e.CameraId);

                entity.ToTable("Sws_Camera");

                entity.Property(e => e.CameraId)
                    .HasColumnName("CameraID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.AppKey)
                    .HasMaxLength(100)
                    .HasComment("开发者应用内 AppKey");

                entity.Property(e => e.AppSecret)
                    .HasMaxLength(100)
                    .HasComment("开发者应用内 AppSecret");

                entity.Property(e => e.CameraName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("镜头名称");

                entity.Property(e => e.CameraType).HasComment("品牌");

                entity.Property(e => e.ChannelNum).HasComment("对应门禁的通道号");

                entity.Property(e => e.DoorId)
                    .HasColumnName("DoorID")
                    .HasComment("镜头对应的门禁");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(20)
                    .HasComment("IP");

                entity.Property(e => e.LimitTime).HasComment("Token 超期时间");

                entity.Property(e => e.Numbering)
                    .HasMaxLength(50)
                    .HasComment("Dss 相关");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .HasComment("密码");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasMaxLength(50)
                    .HasComment("DSS 相关");

                entity.Property(e => e.Port).HasComment("端口");

                entity.Property(e => e.SerialNum)
                    .HasMaxLength(50)
                    .HasComment("序列号");

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasComment("泵房ID");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .HasComment("开发者应用内 Token");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasComment("帐号");
            });

            modelBuilder.Entity<SwsComTypes>(entity =>
            {
                entity.ToTable("Sws_ComTypes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cnname)
                    .HasColumnName("CNName")
                    .HasMaxLength(50);

                entity.Property(e => e.ComTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SwsConsumpSetting>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.DeviceId, e.Type });

                entity.ToTable("Sws_ConsumpSetting");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.Type).HasComment("1：单吨能耗 ，2吨水能耗   3：泵房能耗,4:曲线汇总的设备配置,5:能耗对比的设备配置");
            });

            modelBuilder.Entity<SwsCutOffMessage>(entity =>
            {
                entity.ToTable("Sws_CutOffMessage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CutOffReason).HasMaxLength(50);

                entity.Property(e => e.CutOffTime).HasMaxLength(50);

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.SupplyTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SwsDataInfo>(entity =>
            {
                entity.ToTable("Sws_DataInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cnname)
                    .IsRequired()
                    .HasColumnName("CNName")
                    .HasMaxLength(20)
                    .HasComment("中文名称");

                entity.Property(e => e.DataId)
                    .HasColumnName("DataID")
                    .HasComment("key");

                entity.Property(e => e.DataRatio).HasComment("倍率");

                entity.Property(e => e.DeviceType).HasComment("设备分类 1：供水设备   2：直饮水    3：电机");

                entity.Property(e => e.Enname)
                    .IsRequired()
                    .HasColumnName("ENName")
                    .HasMaxLength(20)
                    .HasComment("英文名称");

                entity.Property(e => e.IsCumulation)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否为累计量");

                entity.Property(e => e.IsShow).HasDefaultValueSql("((1))");

                entity.Property(e => e.Region)
                    .HasDefaultValueSql("((0))")
                    .HasComment("1低区；2中区；3高区；4超高区；5超超高区；6智慧泵房；0共用");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasComment("单位");
            });

            modelBuilder.Entity<SwsDataInfo1>(entity =>
            {
                entity.ToTable("Sws_DataInfo1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cnname)
                    .IsRequired()
                    .HasColumnName("CNName")
                    .HasMaxLength(20);

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.Enname)
                    .IsRequired()
                    .HasColumnName("ENName")
                    .HasMaxLength(20);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<SwsDataInfo2>(entity =>
            {
                entity.ToTable("Sws_DataInfo2");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cnname)
                    .IsRequired()
                    .HasColumnName("CNName")
                    .HasMaxLength(20);

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.Enname)
                    .IsRequired()
                    .HasColumnName("ENName")
                    .HasMaxLength(20);

                entity.Property(e => e.IsCumulation).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsShow).HasDefaultValueSql("((1))");

                entity.Property(e => e.Region).HasDefaultValueSql("((0))");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<SwsDataInfo222>(entity =>
            {
                entity.ToTable("Sws_DataInfo222");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cnname)
                    .IsRequired()
                    .HasColumnName("CNName")
                    .HasMaxLength(20);

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.Enname)
                    .IsRequired()
                    .HasColumnName("ENName")
                    .HasMaxLength(20);

                entity.Property(e => e.IsCumulation).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsShow).HasDefaultValueSql("((1))");

                entity.Property(e => e.Region).HasDefaultValueSql("((0))");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<SwsDeviceInfo01>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.ToTable("Sws_DeviceInfo01");

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(50)
                    .HasComment("设备名称");

                entity.Property(e => e.DeviceNum)
                    .HasMaxLength(50)
                    .HasComment("设备编号");

                entity.Property(e => e.DeviceType).HasComment("设备类型");

                entity.Property(e => e.ExportDn)
                    .HasColumnName("ExportDN")
                    .HasMaxLength(50)
                    .HasComment("出口 DN");

                entity.Property(e => e.Frequency).HasComment("变频分类");

                entity.Property(e => e.Gui)
                    .HasColumnName("GUI")
                    .HasComment("工艺图");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(200)
                    .HasComment("图片地址");

                entity.Property(e => e.ImportDn)
                    .HasColumnName("ImportDN")
                    .HasMaxLength(50)
                    .HasComment("进口 DN");

                entity.Property(e => e.ManufactureDate)
                    .HasColumnType("datetime")
                    .HasComment("出厂日期");

                entity.Property(e => e.Manufacturer).HasComment("厂商");

                entity.Property(e => e.Partition).HasComment("分区   1：低区   2：中区   3：高区   4：超高区  5：超超高区");

                entity.Property(e => e.PumpNum).HasComment("泵数量");

                entity.Property(e => e.PumpType)
                    .HasMaxLength(50)
                    .HasComment("泵类型");

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasComment("泵房ID");
            });

            modelBuilder.Entity<SwsDeviceInfo02>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.ToTable("Sws_DeviceInfo02");

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(50)
                    .HasComment("设备名称");

                entity.Property(e => e.DeviceNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("设备编号");

                entity.Property(e => e.DeviceType).HasComment("设备类型");

                entity.Property(e => e.Gui).HasColumnName("GUI");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(200);

                entity.Property(e => e.IsSingle).HasComment("是否为单变频（是  一拖二，否 二拖二）");

                entity.Property(e => e.Partition).HasComment("分区   1：低区   2：中区   3：高区   4：超高区  5：超超高区");

                entity.Property(e => e.ProductionDate)
                    .HasColumnType("datetime")
                    .HasComment("生产日期");

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<SwsDeviceInfo03>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.ToTable("Sws_DeviceInfo03");

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(50)
                    .HasComment("设备名称");

                entity.Property(e => e.DeviceNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("设备编号");

                entity.Property(e => e.Partition).HasComment("分区   1：低区   2：中区   3：高区   4：超高区  5：超超高区");

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<SwsDeviceTemplate>(entity =>
            {
                entity.HasKey(e => new { e.DeviceId, e.TemplateId, e.DeviceType });

                entity.ToTable("Sws_DeviceTemplate");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            });

            modelBuilder.Entity<SwsDpcinfo>(entity =>
            {
                entity.HasKey(e => e.Dpcname);

                entity.ToTable("Sws_DPCInfo");

                entity.Property(e => e.Dpcname)
                    .HasColumnName("DPCName")
                    .HasMaxLength(50);

                entity.Property(e => e.PluginFile)
                    .IsRequired()
                    .HasColumnType("image");
            });

            modelBuilder.Entity<SwsEventAttention>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.DataId });

                entity.ToTable("Sws_EventAttention");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DataId).HasColumnName("DataID");
            });

            modelBuilder.Entity<SwsEventHandle>(entity =>
            {
                entity.ToTable("Sws_EventHandle");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventTime).HasColumnType("datetime");

                entity.Property(e => e.FeedBackInfo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<SwsEventHistory>(entity =>
            {
                entity.ToTable("Sws_EventHistory");

                entity.HasIndex(e => e.EventTime)
                    .HasName("NonClusteredIndex-20210913-134544");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CurrentValue).HasDefaultValueSql("((-9999))");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComputedColumnSql("(CONVERT([char](10),[EventTime],(21)))");

                entity.Property(e => e.EventMessage)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EventMonth).HasComputedColumnSql("(datepart(month,[EventTime]))");

                entity.Property(e => e.EventTime).HasColumnType("datetime");

                entity.Property(e => e.EventYear).HasComputedColumnSql("(datepart(year,[EventTime]))");

                entity.Property(e => e.LimitValue).HasDefaultValueSql("((-9999))");

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");
            });

            modelBuilder.Entity<SwsEventInfo>(entity =>
            {
                entity.ToTable("Sws_EventInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CurrentValue).HasDefaultValueSql("((-9999))");

                entity.Property(e => e.EventLevel).HasComment("报警等级");

                entity.Property(e => e.EventMessage)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("报警信息");

                entity.Property(e => e.EventSource).HasComment("报警源 DataID");

                entity.Property(e => e.EventTime)
                    .HasColumnType("datetime")
                    .HasComment("报警时间");

                entity.Property(e => e.EventType).HasComment("报警类型  0：非阈值报警；1：上限报警；2：上上限报警；3：下限报警；4：下下限报警");

                entity.Property(e => e.LimitValue).HasDefaultValueSql("((-9999))");

                entity.Property(e => e.Rtuid)
                    .HasColumnName("RTUID")
                    .HasComment("设备 ID");
            });

            modelBuilder.Entity<SwsEventScheme>(entity =>
            {
                entity.HasKey(e => new { e.Rtuid, e.DataId });

                entity.ToTable("Sws_EventScheme");

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.SchemeName).HasMaxLength(50);
            });

            modelBuilder.Entity<SwsGpsborrowing>(entity =>
            {
                entity.ToTable("Sws_Gpsborrowing");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BorrowTime).HasColumnType("datetime");

                entity.Property(e => e.RemandTime).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<SwsGpsmodule>(entity =>
            {
                entity.HasKey(e => e.Gpsid);

                entity.ToTable("Sws_GPSModule");

                entity.Property(e => e.Gpsid)
                    .HasColumnName("GPSID")
                    .ValueGeneratedNever();

                entity.Property(e => e.GpSnumber)
                    .IsRequired()
                    .HasColumnName("GpSNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.GpsName).HasMaxLength(50);

                entity.Property(e => e.Manufacture).HasMaxLength(50);

                entity.Property(e => e.ModelNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<SwsGuiinfo>(entity =>
            {
                entity.HasKey(e => e.Num);

                entity.ToTable("Sws_GUIInfo");

                entity.Property(e => e.Num)
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeviceType).HasComment("设备分类  1供水设备   2直饮水   3电机");

                entity.Property(e => e.EquipType).HasComment("DeviceType 下对应的二级分类");

                entity.Property(e => e.Guiname)
                    .IsRequired()
                    .HasColumnName("GUIName")
                    .HasMaxLength(50)
                    .HasComment("工艺图名称");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasColumnName("ImageURL")
                    .HasMaxLength(150)
                    .HasComment("图片地址");

                entity.Property(e => e.IsSum).HasComment("是否是总的");

                entity.Property(e => e.PageUrl)
                    .IsRequired()
                    .HasColumnName("PageURL")
                    .HasMaxLength(150)
                    .HasComment("对应的网页地址");

                entity.Property(e => e.PumpNum).HasComment("泵数量（方便检索）");
            });

            modelBuilder.Entity<SwsMaDeBaseInfo>(entity =>
            {
                entity.ToTable("Sws_MaDeBaseInfo");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<SwsOpcsetting>(entity =>
            {
                entity.HasKey(e => e.Rtuid)
                    .HasName("PK_OPCSetting");

                entity.ToTable("Sws_OPCSetting");

                entity.Property(e => e.Rtuid)
                    .HasColumnName("RTUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Opcaddress)
                    .HasColumnName("OPCAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.Opcprefix)
                    .HasColumnName("OPCPrefix")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SwsPropertyInfo>(entity =>
            {
                entity.HasKey(e => e.PropertyId);

                entity.ToTable("Sws_PropertyInfo");

                entity.Property(e => e.PropertyId)
                    .HasColumnName("PropertyID")
                    .HasComment("资产id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .HasComment("品牌");

                entity.Property(e => e.BuyDate)
                    .HasColumnType("datetime")
                    .HasComment("购买日期");

                entity.Property(e => e.BuyMonery).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Custodian)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("保管人");

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(50)
                    .HasComment("厂商");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("资产名称");

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Size)
                    .HasMaxLength(50)
                    .HasComment("规格");

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasComment("所属泵房");

                entity.Property(e => e.StorePosition)
                    .HasMaxLength(50)
                    .HasComment("存放位置");

                entity.Property(e => e.Type).HasComment("资产类型");

                entity.Property(e => e.WarrantyPeriod)
                    .HasMaxLength(50)
                    .HasComment("质保期");
            });

            modelBuilder.Entity<SwsRtuinfo>(entity =>
            {
                entity.HasKey(e => e.Rtuid)
                    .HasName("PK_RTUInfo");

                entity.ToTable("Sws_RTUInfo");

                entity.Property(e => e.Rtuid)
                    .HasColumnName("RTUID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivelySent).HasComment("主动发送");

                entity.Property(e => e.ComAddress).HasComment("通讯地址");

                entity.Property(e => e.ComType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("通讯类型");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("DeviceID")
                    .HasMaxLength(30)
                    .HasComment("通讯编号");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("控制器类型");

                entity.Property(e => e.Ipport)
                    .HasColumnName("IPPort")
                    .HasComment("端口");

                entity.Property(e => e.PluginFile).HasColumnType("image");

                entity.Property(e => e.Priority)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("优先级");

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasComment("泵房ID");
            });

            modelBuilder.Entity<SwsStation>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.ToTable("Sws_Station");

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasComment("泵房主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcceptanceDate)
                    .HasColumnType("datetime")
                    .HasComment("验收日期");

                entity.Property(e => e.CameraMonitor).HasComment("视频接入");

                entity.Property(e => e.CleaningCycle).HasComment("水箱清洗周期");

                entity.Property(e => e.ControlMonitor).HasComment("控制接入");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("录入时间");

                entity.Property(e => e.DoorId).HasColumnName("DoorID");

                entity.Property(e => e.Gui3dnum).HasColumnName("GUI3DNum");

                entity.Property(e => e.InType).HasComment("内置类型  0泵房无属性    1供水泵房    2直饮水泵房   3电机泵房");

                entity.Property(e => e.InspectionCycle).HasComment("巡检周期");

                entity.Property(e => e.InstallationDate)
                    .HasColumnType("datetime")
                    .HasComment("安装日期");

                entity.Property(e => e.InstallationPosition).HasComment("安装位置");

                entity.Property(e => e.Lat).HasComment("纬度");

                entity.Property(e => e.Lng).HasComment("经度");

                entity.Property(e => e.MaintenanceCycle).HasComment("保养周期");

                entity.Property(e => e.Manager).HasComment("负责人");

                entity.Property(e => e.ManagerPhone)
                    .HasMaxLength(20)
                    .HasComment("负责人电话");

                entity.Property(e => e.OccupancyRate).HasComment("入住率");

                entity.Property(e => e.QualityEndDate)
                    .HasColumnType("datetime")
                    .HasComment("质保结束日期");

                entity.Property(e => e.Remark)
                    .HasMaxLength(300)
                    .HasComment("泵房描述");

                entity.Property(e => e.StaitonType).HasComment("泵房类型");

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("名称");

                entity.Property(e => e.StationNum)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("编号");

                entity.Property(e => e.SwitchingCycle).HasComment("换药周期");

                entity.Property(e => e.WaterDisinfection).HasComment("水质消毒仪器");

                entity.Property(e => e.WaterQualityMonitor).HasComment("水质是否监控");

                entity.Property(e => e.WaterTankNum).HasComment("水箱个数");

                entity.Property(e => e.WaterTankPublic).HasComment("水箱是否公用");
            });

            modelBuilder.Entity<SwsTemplate>(entity =>
            {
                entity.ToTable("Sws_Template");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("主键ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Classify)
                    .HasColumnName("classify")
                    .HasComment("类型");

                entity.Property(e => e.DataId)
                    .HasColumnName("DataID")
                    .HasComment("模板dataid");

                entity.Property(e => e.DeviceType).HasComment("设备类型");

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("模板名称");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("用户id");
            });

            modelBuilder.Entity<SwsUserStation>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.StationId });

                entity.ToTable("Sws_UserStation");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("用户 ID");

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasComment("泵房 ID");

                entity.Property(e => e.FocusOn).HasComment("是否关注");
            });

            modelBuilder.Entity<SwsUserStation2>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.StationId });

                entity.ToTable("Sws_UserStation2");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<SwsValveWith01>(entity =>
            {
                entity.HasKey(e => e.ValveId);

                entity.ToTable("Sws_ValveWith01");

                entity.Property(e => e.ValveId)
                    .HasColumnName("ValveID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .HasComment("供水设备 ID");

                entity.Property(e => e.IsAdjusted).HasComment("开度是否可调");

                entity.Property(e => e.IsRemote).HasComment("是否可以远程");

                entity.Property(e => e.ValveName)
                    .HasMaxLength(50)
                    .HasComment("电动阀名称");

                entity.Property(e => e.ValveNum).HasComment("电动阀编号  1-5");
            });

            modelBuilder.Entity<SysArea>(entity =>
            {
                entity.ToTable("Sys_Area");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysDataItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.ToTable("Sys_DataItem");

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("字典名称");

                entity.Property(e => e.ItemValue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("字典对应值");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("父级");

                entity.Property(e => e.Reamrk)
                    .HasMaxLength(200)
                    .HasComment("备注");

                entity.Property(e => e.Sort).HasComment("排序");
            });

            modelBuilder.Entity<SysDataItemDetail>(entity =>
            {
                entity.HasKey(e => e.ItemDetailId);

                entity.ToTable("Sys_DataItemDetail");

                entity.Property(e => e.ItemDetailId)
                    .HasColumnName("ItemDetailID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.FItemId)
                    .HasColumnName("F_ItemId")
                    .HasComment("字典分类的ID");

                entity.Property(e => e.IsEnable).HasComment("是否启用");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("具体项的名称");

                entity.Property(e => e.ItemValue)
                    .HasMaxLength(20)
                    .HasComment("具体项的值");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.Sort).HasComment("排序");
            });

            modelBuilder.Entity<SysDepartMent>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.ToTable("Sys_DepartMent");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("DepartmentID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.DptName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("部门名称");

                entity.Property(e => e.Email)
                    .HasMaxLength(20)
                    .HasComment("邮箱");

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .HasComment("传真");

                entity.Property(e => e.InnerPhone)
                    .HasMaxLength(20)
                    .HasComment("对内电话");

                entity.Property(e => e.Manager)
                    .HasMaxLength(10)
                    .HasComment("管理人");

                entity.Property(e => e.OuterPhone)
                    .HasMaxLength(20)
                    .HasComment("对外电话");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("父级");

                entity.Property(e => e.Reamrk)
                    .HasMaxLength(300)
                    .HasComment("备注");

                entity.Property(e => e.Sort).HasComment("排序");

                entity.Property(e => e.Type).HasComment("部门类型");
            });

            modelBuilder.Entity<SysEarlyWarningPlan>(entity =>
            {
                entity.ToTable("Sys_EarlyWarningPlan");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Contents).HasMaxLength(512);

                entity.Property(e => e.IsEnable).HasComment("是否有效");

                entity.Property(e => e.Title).HasMaxLength(64);
            });

            modelBuilder.Entity<SysLog>(entity =>
            {
                entity.ToTable("Sys_Log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("IP")
                    .HasMaxLength(20)
                    .HasComment("ip");

                entity.Property(e => e.LogAction)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("发生的 action");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasComment("日志时间");

                entity.Property(e => e.LogLevel)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("日志等级");

                entity.Property(e => e.LogLogger)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("发生的控制器");

                entity.Property(e => e.LogMessage)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasComment("日志描述");

                entity.Property(e => e.LogType).HasComment("日志类型");

                entity.Property(e => e.LogUrl)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("请求地址");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("操作人");
            });

            modelBuilder.Entity<SysModule>(entity =>
            {
                entity.HasKey(e => e.ModuleNum);

                entity.ToTable("Sys_Module");

                entity.Property(e => e.ModuleNum)
                    .HasComment("模块编号")
                    .ValueGeneratedNever();

                entity.Property(e => e.HttpMethod)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("请求方式");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("图标");

                entity.Property(e => e.IsEnable).HasComment("是否启用");

                entity.Property(e => e.IsMenu).HasComment("是否是菜单模块");

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("模块名称");

                entity.Property(e => e.Pnum)
                    .HasColumnName("PNum")
                    .HasComment("父级");

                entity.Property(e => e.Sort).HasComment("排序");

                entity.Property(e => e.Target).HasComment("打开方式");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(200)
                    .HasComment("请求地址");
            });

            modelBuilder.Entity<SysModuleButton>(entity =>
            {
                entity.HasKey(e => e.ModuleButtonId);

                entity.ToTable("Sys_ModuleButton");

                entity.Property(e => e.ModuleButtonId)
                    .HasColumnName("ModuleButtonID")
                    .HasComment("编号")
                    .ValueGeneratedNever();

                entity.Property(e => e.ButtionIcon)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("按钮图标");

                entity.Property(e => e.ButtonClass)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("按钮样式");

                entity.Property(e => e.ButtonId)
                    .IsRequired()
                    .HasColumnName("ButtonID")
                    .HasMaxLength(50)
                    .HasComment("按钮ID（html）");

                entity.Property(e => e.ButtonName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("按钮名称");

                entity.Property(e => e.HttpMethod)
                    .HasMaxLength(10)
                    .HasComment("请求方式");

                entity.Property(e => e.IsEnable).HasComment("是否启用");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("ModuleID")
                    .HasComment("对应的模块主键");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("父级");

                entity.Property(e => e.Sort).HasComment("排序");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(200)
                    .HasComment("请求地址");
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("Sys_Role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsEnable).HasComment("是否有效");

                entity.Property(e => e.Remark)
                    .HasMaxLength(300)
                    .HasComment("备注");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("角色名称");

                entity.Property(e => e.Sort).HasComment("排序");
            });

            modelBuilder.Entity<SysRoleModule>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.ModuleId, e.Type });

                entity.ToTable("Sys_RoleModule");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasComment("角色主键");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("ModuleID")
                    .HasComment("模块编号");

                entity.Property(e => e.Type).HasComment("1：模块权限   2：按钮权限");
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Sys_User");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("帐号");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.Department).HasComment("部门主键");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasComment("邮箱");

                entity.Property(e => e.ErrorTimes).HasComment("错误次数");

                entity.Property(e => e.Gender).HasComment("性别   0 男  1 女");

                entity.Property(e => e.HeadIcon)
                    .HasMaxLength(200)
                    .HasComment("头像");

                entity.Property(e => e.Imei)
                    .HasColumnName("IMEI")
                    .HasMaxLength(50);

                entity.Property(e => e.IsAdmin).HasComment("是否是管理员");

                entity.Property(e => e.IsEnable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否有效  启用禁用账户");

                entity.Property(e => e.IsLock).HasComment("是否锁定");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("datetime")
                    .HasComment("最后登录时间");

                entity.Property(e => e.LitmitTime).HasComment("验证有效时间");

                entity.Property(e => e.NickName)
                    .HasMaxLength(20)
                    .HasComment("昵称");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("密码");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasComment("电话");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.Property(e => e.WeChatKey)
                    .HasMaxLength(50)
                    .HasComment("微信标识");
            });

            modelBuilder.Entity<SysUserModule>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ModuleId, e.Type });

                entity.ToTable("Sys_UserModule");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("用户主键");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("ModuleID")
                    .HasComment("模块编号");

                entity.Property(e => e.Type).HasComment("1：模块权限   2：按钮权限");
            });

            modelBuilder.Entity<SysUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Role });

                entity.ToTable("Sys_UserRole");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");

                entity.Property(e => e.Describe).HasMaxLength(100);

                entity.Property(e => e.Hp)
                    .HasColumnName("HP")
                    .HasMaxLength(11);

                entity.Property(e => e.Imei)
                    .HasColumnName("IMEI")
                    .HasMaxLength(15);

                entity.Property(e => e.Phone).HasMaxLength(11);

                entity.Property(e => e.Region).HasMaxLength(20);

                entity.Property(e => e.UserEmail).HasMaxLength(50);

                entity.Property(e => e.UserFullName).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.WeChatId)
                    .HasColumnName("WeChatID")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ViewDeviceInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_DeviceInfo");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.DeviceName).HasMaxLength(50);

                entity.Property(e => e.DeviceNum).HasMaxLength(50);

                entity.Property(e => e.DeviceTypeZys).HasColumnName("DeviceType_ZYS");

                entity.Property(e => e.Rtuid).HasColumnName("RTUID");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<WarnData>(entity =>
            {
                entity.HasKey(e => new { e.DeviceId, e.StationId, e.RuleId });

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.RuleId)
                    .HasColumnName("RuleID")
                    .HasComment("规则主键");

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<WarnData1>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.RuleId)
                    .HasColumnName("RuleID")
                    .HasComment("规则主键");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WarnRule>(entity =>
            {
                entity.HasKey(e => e.RuleId);

                entity.Property(e => e.RuleId)
                    .HasColumnName("RuleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Partition).HasDefaultValueSql("((1))");

                entity.Property(e => e.RuleSql).IsRequired();

                entity.Property(e => e.RuleText).IsRequired();
            });

            modelBuilder.Entity<WarnRuleDetail>(entity =>
            {
                entity.ToTable("WarnRule_Detail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompareSymbol)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment(">,<");

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.Num).HasComment("条件编号，排序");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("主表id");

                entity.Property(e => e.RelateSymbol)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("and or");

                entity.Property(e => e.Value).HasComment("设定值");
            });

            modelBuilder.Entity<WoAreaInfo>(entity =>
            {
                entity.ToTable("WO_AreaInfo");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FillColor).HasMaxLength(50);

                entity.Property(e => e.Pid).HasColumnName("PID");
            });

            modelBuilder.Entity<WoAreaRtu>(entity =>
            {
                entity.HasKey(e => new { e.AreaId, e.EquipmentId, e.StationId });

                entity.ToTable("WO_AreaRTU");

                entity.Property(e => e.AreaId).HasColumnName("AreaID");

                entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<WoAssignmentPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);

                entity.ToTable("WO_AssignmentPlan");

                entity.Property(e => e.PlanId)
                    .HasColumnName("PlanID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BeginDate).HasColumnType("datetime");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Dmaid).HasColumnName("DMAID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Gis).IsUnicode(false);

                entity.Property(e => e.IsFinish).HasDefaultValueSql("((0))");

                entity.Property(e => e.PlanName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TemplateId)
                    .HasColumnName("TemplateID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TemplatePlanId).HasColumnName("TemplatePlanID");

                entity.Property(e => e.UniqueTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<WoEvents>(entity =>
            {
                entity.HasKey(e => e.IncidentId);

                entity.ToTable("WO_Events");

                entity.Property(e => e.IncidentId)
                    .HasColumnName("IncidentID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.IncidentContent).HasMaxLength(500);

                entity.Property(e => e.IncidentNum).HasMaxLength(50);

                entity.Property(e => e.Lat).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Lng).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.ReportTime).HasColumnType("datetime");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.Type).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<WoFbOfTemplate>(entity =>
            {
                entity.HasKey(e => new { e.TemplateId, e.FeedBackId });

                entity.ToTable("WO_FbOfTemplate");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.FeedBackId).HasColumnName("FeedBackID");
            });

            modelBuilder.Entity<WoFbtemplate>(entity =>
            {
                entity.HasKey(e => e.TemplateId);

                entity.ToTable("WO_FBTemplate");

                entity.Property(e => e.TemplateId)
                    .HasColumnName("TemplateID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DataId)
                    .IsRequired()
                    .HasColumnName("DataID")
                    .IsUnicode(false);

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<WoFeedBackInfo>(entity =>
            {
                entity.ToTable("WO_FeedBackInfo");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FeedBackName).HasMaxLength(50);

                entity.Property(e => e.Type).HasComment(@"1：数值类型
2：字符串类型
3：布尔类型");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'#')");
            });

            modelBuilder.Entity<WoForward>(entity =>
            {
                entity.ToTable("WO_Forward");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("申请ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingTime)
                    .HasColumnType("datetime")
                    .HasComment("审核时间");

                entity.Property(e => e.Auditor).HasComment("审核人");

                entity.Property(e => e.CompleteTime)
                    .HasColumnType("datetime")
                    .HasComment("完成时间");

                entity.Property(e => e.ExtensionTime)
                    .HasColumnType("datetime")
                    .HasComment("申请时间");

                entity.Property(e => e.ForwardTime).HasColumnType("datetime");

                entity.Property(e => e.RecipientId)
                    .HasColumnName("RecipientID")
                    .HasComment("接收人");

                entity.Property(e => e.Remake)
                    .HasMaxLength(500)
                    .HasComment("备注");

                entity.Property(e => e.State).HasComment("状态（1.审核通过2.不通过3.未审核）");

                entity.Property(e => e.Type).HasComment("申请类型（1.退单、2.移交）");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("申请人");

                entity.Property(e => e.Woid)
                    .HasColumnName("WOID")
                    .HasComment("工单 ID");
            });

            modelBuilder.Entity<WoInsExtension>(entity =>
            {
                entity.ToTable("WO_InsExtension");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingTime).HasColumnType("datetime");

                entity.Property(e => e.CompleteTime).HasColumnType("datetime");

                entity.Property(e => e.ExtensionTime).HasColumnType("datetime");

                entity.Property(e => e.OldCompleteTime).HasColumnType("datetime");

                entity.Property(e => e.PlanId).HasColumnName("PlanID");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<WoInsForward>(entity =>
            {
                entity.ToTable("WO_InsForward");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("申请ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingTime)
                    .HasColumnType("datetime")
                    .HasComment("审核时间");

                entity.Property(e => e.Auditor).HasComment("审核人");

                entity.Property(e => e.CompleteTime)
                    .HasColumnType("datetime")
                    .HasComment("完成时间");

                entity.Property(e => e.ExtensionTime)
                    .HasColumnType("datetime")
                    .HasComment("申请时间");

                entity.Property(e => e.ForwardTime).HasColumnType("datetime");

                entity.Property(e => e.OldPlanId).HasColumnName("OldPlanID");

                entity.Property(e => e.PlanId)
                    .HasColumnName("PlanID")
                    .HasComment("工单 ID");

                entity.Property(e => e.RecipientId)
                    .HasColumnName("RecipientID")
                    .HasComment("接收人");

                entity.Property(e => e.Remake)
                    .HasMaxLength(500)
                    .HasComment("备注");

                entity.Property(e => e.State).HasComment("状态（1.审核通过2.不通过3.未审核）");

                entity.Property(e => e.Type).HasComment("申请类型（1.退单、2.移交）");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("申请人");
            });

            modelBuilder.Entity<WoInspectPlanCheck>(entity =>
            {
                entity.ToTable("WO_InspectPlanCheck");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DetailContent).HasMaxLength(2000);

                entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");

                entity.Property(e => e.Gislocation)
                    .HasColumnName("GISLocation")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.PlanId).HasColumnName("PlanID");
            });

            modelBuilder.Entity<WoInspectionPlan>(entity =>
            {
                entity.ToTable("WO_InspectionPlan");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("主键ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BeginTime)
                    .HasColumnType("datetime")
                    .HasComment("开始时间");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.CreateUser).HasComment("创建人");

                entity.Property(e => e.Cycle).HasComment("巡检周期");

                entity.Property(e => e.DayNums)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dmaid)
                    .HasColumnName("DMAID")
                    .HasComment("分区ID");

                entity.Property(e => e.EnabledMark).HasComment("是否启用");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasComment("结束时间");

                entity.Property(e => e.Inspector).HasComment("巡检员");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasComment("修改时间（备用）");

                entity.Property(e => e.PlanName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("计划名称");

                entity.Property(e => e.Remark).HasComment("备注");

                entity.Property(e => e.ScheduleId)
                    .HasColumnName("ScheduleID")
                    .HasMaxLength(100);

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.Travel).HasComment("行进方式");
            });

            modelBuilder.Entity<WoPlanInspectO>(entity =>
            {
                entity.HasKey(e => new { e.PlanId, e.InspectObject, e.IsTemplate, e.PumpStationId });

                entity.ToTable("WO_PlanInspectO");

                entity.Property(e => e.PlanId).HasColumnName("PlanID");

                entity.Property(e => e.PumpStationId).HasColumnName("PumpStationID");

                entity.Property(e => e.ObjectName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            });

            modelBuilder.Entity<WoResource>(entity =>
            {
                entity.ToTable("WO_Resource");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.Suffix).HasMaxLength(50);
            });

            modelBuilder.Entity<WoTeamInfo>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.ToTable("WO_TeamInfo");

                entity.Property(e => e.TeamId)
                    .HasColumnName("TeamID")
                    .ValueGeneratedNever();

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<WoTeamUser>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId });

                entity.ToTable("WO_TeamUser");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<WoTemplateInfo>(entity =>
            {
                entity.ToTable("WO_TemplateInfo");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.TemplateName).HasMaxLength(50);
            });

            modelBuilder.Entity<WoWoextension>(entity =>
            {
                entity.ToTable("WO_WOExtension");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditingTime).HasColumnType("datetime");

                entity.Property(e => e.CompleteTime).HasColumnType("datetime");

                entity.Property(e => e.ExtensionTime).HasColumnType("datetime");

                entity.Property(e => e.OldCompleteTime).HasColumnType("datetime");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Woid).HasColumnName("WOID");
            });

            modelBuilder.Entity<WoWooperation>(entity =>
            {
                entity.ToTable("WO_WOOperation");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.OperationTime).HasColumnType("datetime");

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<WoWorkOrder>(entity =>
            {
                entity.HasKey(e => e.Woid);

                entity.ToTable("WO_WorkOrder");

                entity.Property(e => e.Woid)
                    .HasColumnName("WOID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompleteTime).HasColumnType("datetime");

                entity.Property(e => e.EarlyWarningPlanId)
                    .HasColumnName("EarlyWarningPlanID")
                    .HasMaxLength(50);

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.ReleaseTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
