using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class WarnRuleService:BaseService,IWarnRuleService
    {
       
        //待测
        public IEnumerable<dynamic> GetDetailRuleByID(int ruleid)
        {
            var compare_fitem = (int)WarnRule_enum.比较符号;
            var relate_fitem = (int)WarnRule_enum.关系符号;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@ruleid",ruleid),
                new SqlParameter("@compare_fitem",compare_fitem),
                new SqlParameter("@relate_fitem",relate_fitem)
            };
            string sql = @" select r.*,m1.ItemName as CompareSName,m2.ItemName as RelateSName,d.CNName from [dbo].[WarnRule_Detail] r
 left join [dbo].[Sws_DataInfo] d on r.DataID=d.DataID and DeviceType=1 and DataType=1
 left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@compare_fitem and IsEnable=1) m1 on r.CompareSymbol=m1.ItemValue
 left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@relate_fitem and IsEnable=1) m2 on r.RelateSymbol=m2.ItemValue
 where ParentID=@ruleid order by Num asc";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //根据子规则id获取对应子规则的数据
        public IEnumerable<dynamic> GetRuleByDetailID(int detailid)
        {
            var compare_fitem = (int)WarnRule_enum.比较符号;
            var relate_fitem = (int)WarnRule_enum.关系符号;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@detailid",detailid),
                new SqlParameter("@compare_fitem",compare_fitem),
                new SqlParameter("@relate_fitem",relate_fitem)
            };
            string sql = @" select r.*,m1.ItemName as CompareSName,m2.ItemName as RelateSName,d.CNName from [dbo].[WarnRule_Detail] r
 left join [dbo].[Sws_DataInfo] d on r.DataID=d.DataID and DeviceType=1 and DataType=1
 left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@compare_fitem and IsEnable=1) m1 on r.CompareSymbol=m1.ItemValue
 left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@relate_fitem and IsEnable=1) m2 on r.RelateSymbol=m2.ItemValue
 where r.ID=@detailid order by Num asc";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        public int SetRuleInfo(WarnRuleDetail rd, int RuleId,byte partition)
        {
            var compare_fitem = (int)WarnRule_enum.比较符号;
            var relate_fitem = (int)WarnRule_enum.关系符号;
            var RuleDetailList = this.Context.Set<WarnRuleDetail>().Where(r => r.ParentId == RuleId).AsNoTracking().ToList();
            var oldRule= this.Context.Set<WarnRule>().Where(r => r.RuleId == RuleId).AsNoTracking().FirstOrDefault();
           
            var oldRuleDetail = RuleDetailList.Where(r => r.Id == rd.Id).FirstOrDefault();
            List<WarnRuleDetail> appendList = RuleDetailList.Where(r => r.Id != rd.Id).ToList();
            appendList.Add(rd);
            var dataidList = appendList.Select(r => r.DataId).ToList();
            var swsDataInfo = this.Context.Set<SwsDataInfo>().Where(r => r.DeviceType == 1 && r.DataType == 1&& dataidList.Contains(r.DataId)).ToList();
            var dictorydata = this.Context.Set<SysDataItemDetail>().Where(r=>r.FItemId== compare_fitem || r.FItemId== relate_fitem).ToList();
            if (oldRuleDetail == null)//添加
            {
                this.Context.Set<WarnRuleDetail>().Add(rd);
            }
            else//修改
            {
                this.Context.Set<WarnRuleDetail>().Update(rd);
            }
            appendList = appendList.OrderBy(r => r.Num).ToList();
            var i = 1;
            StringBuilder RuleText = new StringBuilder();
            StringBuilder RuleSql = new StringBuilder();
            string RuleSqlstr = "";
            if (appendList.Count == 1)
            {
                foreach (var item in appendList)
                {
                    var datainfo = swsDataInfo.Where(r => r.DataId == item.DataId).FirstOrDefault();
                    var compareS = dictorydata.Where(r => r.FItemId == compare_fitem && r.ItemValue == item.CompareSymbol).FirstOrDefault();
                    var relateS = dictorydata.Where(r => r.FItemId == relate_fitem && r.ItemValue == item.RelateSymbol).FirstOrDefault();
                    

                    RuleText.Append(datainfo?.Cnname + compareS?.ItemName + item.Value);
                    
                    RuleSql.Append("{'AnalogValues." + item.DataId + "':"+ GetMongoSymbol(item.CompareSymbol, item.Value) + "}");


                }
                RuleSqlstr = RuleSql.ToString();
            }
            else if(appendList.Count>1)
            {
                RuleSql.Append("{'$and':[");
                foreach (var item in appendList)
                {
                    var datainfo = swsDataInfo.Where(r => r.DataId == item.DataId).FirstOrDefault();
                    var compareS = dictorydata.Where(r => r.FItemId == compare_fitem && r.ItemValue == item.CompareSymbol).FirstOrDefault();
                    var relateS = dictorydata.Where(r => r.FItemId == relate_fitem && r.ItemValue == item.RelateSymbol).FirstOrDefault();
                    if (i == 1)
                    {

                        RuleText.Append(datainfo?.Cnname + compareS?.ItemName + item.Value);
                       // RuleSql.Append(item.DataId + " " + item.CompareSymbol + " " + item.Value);

                    }
                    else
                    {
                        RuleText.Append(" " + relateS?.ItemName + " " + datainfo?.Cnname + compareS?.ItemName + item.Value);
                        //RuleSql.Append(" " + item.RelateSymbol + " " + item.DataId + " " + item.CompareSymbol + " " + item.Value);
                    }
                    i = i + 1;
                    RuleSql.Append("{'AnalogValues." + item.DataId + "':" + GetMongoSymbol(item.CompareSymbol, item.Value) + "},");
                }
                 RuleSqlstr = RuleSql.ToString();
                RuleSqlstr = RuleSqlstr.Substring(0, RuleSqlstr.Length-1)+"]}";
            }
           

            if (oldRule == null)//添加
            {
                WarnRule r = new WarnRule();
                r.RuleId = RuleId;
                r.RuleText = RuleText.ToString();
                r.RuleSql = RuleSqlstr;
                r.Partition = partition;
                this.Context.Set<WarnRule>().Add(r);
            }
            else//修改
            {
                WarnRule r = new WarnRule();
                r.RuleId = RuleId;
                r.RuleText = RuleText.ToString();
                r.RuleSql = RuleSqlstr;
                r.Partition = oldRule.Partition;
                this.Context.Set<WarnRule>().Update(r);
            }
            return this.Context.SaveChanges();
        }
        string GetMongoSymbol(string CompareSymbol,double Value)
        {
            var mongoStr = "";
            if (CompareSymbol == ">")
            {
                mongoStr ="{"+ "'$gt':"+ Value + ""+"}";
            }
            else if (CompareSymbol == ">=")
            {
                
                mongoStr = "{" + "'$gte':" + Value + "" + "}";
            }
            else if (CompareSymbol == "<=")
            {
                
                mongoStr = "{" + "'$lte':" + Value + "" + "}";
            }
            else if (CompareSymbol == "<")
            {
                
                mongoStr = "{" + "'$lt':" + Value + "" + "}";
            }
            return mongoStr;
        }
        //删除子规则
        public int Del_DetailRule(int detailID,int ruleID)
        {
            var compare_fitem = (int)WarnRule_enum.比较符号;
            var relate_fitem = (int)WarnRule_enum.关系符号;
            var RuleDetailList = this.Context.Set<WarnRuleDetail>().Where(r => r.ParentId == ruleID).AsNoTracking().ToList();
            var oldRule = this.Context.Set<WarnRule>().Where(r => r.RuleId == ruleID).AsNoTracking().FirstOrDefault();
            var delteDetailRule = RuleDetailList.Where(r => r.Id == detailID).FirstOrDefault();
            var dataidList = RuleDetailList.Select(r => r.DataId).ToList();
            var swsDataInfo = this.Context.Set<SwsDataInfo>().Where(r => r.DeviceType == 1 && r.DataType == 1 && dataidList.Contains(r.DataId)).ToList();
            var dictorydata = this.Context.Set<SysDataItemDetail>().Where(r => r.FItemId == compare_fitem || r.FItemId == relate_fitem).ToList();
            if (delteDetailRule != null)
            {
                this.Context.Set<WarnRuleDetail>().Remove(delteDetailRule);//删除子规则
            }
            if (RuleDetailList.Count > 0)
            {
                RuleDetailList.Remove(delteDetailRule);
                if (RuleDetailList.Count > 0)
                {
                    RuleDetailList = RuleDetailList.OrderBy(r => r.Num).ToList();
                    var i = 1;
                    StringBuilder RuleText = new StringBuilder();
                    StringBuilder RuleSql = new StringBuilder();
                    string RuleSqlstr = "";
                    if (RuleDetailList.Count == 1)
                    {
                        foreach (var item in RuleDetailList)
                        {
                            var datainfo = swsDataInfo.Where(r => r.DataId == item.DataId).FirstOrDefault();
                            var compareS = dictorydata.Where(r => r.FItemId == compare_fitem && r.ItemValue == item.CompareSymbol).FirstOrDefault();
                            var relateS = dictorydata.Where(r => r.FItemId == relate_fitem && r.ItemValue == item.RelateSymbol).FirstOrDefault();


                            RuleText.Append(datainfo?.Cnname + compareS?.ItemName + item.Value);

                            RuleSql.Append("{'AnalogValues." + item.DataId + "':" + GetMongoSymbol(item.CompareSymbol, item.Value) + "}");


                        }
                        RuleSqlstr = RuleSql.ToString();
                    }
                    else if (RuleDetailList.Count > 1)
                    {
                        RuleSql.Append("{'$and':[");
                        foreach (var item in RuleDetailList)
                        {
                            var datainfo = swsDataInfo.Where(r => r.DataId == item.DataId).FirstOrDefault();
                            var compareS = dictorydata.Where(r => r.FItemId == compare_fitem && r.ItemValue == item.CompareSymbol).FirstOrDefault();
                            var relateS = dictorydata.Where(r => r.FItemId == relate_fitem && r.ItemValue == item.RelateSymbol).FirstOrDefault();
                            if (i == 1)
                            {

                                RuleText.Append(datainfo?.Cnname + compareS?.ItemName + item.Value);
                                // RuleSql.Append(item.DataId + " " + item.CompareSymbol + " " + item.Value);

                            }
                            else
                            {
                                RuleText.Append(" " + relateS?.ItemName + " " + datainfo?.Cnname + compareS?.ItemName + item.Value);
                                //RuleSql.Append(" " + item.RelateSymbol + " " + item.DataId + " " + item.CompareSymbol + " " + item.Value);
                            }
                            i = i + 1;
                            RuleSql.Append("{'AnalogValues." + item.DataId + "':" + GetMongoSymbol(item.CompareSymbol, item.Value) + "},");
                        }
                        RuleSqlstr = RuleSql.ToString();
                        RuleSqlstr = RuleSqlstr.Substring(0, RuleSqlstr.Length - 1) + "]}";
                    }



                    //foreach (var item in RuleDetailList)
                    //{
                    //    var datainfo = swsDataInfo.Where(r => r.DataId == item.DataId).FirstOrDefault();
                    //    var compareS = dictorydata.Where(r => r.FItemId == compare_fitem && r.ItemValue == item.CompareSymbol).FirstOrDefault();
                    //    var relateS = dictorydata.Where(r => r.FItemId == relate_fitem && r.ItemValue == item.RelateSymbol).FirstOrDefault();
                    //    if (i == 1)
                    //    {

                    //        RuleText.Append(datainfo?.Cnname + compareS?.ItemName + item.Value);
                    //        RuleSql.Append(item.DataId + " " + item.CompareSymbol + " " + item.Value);
                    //    }
                    //    else
                    //    {
                    //        RuleText.Append(" " + relateS?.ItemName+" " + datainfo?.Cnname + compareS?.ItemName + item.Value);
                    //        RuleSql.Append(" " + item.RelateSymbol + " " + item.DataId + " " + item.CompareSymbol + " " + item.Value);
                    //    }
                    //    i = i + 1;
                    //}
                    oldRule.RuleText = RuleText.ToString();
                    oldRule.RuleSql = RuleSqlstr;
                    this.Context.Set<WarnRule>().Update(oldRule);
                }
                else
                {
                    if (oldRule != null)
                    {
                        this.Context.Set<WarnRule>().Remove(oldRule);
                    }
                }

            }
            return this.Context.SaveChanges();
        }
        //删除规则
        public int DeleteRules(List<int> ruleIDs)
        {
            var rules = this.Context.Set<WarnRule>().Where(r => ruleIDs.Contains(r.RuleId)).ToList();
            var detailRules= this.Context.Set<WarnRuleDetail>().Where(r => ruleIDs.Contains(r.ParentId)).ToList();
            if (rules.Count > 0)
            {
                this.Context.Set<WarnRule>().RemoveRange(rules);
            }
            if (detailRules.Count > 0)
            {
                this.Context.Set<WarnRuleDetail>().RemoveRange(detailRules);
            }
            return this.Context.SaveChanges();
        }
        #region 预警报告
        public PageResult<warnReport> LoadWarnDataList(int userid,Expression<Func<warnReport, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userid",userid)
            };
            string sql = @" select w.DeviceID, w.StationID, Data, d.DeviceName,RuleText,w.RuleID,UpdateTime from [dbo].[WarnData] w
  left join [dbo].[Sws_DeviceInfo01] d on w.DeviceID=d.DeviceID and w.StationID=d.StationID
  left join [dbo].[WarnRule] r on w.RuleID=r.RuleID";
            if (userid != 0)
            {
                sql += " left join [dbo].[Sws_UserStation] s on w.StationID=s.StationID where UserID=@userid";
            }
  //          sql = @" select w.DeviceID, w.StationID, d.DeviceName,RuleText,w.DataID,Value,w.RuleID,f.CNName from [dbo].[WarnData1] w
  // left join [dbo].[Sws_DeviceInfo01] d on w.DeviceID=d.DeviceID and w.StationID=d.StationID
  //left join [dbo].[WarnRule] r on w.RuleID=r.RuleID
  //left join [dbo].[Sws_DataInfo] f on f.DataID=w.DataID and f.DeviceType=r.Partition";

            PageResult<warnReport> presult = new PageResult<warnReport>();
            presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            return presult;
        }
        //查询当前帐号下预警数量
        public int GetWarnCount(int userid)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userid",userid)
            };
            string sql = "select count(*) as num from [dbo].[WarnData]";
            if (userid != 0)
            {
                sql += " w left join[dbo].[Sws_UserStation] s on w.StationID=s.StationID where UserID=@userid";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter).FirstOrDefault().num;
            return query;
        }
        #endregion
    }
}
