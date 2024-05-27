
let viewgird = [
    {
        path: '/Sys_Log',
        name: 'sys_Log',
        component: () => import('@/views/system/Sys_Log.vue')
    },
    {
        path: '/Sys_User',
        name: 'Sys_User',
        component: () => import('@/views/system/Sys_User.vue')
    },
    {
        path: '/permission',
        name: 'permission',
        component: () => import('@/views/system/Permission.vue')
    },
    {
        path: '/Sys_Dictionary',
        name: 'Sys_Dictionary',
        component: () => import('@/views/system/Sys_Dictionary.vue')
    },
    {
        path: '/Sys_Role',
        name: 'Sys_Role',
        component: () => import('@/views/system/Sys_Role.vue')
    }, {
        path: '/Sys_Role1',
        name: 'Sys_Role1',
        component: () => import('@/views/system/Sys_Role1.vue')
    }
    , {
        path: '/Sys_DictionaryList',
        name: 'Sys_DictionaryList',
        component: () => import('@/views/system/Sys_DictionaryList.vue')
    }, {
        path: '/FormDesignOptions',
        name: 'FormDesignOptions',
        component: () => import('@/views/system/form/FormDesignOptions.vue')
    }, {
        path: '/FormCollectionObject',
        name: 'FormCollectionObject',
        component: () => import('@/views/system/form/FormCollectionObject.vue')
    }, {
        path: '/Custom/Sys_User_ExtendData',
        name: 'Sys_User_ExtendData',
        component: () => import('@/views/custom/custom/Sys_User_ExtendData.vue')
    }, {
        path: '/Sys_Table_Extend',
        name: 'Sys_Table_Extend',
        component: () => import('@/views/custom/custom/Sys_Table_Extend.vue')
    }, {
        path: '/Sys_User_Extend',
        name: 'Sys_User_Extend',
        component: () => import('@/views/custom/custom/Sys_User_Extend.vue')
    }
    , {
        path: '/Base_DefectItem_Extend',
        name: 'Base_DefectItem_Extend',
        component: () => import('@/views/custom/custom/Base_DefectItem_Extend.vue')
    }
    , {
        path: '/Base_MeritPay_Extend',
        name: 'Base_MeritPay_Extend',
        component: () => import('@/views/custom/custom/Base_MeritPay_Extend.vue')
    }
    , {
        path: '/Base_Process_Extend',
        name: 'Base_Process_Extend',
        component: () => import('@/views/custom/custom/Base_Process_Extend.vue')
    }
    , {
        path: '/Base_Product_Extend',
        name: 'Base_Product_Extend',
        component: () => import('@/views/custom/custom/Base_Product_Extend.vue')
    }, {
        path: '/Sys_User_ExtendData',
        name: 'Sys_User_ExtendData',
        component: () => import('@/views/custom/custom/Sys_User_ExtendData.vue')
    }, {
        path: '/Sys_Dept',
        name: 'Sys_Dept',
        component: () => import('@/views/system/Sys_Dept.vue')
    }, {
        path: '/Sys_Unit',
        name: 'Sys_Unit',
        component: () => import('@/views/custom/custom/Sys_Unit.vue')
    }, {
        path: '/Base_DefectItem',
        name: 'Base_DefectItem',
        component: () => import('@/views/custom/custom/Base_DefectItem.vue')
    }, {
        path: '/Base_Process',
        name: 'Base_Process',
        component: () => import('@/views/custom/custom/Base_Process.vue')
    }, {
        path: '/Base_ProcessList',
        name: 'Base_ProcessList',
        component: () => import('@/views/custom/custom/Base_ProcessList.vue')
    }, {
        path: '/Base_NumberRule',
        name: 'Base_NumberRule',
        component: () => import('@/views/custom/custom/Base_NumberRule.vue')
    }, {
        path: '/Base_ProcessLine',
        name: 'Base_ProcessLine',
        component: () => import('@/views/custom/custom/Base_ProcessLine.vue'),
        meta: {
            keepAlive: false
        }
    }, {
        path: '/Base_ProcessLineList',
        name: 'Base_ProcessLineList',
        component: () => import('@/views/custom/custom/Base_ProcessLineList.vue'),
        meta: {
            keepAlive: false
        }
    }, {
        path: '/Base_MeritPay',
        name: 'Base_MeritPay',
        component: () => import('@/views/custom/custom/Base_MeritPay.vue')
    }, {
        path: '/Base_Product',
        name: 'Base_Product',
        component: () => import('@/views/custom/custom/Base_Product.vue')
    }, {
        path: '/View_Base_MaterialDetail',
        name: 'View_Base_MaterialDetail',
        component: () => import('@/views/custom/custom/View_Base_MaterialDetail.vue')
    }, {
        path: '/Ware_WareHouseBill',
        name: 'Ware_WareHouseBill',
        component: () => import('@/views/warehouse/warehouse/Ware_WareHouseBill.vue')
    }, {
        path: '/Ware_WareHouseBillList',
        name: 'Ware_WareHouseBillList',
        component: () => import('@/views/warehouse/warehouse/Ware_WareHouseBillList.vue')
    }, {
        path: '/Ware_OutWareHouseBillList',
        name: 'Ware_OutWareHouseBillList',
        component: () => import('@/views/warehouse/warehouse/Ware_OutWareHouseBillList.vue')
    }, {
        path: '/Ware_OutWareHouseBill',
        name: 'Ware_OutWareHouseBill',
        component: () => import('@/views/warehouse/warehouse/Ware_OutWareHouseBill.vue')
    }, {
        path: '/View_WareInOutDetail',
        name: 'View_WareInOutDetail',
        component: () => import('@/views/warehouse/warehouse/View_WareInOutDetail.vue')
    }, {
        path: '/View_StockBalance',
        name: 'View_StockBalance',
        component: () => import('@/views/warehouse/warehouse/View_StockBalance.vue')
    }, {
        path: '/Production_SalesOrderList',
        name: 'Production_SalesOrderList',
        component: () => import('@/views/production/production/Production_SalesOrderList.vue')
    }, {
        path: '/Production_SalesOrder',
        name: 'Production_SalesOrder',
        component: () => import('@/views/production/production/Production_SalesOrder.vue')
    }, {
        path: '/Production_ProductPlanList',
        name: 'Production_ProductPlanList',
        component: () => import('@/views/production/production/Production_ProductPlanList.vue')
    }, {
        path: '/Production_ProductPlan',
        name: 'Production_ProductPlan',
        component: () => import('@/views/production/production/Production_ProductPlan.vue')
    }, {
        path: '/Production_AssembleWorkOrderList',
        name: 'Production_AssembleWorkOrderList',
        component: () => import('@/views/production/production/Production_AssembleWorkOrderList.vue')
    }, {
        path: '/Production_AssembleWorkOrder',
        name: 'Production_AssembleWorkOrder',
        component: () => import('@/views/production/production/Production_AssembleWorkOrder.vue')
    }, {
        path: '/Production_WorkOrder',
        name: 'Production_WorkOrder',
        component: () => import('@/views/production/production/Production_WorkOrder.vue')
    }, {
        path: '/Production_WorkOrderList',
        name: 'Production_WorkOrderList',
        component: () => import('@/views/production/production/Production_WorkOrderList.vue')
    }, {
        path: '/Base_PrintCatalog',
        name: 'Base_PrintCatalog',
        component: () => import('@/views/custom/custom/Base_PrintCatalog.vue')
    }, {
        path: '/Base_PrintTemplate',
        name: 'Base_PrintTemplate',
        component: () => import('@/views/custom/custom/Base_PrintTemplate.vue')
    }, {
        path: '/Base_PrintTemplateTree',
        name: 'Base_PrintTemplateTree',
        component: () => import('@/views/custom/custom/Base_PrintTemplateTree.vue')
    }, {
        path: '/Production_ReportWorkOrder',
        name: 'Production_ReportWorkOrder',
        component: () => import('@/views/production/production/Production_ReportWorkOrder.vue')
    }, {
        path: '/Production_ReportWorkOrderList',
        name: 'Production_ReportWorkOrderList',
        component: () => import('@/views/production/production/Production_ReportWorkOrderList.vue')
    }, {
        path: '/View_OutputStatistics',
        name: 'View_OutputStatistics',
        component: () => import('@/views/report/report/View_OutputStatistics.vue')
    }, {
        path: '/Sys_WorkFlowTable',
        name: 'Sys_WorkFlowTable',
        component: () => import('@/views/system/flow/Sys_WorkFlowTable.vue')
    }, {
        path: '/Sys_WorkFlow',
        name: 'Sys_WorkFlow',
        component: () => import('@/views/system/flow/Sys_WorkFlow.vue')
    }, {
        path: '/View_EmployeePerformance',
        name: 'View_EmployeePerformance',
        component: () => import('@/views/report/report/View_EmployeePerformance.vue')
    }, {
        path: '/View_SalaryReport',
        name: 'View_SalaryReport',
        component: () => import('@/views/report/report/View_SalaryReport.vue')
    }, {
        path: '/View_DefectItemDistribute',
        name: 'View_DefectItemDistribute',
        component: () => import('@/views/report/report/View_DefectItemDistribute.vue')
    }, {
        path: '/View_DefectItemSummary',
        name: 'View_DefectItemSummary',
        component: () => import('@/views/report/report/View_DefectItemSummary.vue')
    }, {
        path: '/View_ProductionReport',
        name: 'View_ProductionReport',
        component: () => import('@/views/report/report/View_ProductionReport.vue')
    }, {
        path: '/Base_Notice',
        name: 'Base_Notice',
        component: () => import('@/views/custom/custom/Base_Notice.vue')
    }, {
        path: '/Base_Product_ExtendData',
        name: 'Base_Product_ExtendData',
        component: () => import('@/views/custom/custom/Base_Product_ExtendData.vue')
    }, {
        path: '/Base_Process_ExtendData',
        name: 'Base_Process_ExtendData',
        component: () => import('@/views/custom/custom/Base_Process_ExtendData.vue')
    }, {
        path: '/Base_DefectItem_ExtendData',
        name: 'Base_DefectItem_ExtendData',
        component: () => import('@/views/custom/custom/Base_DefectItem_ExtendData.vue')
    }, {
        path: '/Base_MaterialDetail_ExtendData',
        name: 'Base_MaterialDetail_ExtendData',
        component: () => import('@/views/custom/custom/Base_MaterialDetail_ExtendData.vue')
    }, {
        path: '/Base_MeritPay_ExtendData',
        name: 'Base_MeritPay_ExtendData',
        component: () => import('@/views/custom/custom/Base_MeritPay_ExtendData.vue')
    }, {
        path: '/Sys_QuartzLog',
        name: 'Sys_QuartzLog',
        component: () => import('@/views/system/quartz/Sys_QuartzLog.vue')
    }, {
        path: '/Sys_QuartzOptions',
        name: 'Sys_QuartzOptions',
        component: () => import('@/views/system/quartz/Sys_QuartzOptions.vue')
    }, {
        path: '/Sys_VersionInfo',
        name: 'Sys_VersionInfo',
        component: () => import('@/views/system/system/Sys_VersionInfo.vue')
    }, {
        path: '/Sys_UserTree',
        name: 'Sys_UserTree',
        component: () => import('@/views/system/Sys_UserTree.vue')
    }, {
        path: '/Equip_DevCatalog',
        name: 'Equip_DevCatalog',
        component: () => import('@/views/equip/equip/Equip_DevCatalog.vue')
    }, {
        path: '/Base_WorkShop',
        name: 'Base_WorkShop',
        component: () => import('@/views/custom/custom/Base_WorkShop.vue')
    }, {
        path: '/Equip_Device',
        name: 'Equip_Device',
        component: () => import('@/views/equip/equip/Equip_Device.vue')
    }, {
        path: '/Equip_DeviceTree',
        name: 'Equip_DeviceTree',
        component: () => import('@/views/equip/equip/Equip_DeviceTree.vue')
    },
    {
        path: '/DashBoard/WorkShopBoard',
        name: 'WorkShopBoard',
        component: () => import('@/views/dashboard/WorkShopBoard.vue'),
    }, {
        path: '/Equip_SpotMaintenance',
        name: 'Equip_SpotMaintenance',
        component: () => import('@/views/equip/equip/Equip_SpotMaintenance.vue')
    }, {
        path: '/Equip_SpotMaintPlan',
        name: 'Equip_SpotMaintPlan',
        component: () => import('@/views/equip/equip/Equip_SpotMaintPlan.vue')
    }, {
        path: '/Equip_SpotMaintPlanDevice',
        name: 'Equip_SpotMaintPlanDevice',
        component: () => import('@/views/equip/equip/Equip_SpotMaintPlanDevice.vue')
    }, {
        path: '/Equip_SpotMaintPlanProject',
        name: 'Equip_SpotMaintPlanProject',
        component: () => import('@/views/equip/equip/Equip_SpotMaintPlanProject.vue')
    }, {
        path: '/Equip_MaintainPaper',
        name: 'Equip_MaintainPaper',
        component: () => import('@/views/equip/equip/Equip_MaintainPaper.vue')
    }, {
        path: '/Equip_SpotMaintWorkOrder',
        name: 'Equip_SpotMaintWorkOrder',
        component: () => import('@/views/equip/equip/Equip_SpotMaintWorkOrder.vue')
    }, {
        path: '/Cal_TeamMember',
        name: 'Cal_TeamMember',
        component: () => import('@/views/calendar/calendar/Cal_TeamMember.vue')
    }, {
        path: '/Cal_Team',
        name: 'Cal_Team',
        component: () => import('@/views/calendar/calendar/Cal_Team.vue')
    }, {
        path: '/Cal_PlanShift',
        name: 'Cal_PlanShift',
        component: () => import('@/views/calendar/calendar/Cal_PlanShift.vue')
    }, {
        path: '/Cal_PlanTeam',
        name: 'Cal_PlanTeam',
        component: () => import('@/views/calendar/calendar/Cal_PlanTeam.vue')
    }, {
        path: '/Cal_Plan',
        name: 'Cal_Plan',
        component: () => import('@/views/calendar/calendar/Cal_Plan.vue')
    }, {
        path: '/Cal_TeamShift',
        name: 'Cal_TeamShift',
        component: () => import('@/views/calendar/calendar/Cal_TeamShift.vue')
    }, {
        path: '/Cal_HolidaySec',
        name: 'Cal_HolidaySec',
        component: () => import('@/views/calendar/calendar/Cal_HolidaySec.vue')
    }, {
        path: '/Cal_Calendar',
        name: 'Cal_Calendar',
        component: () => import('@/views/calendar/calendar/calendar/index.vue')
    }, {
        path: '/Tools_ToolType',
        name: 'Tools_ToolType',
        component: () => import('@/views/tools/tools/Tools_ToolType.vue')
    }, {
        path: '/Tools_Tool',
        name: 'Tools_Tool',
        component: () => import('@/views/tools/tools/Tools_Tool.vue')
    }, {
        path: '/Tools_ToolsReceiveList',
        name: 'Tools_ToolsReceiveList',
        component: () => import('@/views/tools/tools/Tools_ToolsReceiveList.vue')
    }, {
        path: '/Tools_ToolsReceive',
        name: 'Tools_ToolsReceive',
        component: () => import('@/views/tools/tools/Tools_ToolsReceive.vue')
    }, {
        path: '/Tools_ToolsReturnList',
        name: 'Tools_ToolsReturnList',
        component: () => import('@/views/tools/tools/Tools_ToolsReturnList.vue')
    }, {
        path: '/Tools_ToolsReturn',
        name: 'Tools_ToolsReturn',
        component: () => import('@/views/tools/tools/Tools_ToolsReturn.vue')
    }, {
        path: '/Quality_Defect',
        name: 'Quality_Defect',
        component: () => import('@/views/quality/quality/Quality_Defect.vue')
    }, {
        path: '/Quality_TestItem',
        name: 'Quality_TestItem',
        component: () => import('@/views/quality/quality/Quality_TestItem.vue')
    }, {
        path: '/Quality_Template',
        name: 'Quality_Template',
        component: () => import('@/views/quality/quality/Quality_Template.vue')
    }, {
        path: '/Quality_TemplateTestItem',
        name: 'Quality_TemplateTestItem',
        component: () => import('@/views/quality/quality/Quality_TemplateTestItem.vue')
    }, {
        path: '/Quality_TemplateProduct',
        name: 'Quality_TemplateProduct',
        component: () => import('@/views/quality/quality/Quality_TemplateProduct.vue')
    }, {
        path: '/Quality_InComingCheckTestItem',
        name: 'Quality_InComingCheckTestItem',
        component: () => import('@/views/quality/quality/Quality_InComingCheckTestItem.vue')
    }, {
        path: '/Quality_InComingCheck',
        name: 'Quality_InComingCheck',
        component: () => import('@/views/quality/quality/Quality_InComingCheck.vue')
    }, {
        path: '/Quality_ProcessCheckTestItem',
        name: 'Quality_ProcessCheckTestItem',
        component: () => import('@/views/quality/quality/Quality_ProcessCheckTestItem.vue')
    }, {
        path: '/Quality_ProcessCheck',
        name: 'Quality_ProcessCheck',
        component: () => import('@/views/quality/quality/Quality_ProcessCheck.vue')
    }, {
        path: '/Quality_OutCheckTestItem',
        name: 'Quality_OutCheckTestItem',
        component: () => import('@/views/quality/quality/Quality_OutCheckTestItem.vue')
    }, {
        path: '/Quality_OutCheck',
        name: 'Quality_OutCheck',
        component: () => import('@/views/quality/quality/Quality_OutCheck.vue')
    }, {
        path: '/Base_ExcelTemplate',
        name: 'Base_ExcelTemplate',
        component: () => import('@/views/custom/custom/Base_ExcelTemplate.vue')
    }, {
        path: '/Base_ExcelTemplateTree',
        name: 'Base_ExcelTemplateTree',
        component: () => import('@/views/custom/custom/Base_ExcelTemplateTree.vue')
    }, {
        path: '/Bi_db_dim',
        name: 'Bi_db_dim',
        component: () => import('@/views/bi/bimanage/Bi_db_dim.vue')
    }, {
        path: '/Bi_db_source',
        name: 'Bi_db_source',
        component: () => import('@/views/bi/bimanage/Bi_db_source.vue')
    }, {
        path: '/Bi_db_set',
        name: 'Bi_db_set',
        component: () => import('@/views/bi/bimanage/Bi_db_set.vue')
    }, {
        path: '/Bi_db_ybp',
        name: 'Bi_db_ybp',
        component: () => import('@/views/bi/bimanage/Bi_db_ybp.vue')
    }, {
        path: '/Bi_desktop',
        name: 'Bi_desktop',
        component: () => import('@/views/bi/bimanage/Bi_desktop.vue')
    }, {
        path: '/Base_DesktopMenu',
        name: 'Base_DesktopMenu',
        component: () => import('@/views/custom/custom/Base_DesktopMenu.vue')
    }, {
        path: '/Base_MaterialDetailTree',
        name: 'Base_MaterialDetailTree',
        component: () => import('@/views/custom/custom/Base_MaterialDetailTree.vue')
    }, {
        path: '/Sys_DeptTree',
        name: 'Sys_DeptTree',
        component: () => import('@/views/system/system/Sys_DeptTree.vue')
    }, {
        path: '/Sys_Department',
        name: 'Sys_Department',
        component: () => import('@/views/system/system/Sys_Department.vue')
    }]

export default viewgird
